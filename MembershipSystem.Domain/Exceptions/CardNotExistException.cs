using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class CardNotExistException : Exception
    {
        public CardNotExistException()
        {

        }

        public CardNotExistException(string message) : base(message)
        {

        }

        public CardNotExistException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}