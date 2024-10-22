using System;

namespace Forest.Storage.Read
{
    internal class ReadReferencedObjectsFirstException : Exception
    {
        public ReadReferencedObjectsFirstException(string positionedStakeholderName)
        {
        }
    }
}