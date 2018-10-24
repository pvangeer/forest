using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using StoryTree.Data;
using StoryTree.Data.Properties;
using StoryTree.Storage.Create;
using StoryTree.Storage.DbContext;
using StoryTree.Storage.Read;

namespace StoryTree.Storage
{
    /// <summary>
    /// This class interacts with an SQLite database file using the Entity Framework.
    /// </summary>
    public class StorageSqLite
    {
        private StagedProject stagedProject;

        public bool HasStagedProject
        {
            get
            {
                return stagedProject != null;
            }
        }

        public void StageProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            stagedProject = new StagedProject(project, project.Create(new PersistenceRegistry()));
        }

        public void UnstageProject()
        {
            stagedProject = null;
        }

        public void SaveProjectAs(string databaseFilePath)
        {
            if (!HasStagedProject)
            {
                throw new InvalidOperationException("Call 'StageProject(IProject)' first before calling this method.");
            }

            try
            {
                var writer = new BackedUpFileWriter(databaseFilePath);
                writer.Perform(() => SaveProjectInDatabase(databaseFilePath));
            }
            catch (IOException e)
            {
                throw new StorageException(e.Message, e);
            }
            finally
            {
                UnstageProject();
            }
        }

        public Project LoadProject(string databaseFilePath)
        {
            string connectionString = GetConnectionToExistingFile(databaseFilePath);
            try
            {
                Project project;
                using (var dbContext = new Entities(connectionString))
                {
                    ValidateDatabaseVersion(dbContext, databaseFilePath);

                    dbContext.LoadTablesIntoContext();

                    ProjectEntity projectEntity;
                    try
                    {
                        projectEntity = dbContext.ProjectEntities.Local.Single();
                    }
                    catch (InvalidOperationException exception)
                    {
                        throw CreateStorageReaderException(databaseFilePath, "Geen geldige database", exception);
                    }

                    project = projectEntity.Read(new ReadConversionCollector());
                }

                return project;
            }
            catch (DataException exception)
            {
                throw CreateStorageReaderException(databaseFilePath, "Geen geldige database", exception);
            }
            catch (SystemException exception)
            {
                throw CreateStorageReaderException(databaseFilePath, "Geen geldige database", exception);
            }
        }

        public bool HasStagedProjectChanges(string filePath)
        {
            if (!HasStagedProject)
            {
                throw new InvalidOperationException("Call 'StageProject(IProject)' first before calling this method.");
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return true;
            }

            string connectionString = GetConnectionToExistingFile(filePath);
            try
            {
                byte[] originalHash;
                using (var dbContext = new Entities(connectionString))
                    originalHash = dbContext.VersionEntities.Select(v => v.FingerPrint).First();

                byte[] hash = FingerprintHelper.Get(stagedProject.Entity);
                return !FingerprintHelper.AreEqual(originalHash, hash);
            }
            catch (Exception e)
            {
                if (e.InnerException is QuotaExceededException)
                {
                    throw new StorageException("Opgeslagen project bevat teveel objecten om een vingerafdruk van te maken", e);
                }
                throw new StorageException(e.Message, e);
            }
        }

        private void SaveProjectInDatabase(string databaseFilePath)
        {
            string connectionString = GetConnectionToNewFile(databaseFilePath);
            using (var dbContext = new Entities(connectionString))
            {
                try
                {
                    dbContext.VersionEntities.Add(new VersionEntity
                    {
                        Version = StoryTreeVersionHelper.GetCurrentDatabaseVersion(),
                        TimeStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture.DateTimeFormat),
                        FingerPrint = FingerprintHelper.Get(stagedProject.Entity)
                    });
                    dbContext.ProjectEntities.Add(stagedProject.Entity);
                    dbContext.SaveChanges();
                }
                catch (DataException exception)
                {
                    throw CreateStorageWriterException(databaseFilePath, "Er is een fout opgetreden bij het opslaan", exception);
                }
                catch (SystemException exception)
                {
                    if (exception is InvalidOperationException || exception is NotSupportedException)
                    {
                        throw CreateStorageWriterException(databaseFilePath, "Het was niet mogelijk een connectie te maken", exception);
                    }
                    throw;
                }
            }
        }

        private static void ValidateDatabaseVersion(Entities storyTreeEntities, string databaseFilePath)
        {
            try
            {
                string databaseVersion = storyTreeEntities.VersionEntities.Select(v => v.Version).Single();
                if (!StoryTreeVersionHelper.IsValidVersion(databaseVersion))
                {
                    string m = string.Format("Database versie ('{0}') is ongeldig",
                        databaseVersion);
                    string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build(m);
                    throw new FormatException(message);
                }

                if (StoryTreeVersionHelper.IsNewerThanCurrent(databaseVersion))
                {
                    string m = string.Format("Database versie ('{0}') is nieuwer dan de huidige versie ('{1}')",
                        databaseVersion, StoryTreeVersionHelper.GetCurrentDatabaseVersion());
                    string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build(m);
                    throw new FormatException(message);
                }
            }
            catch (InvalidOperationException e)
            {
                string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build("Er mag maximaal 1 rij aanwezig zijn in de VersionEntity tabel van het opslagformaat.");
                throw new FormatException(message);
            }
        }

        /// <summary>
        /// Attempts to set the connection to an existing storage file <paramref name="databaseFilePath"/>.
        /// </summary>
        /// <param name="databaseFilePath">Path to database file.</param>
        /// <exception cref="ArgumentException"><paramref name="databaseFilePath"/> is invalid.</exception>
        /// <exception cref="StorageException">Thrown when:<list type="bullet">
        /// <item><paramref name="databaseFilePath"/> does not exist</item>
        /// <item>the database has an invalid schema.</item>
        /// </list>
        /// </exception>
        private static string GetConnectionToExistingFile(string databaseFilePath)
        {
            IOUtils.ValidateFilePath(databaseFilePath);
            return GetConnectionToFile(databaseFilePath);
        }

        /// <summary>
        /// Sets the connection to a newly created (empty) Ringtoets database file.
        /// </summary>
        /// <param name="databaseFilePath">Path to database file.</param>
        /// <exception cref="ArgumentException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="databaseFilePath"/> is invalid</item>
        /// <item><paramref name="databaseFilePath"/> points to an existing file</item>
        /// </list></exception>
        /// <exception cref="StorageException">Thrown when:<list type="bullet">
        /// <item>executing <c>DatabaseStructure</c> script failed</item>
        /// </list>
        /// </exception>
        private static string GetConnectionToNewFile(string databaseFilePath)
        {
            IOUtils.ValidateFilePath(databaseFilePath);
            StorageSqliteCreator.CreateDatabaseStructure(databaseFilePath);
            return GetConnectionToFile(databaseFilePath);
        }

        /// <summary>
        /// Establishes a connection to an existing <paramref name="databaseFilePath"/>.
        /// </summary>
        /// <param name="databaseFilePath">The path of the database file to connect to.</param>
        /// <exception cref="CouldNotConnectException">No file exists at <paramref name="databaseFilePath"/>.</exception>
        private static string GetConnectionToFile(string databaseFilePath)
        {
            if (!File.Exists(databaseFilePath))
            {
                string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build("Bestand bestaat niet");
                throw new Exception(message);
            }

            return GetConnectionToStorage(databaseFilePath);
        }

        /// <summary>
        /// Sets the connection to the Ringtoets database.
        /// </summary>
        /// <param name="databaseFilePath">The path of the file, which is used for creating exceptions.</param>
        /// <exception cref="StorageValidationException">Thrown when the database does not contain the table <c>version</c>.</exception>
        private static string GetConnectionToStorage(string databaseFilePath)
        {
            string connectionString = SqLiteEntityConnectionStringBuilder.BuildSqLiteEntityConnectionString(databaseFilePath);

            using (var dbContext = new Entities(connectionString))
            {
                try
                {
                    dbContext.Database.Initialize(true);
                    dbContext.LoadVersionTableIntoContext();
                }
                catch (Exception exception)
                {
                    string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build("Geen geldig opslagbestand");
                    throw new Exception(message, exception);
                }
            }
            return connectionString;
        }

        /// <summary>
        /// Creates a configured instance of <see cref="StorageException"/> when writing to the storage file failed.
        /// </summary>
        /// <param name="databaseFilePath">The path of the file that was attempted to connect with.</param>
        /// <param name="errorMessage">The critical error message.</param>
        /// <param name="innerException">Exception that caused this exception to be thrown.</param>
        /// <returns>Returns a new <see cref="StorageException"/>.</returns>
        private static StorageException CreateStorageWriterException(string databaseFilePath, string errorMessage, Exception innerException)
        {
            string message = string.Format("Het is niet gelukt om het bestand weg te schrijven op locatie \"{0}\": {1}",databaseFilePath,errorMessage);
            return new StorageException(message, innerException);
        }

        /// <summary>
        /// Creates a configured instance of <see cref="StorageException"/> when reading the storage file failed.
        /// </summary>
        /// <param name="databaseFilePath">The path of the file that was attempted to connect with.</param>
        /// <param name="errorMessage">The critical error message.</param>
        /// <param name="innerException">Exception that caused this exception to be thrown.</param>
        /// <returns>Returns a new <see cref="StorageException"/>.</returns>
        private static StorageException CreateStorageReaderException(string databaseFilePath, string errorMessage, Exception innerException = null)
        {
            string message = new FileReaderErrorMessageBuilder(databaseFilePath).Build(errorMessage);
            return new StorageException(message, innerException);
        }

        private class StagedProject
        {
            public StagedProject(Project projectModel, ProjectEntity projectEntity)
            {
                Model = projectModel;
                Entity = projectEntity;
            }

            public Project Model { get; }
            public ProjectEntity Entity { get; }
        }
    }

    public class StorageException : Exception
    {
        public StorageException(string eMessage, Exception ioException) : base(eMessage,ioException)
        {
        }
    }
}