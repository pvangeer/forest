// Copyright (C) Stichting Deltares 2017. All rights reserved.
//
// This file is part of Ringtoets.
//
// Ringtoets is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Data.SQLite;
using System.IO;
using StoryTree.Storage.Properties;

namespace StoryTree.Storage
{
    /// <summary>
    /// This class interacts with an empty or new SQLite database file.
    /// </summary>
    public static class StorageSqliteCreator
    {
        /// <summary>
        /// Creates a new file with the basic database structure for a Ringtoets database at
        /// <paramref name="databaseFilePath"/>.
        /// </summary>
        /// <param name="databaseFilePath">Path of the new database file.</param>
        /// <exception cref="ArgumentException">Thrown when either:
        /// <list type="bullet">
        /// <item><paramref name="databaseFilePath"/> is invalid</item>
        /// <item><paramref name="databaseFilePath"/> points to an existing file</item>
        /// </list></exception>
        /// <exception cref="StorageException">Thrown when executing <c>DatabaseStructure</c> script fails.</exception>
        public static void CreateDatabaseStructure(string databaseFilePath)
        {
            IOUtils.ValidateFilePath(databaseFilePath);

            if (File.Exists(databaseFilePath))
            {
                string message = $"File '{databaseFilePath}' already exists.";
                throw new ArgumentException(message);
            }

            SQLiteConnection.CreateFile(databaseFilePath);
            string connectionString = SqLiteConnectionStringBuilder.BuildSqLiteConnectionString(databaseFilePath, false);
            try
            {
                using (var dbContext = new SQLiteConnection(connectionString, true))
                {
                    dbContext.Open();
                    using (SQLiteCommand command = dbContext.CreateCommand())
                    {
                        command.CommandText = Resources.TellTheStoryDatabaseSchema;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException exception)
            {
                string message = string.Format("Kon bestand \"{0}\" niet wegschrijven",databaseFilePath);
                throw new StorageException(message, exception);
            }
            finally
            {
                SQLiteConnection.ClearAllPools();
            }
        }
    }
}