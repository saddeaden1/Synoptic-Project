using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class CardAlreadyRegisteredException : Exception
    {
        public CardAlreadyRegisteredException()
        {
            
        }

        public CardAlreadyRegisteredException(string message ) : base(message)
        {

        }

        public CardAlreadyRegisteredException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
