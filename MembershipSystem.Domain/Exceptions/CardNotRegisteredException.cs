using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class CardNotRegisteredException : Exception
    {
        public CardNotRegisteredException()
        {
            
        }

        public CardNotRegisteredException(string message ) : base(message)
        {

        }

        public CardNotRegisteredException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
