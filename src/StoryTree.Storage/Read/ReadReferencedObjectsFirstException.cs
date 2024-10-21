using System;

namespace StoryTree.Storage.Read
{
    internal class ReadReferencedObjectsFirstException : Exception
    {
        public ReadReferencedObjectsFirstException(string positionedStakeholderName)
        {
        }
    }
}