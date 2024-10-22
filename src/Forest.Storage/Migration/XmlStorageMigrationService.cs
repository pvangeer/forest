using System;
using System.Collections.Generic;
using System.Xml;
using Forest.Storage.XmlEntities;

namespace Forest.Storage.Migration
{
    public static class XmlStorageMigrationService
    {
        public static void MigrateFile(string oldFileName, string newFileName)
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(oldFileName);
            }
            catch (Exception e)
            {
                throw new XmlMigrationException("De inhoud van dit bestand kon niet worden geladen", e);
            }

            var versionNode = GetVersionNode(xmlDoc);
            if (versionNode == null)
                throw new XmlMigrationException("Er kon geen versie-informatie worden gevonden.");

            var migrators = GatherMigrationScripts(versionNode);

            try
            {
                foreach (var fileMigrator in migrators)
                    fileMigrator.Execute(xmlDoc);
            }
            catch (Exception e)
            {
                throw new XmlMigrationException(
                    $"Er is een onverwachte fout opgetreden bij het migreren van dit bestand: '{e.Message}'. \n De migratie van het bestand is niet gelukt.");
            }

            UpdateVersionInformation(versionNode, xmlDoc);

            try
            {
                xmlDoc.Save(newFileName);
            }
            catch (Exception e)
            {
                throw new XmlMigrationException($"Het gemigreerde bestand kon niet worden opgeslagen: {e.Message}");
            }
        }

        private static void UpdateVersionInformation(XmlNode versionNode, XmlDocument xmlDoc)
        {
            var versionXmlEntity = new VersionXmlEntity();
            versionNode.InnerText = versionXmlEntity.FileVersion;

            var lastChangedNode = GetLastChangedNode(xmlDoc);
            if (lastChangedNode != null)
                lastChangedNode.InnerText = versionXmlEntity.LastChanged;
        }

        private static List<FileMigrationScript> GatherMigrationScripts(XmlNode versionNode)
        {
            var migrators = new List<FileMigrationScript>();
            var version = versionNode.InnerText;
            switch (version)
            {
                case "24.1":
                    //migrators.Add(new Migrator241To242());
                    break;
            }

            return migrators;
        }

        public static bool NeedsMigration(string fileName)
        {
            IOUtils.ValidateFilePath(fileName);

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                var versionInformation = GetVersionInformation(xmlDoc);
                if (versionInformation == null)
                    throw new XmlStorageException(
                        "Het gespecificeerde bestand heeft geen versie-informatie en kan niet worden gelezen.", null);

                return !HasCurrentVersion(versionInformation);
            }
            catch (Exception exception)
            {
                throw new XmlStorageException("Bestand kon niet worden gelezen", exception);
            }
        }

        private static bool HasCurrentVersion(string versionNodeValue)
        {
            return versionNodeValue == VersionXmlEntity.CurrentVersion;
        }

        private static string GetVersionInformation(XmlDocument xmlDoc)
        {
            return GetVersionNode(xmlDoc)?.InnerText;
        }

        private static XmlNode GetVersionNode(XmlDocument xmlDoc)
        {
            return xmlDoc.SelectSingleNode(
                $"/project/{ProjectXmlEntity.VersionInformationElementName}/{VersionXmlEntity.FileVersionElementName}");
        }

        private static XmlNode GetLastChangedNode(XmlDocument xmlDoc)
        {
            return xmlDoc.SelectSingleNode(
                $"/project/{ProjectXmlEntity.VersionInformationElementName}/{VersionXmlEntity.LastChangedElementName}");
        }
    }
}