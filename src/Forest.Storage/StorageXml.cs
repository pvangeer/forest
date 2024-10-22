using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Forest.Data;
using Forest.Storage.Create;
using Forest.Storage.Read;
using Forest.Storage.XmlEntities;

namespace Forest.Storage
{
    public class StorageXml
    {
        private readonly int emptyEventTreeProjectHash;
        private int lastOpenedOrSavedEventTreeProjectHash;
        private EventTreeProjectXmlEntity stagedEventTreeProjectXmlEntity;
        private VersionInfo versionInfo;

        public StorageXml()
        {
            emptyEventTreeProjectHash =
                FingerprintHelper.Get(EventTreeProjectFactory.CreateStandardNewProject().Create(new PersistenceRegistry()));
        }

        public bool HasStagedEventTreeProject => stagedEventTreeProjectXmlEntity != null;

        public void StageVersionInformation(VersionInfo newVersionInfo)
        {
            versionInfo = newVersionInfo;
        }

        public void StageEventTreeProject(EventTreeProject eventTreeProject)
        {
            if (eventTreeProject == null)
                throw new ArgumentNullException(nameof(eventTreeProject));

            stagedEventTreeProjectXmlEntity = eventTreeProject.Create(new PersistenceRegistry());
        }

        public void UnStageEventTreeProject()
        {
            stagedEventTreeProjectXmlEntity = null;
        }

        public void UnStageVersionInformation()
        {
            versionInfo = null;
        }

        public void SaveProjectAs(string databaseFilePath)
        {
            if (!HasStagedEventTreeProject)
                throw new InvalidOperationException("Call 'StageAnalysis(Analysis)' first before calling this method.");

            try
            {
                var writer = new BackedUpFileWriter(databaseFilePath);
                writer.Perform(() => SaveProjectInDatabase(databaseFilePath));
            }
            catch (IOException e)
            {
                throw new XmlStorageException(e.Message, e);
            }
            finally
            {
                UnStageEventTreeProject();
                UnStageVersionInformation();
            }
        }

        public Project LoadProject(string filePath)
        {
            IOUtils.ValidateFilePath(filePath);

            try
            {
                ProjectXmlEntity projectXmlEntity;

                var serializer = new XmlSerializer(typeof(ProjectXmlEntity));

                using (var reader = XmlReader.Create(filePath))
                {
                    try
                    {
                        projectXmlEntity = (ProjectXmlEntity)serializer.Deserialize(reader);
                    }
                    catch (InvalidOperationException exception)
                    {
                        throw CreateStorageReaderException(filePath, "Bestand kon niet worden gelezen", exception.InnerException);
                    }
                }

                lastOpenedOrSavedEventTreeProjectHash = FingerprintHelper.Get(projectXmlEntity.EventTreeProject);
                return new Project
                {
                    EventTreeProject = projectXmlEntity.EventTreeProject.Read(new ReadConversionCollector()),
                    Created = projectXmlEntity.VersionInformation.Created,
                    Author = projectXmlEntity.VersionInformation.Creator
                };
            }
            catch (Exception exception)
            {
                throw CreateStorageReaderException(filePath, "Het project kon niet worden ingeladen", exception);
            }
        }

        public bool HasStagedProjectChanges()
        {
            if (!HasStagedEventTreeProject)
                throw new InvalidOperationException("Call 'StageAnalysis(IProject)' first before calling this method.");

            var hash = FingerprintHelper.Get(stagedEventTreeProjectXmlEntity);
            return hash != emptyEventTreeProjectHash &&
                   lastOpenedOrSavedEventTreeProjectHash != hash;
        }

        private void SaveProjectInDatabase(string filePath)
        {
            IOUtils.ValidateFilePath(filePath);

            try
            {
                var projectXmlEntity = new ProjectXmlEntity { EventTreeProject = stagedEventTreeProjectXmlEntity };
                projectXmlEntity.VersionInformation.Created = versionInfo != null
                    ? versionInfo.DateCreated
                    : projectXmlEntity.VersionInformation.LastChanged;
                projectXmlEntity.VersionInformation.Creator = versionInfo != null
                    ? versionInfo.AuthorCreated
                    : projectXmlEntity.VersionInformation.LastAuthor;

                var serializer = new XmlSerializer(typeof(ProjectXmlEntity));

                using (var writer = XmlWriter.Create(filePath))
                {
                    serializer.Serialize(writer, projectXmlEntity);
                }

                lastOpenedOrSavedEventTreeProjectHash = FingerprintHelper.Get(stagedEventTreeProjectXmlEntity);
            }
            catch (DataException exception)
            {
                throw CreateStorageWriterException(filePath, "Er is een fout opgetreden bij het opslaan",
                    exception);
            }
            catch (SystemException exception)
            {
                if (exception is InvalidOperationException || exception is NotSupportedException)
                    throw CreateStorageWriterException(filePath, "Het was niet mogelijk een connectie te maken",
                        exception);

                throw;
            }
        }

        /// <summary>
        ///     Creates a configured instance of <see cref="XmlStorageException" /> when writing to the storage file failed.
        /// </summary>
        /// <param name="databaseFilePath">The path of the file that was attempted to connect with.</param>
        /// <param name="errorMessage">The critical error message.</param>
        /// <param name="innerException">Exception that caused this exception to be thrown.</param>
        /// <returns>Returns a new <see cref="XmlStorageException" />.</returns>
        private static XmlStorageException CreateStorageWriterException(string databaseFilePath, string errorMessage,
            Exception innerException)
        {
            var message = string.Format("Het is niet gelukt om het bestand weg te schrijven op locatie \"{0}\": {1}",
                databaseFilePath, errorMessage);
            return new XmlStorageException(message, innerException);
        }

        /// <summary>
        ///     Creates a configured instance of <see cref="XmlStorageException" /> when reading the storage file failed.
        /// </summary>
        /// <param name="databaseFilePath">The path of the file that was attempted to connect with.</param>
        /// <param name="errorMessage">The critical error message.</param>
        /// <param name="innerException">Exception that caused this exception to be thrown.</param>
        /// <returns>Returns a new <see cref="XmlStorageException" />.</returns>
        private static XmlStorageException CreateStorageReaderException(string databaseFilePath, string errorMessage,
            Exception innerException = null)
        {
            var message = new FileReaderErrorMessageBuilder(databaseFilePath).Build(errorMessage);
            return new XmlStorageException(message, innerException);
        }
    }

    public class Project
    {
        public EventTreeProject EventTreeProject { get; set; }

        public string Created { get; set; }

        public string Author { get; set; }
    }

    public class XmlStorageException : Exception
    {
        public XmlStorageException(string eMessage, Exception ioException) : base(eMessage, ioException)
        {
        }
    }
}