using System;
using System.Data.SQLite;
using System.IO;
using NUnit.Framework;
using StoryTree.Storage.Preparation.Test.Properties;

namespace StoryTree.Storage.Preparation.Test
{
    [TestFixture]
    public class SqLiteExampleDatabaseGeneratorTest
    {
        [Test]
        [Explicit("Creates a new Ringtoets.rtd file in the root of the Application.Ringtoets.Storage")]
        public void CreateSampleDatabase()
        {
            // Setup
            var storageFile = GetPathToStorageFile();
            if (File.Exists(storageFile))
            {
                TestDelegate precondition = () => File.Delete(storageFile);
                Assert.DoesNotThrow(precondition, "Precondition failed: file could not be deleted: '{0}'", storageFile);
            }

            // Call
            CreateDatabaseFile(storageFile, Resources.TellTheStoryDatabaseSchema);

            // Assert
            Assert.IsTrue(File.Exists(storageFile));
        }

        private static string GetPathToStorageFile()
        {
            return @"C:\src\storytree\design\TellTheStoryDatabaseSchemaSample.sqlite";
        }

        public static void CreateDatabaseFile(string databaseFilePath, string databaseSchemaQuery)
        {
            if (string.IsNullOrWhiteSpace(databaseSchemaQuery))
                throw new ArgumentNullException(nameof(databaseSchemaQuery));
            if (string.IsNullOrWhiteSpace(databaseFilePath))
                throw new ArgumentNullException(nameof(databaseFilePath));

            SQLiteConnection.CreateFile(databaseFilePath);
            PerformCommandOnDatabase(databaseFilePath, databaseSchemaQuery);
        }

        /// <summary>
        ///     Performs the command on a database.
        /// </summary>
        /// <param name="databaseFilePath">The file path to the database.</param>
        /// <param name="commandText">The command text/query.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="databaseFilePath" /> is <c>null</c> or whitespace.</exception>
        private static void PerformCommandOnDatabase(string databaseFilePath, string commandText)
        {
            var connectionString = BuildSqLiteConnectionString(databaseFilePath, false);
            using (var dbContext = new SQLiteConnection(connectionString, true))
            {
                dbContext.Open();
                using (var command = dbContext.CreateCommand())
                {
                    try
                    {
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        SQLiteConnection.ClearAllPools();
                        dbContext.Close();
                    }
                }
            }
        }

        public static string BuildSqLiteConnectionString(string filePath, bool readOnly)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                const string message = @"Cannot create a connection string without the path to the file to connect to.";
                throw new ArgumentNullException(nameof(filePath), message);
            }

            return new SQLiteConnectionStringBuilder
            {
                FailIfMissing = true,
                DataSource = filePath,
                ReadOnly = readOnly,
                ForeignKeys = true,
                Version = 3,
                Pooling = false
            }.ConnectionString;
        }
    }
}