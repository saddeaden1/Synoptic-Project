﻿using System;

namespace MembershipSystem.Domain.Exceptions
{
    public class InvalidCardTypeException : Exception
    {
        public InvalidCardTypeException()
        {
            
        }

        public InvalidCardTypeException(string message ) : base(message)
        {

        }

        public InvalidCardTypeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
