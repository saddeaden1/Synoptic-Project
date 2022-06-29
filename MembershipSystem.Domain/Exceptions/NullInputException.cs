using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class NullInputException : Exception
    {
        public NullInputException()
        {
            
        }

        public NullInputException(string message ) : base(message)
        {

        }

        public NullInputException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
