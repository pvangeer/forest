using System;

namespace Forest.Data.Probabilities
{
    public class ProbabilityException : Exception
    {
        public ProbabilityException()
        {

        }

        public ProbabilityException(string message) : base(message)
        {
            
        }

        public ProbabilityException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
