using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class FailedTransactionException : Exception
    {
        public FailedTransactionException()
        {
            
        }

        public FailedTransactionException(string message ) : base(message)
        {

        }

        public FailedTransactionException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}