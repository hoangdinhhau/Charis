using System;

namespace Charis.Utilities.Exceptions
{
    public class CharisException : Exception
    {
        public CharisException()
        {
        }

        public CharisException(string message)
            : base(message)
        {
        }

        public CharisException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}