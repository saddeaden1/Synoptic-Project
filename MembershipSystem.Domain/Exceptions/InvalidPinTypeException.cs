using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class InvalidPinTypeException : Exception
    {
        public InvalidPinTypeException()
        {
            
        }

        public InvalidPinTypeException(string message ) : base(message)
        {

        }

        public InvalidPinTypeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
