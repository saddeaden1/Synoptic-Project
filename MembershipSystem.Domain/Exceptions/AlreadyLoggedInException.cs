using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class AlreadyLoggedInException : Exception
    {
        public AlreadyLoggedInException()
        {
            
        }

        public AlreadyLoggedInException(string message ) : base(message)
        {

        }

        public AlreadyLoggedInException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
