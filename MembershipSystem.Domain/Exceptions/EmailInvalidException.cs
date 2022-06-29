using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class EmailInvalidException : Exception
    {
        public EmailInvalidException()
        {
            
        }

        public EmailInvalidException(string message ) : base(message)
        {

        }

        public EmailInvalidException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
