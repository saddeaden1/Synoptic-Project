using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException()
        {
            
        }

        public InvalidPhoneNumberException(string message ) : base(message)
        {

        }

        public InvalidPhoneNumberException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
