﻿using System.Xml;

namespace Forest.Storage.Migration
{
    public abstract class FileMigrationScript
    {
        public abstract string BaseVersion { get; }

        public abstract string TargetVersion { get; }

        public abstract void Execute(XmlDocument xmlDocument);
    }
}