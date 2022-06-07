using System;

namespace Scripts.Toolbox
{
    public class InvalidAmountChildrenException : Exception
    {
        public InvalidAmountChildrenException() {}
        public InvalidAmountChildrenException(string message) : base(message) {}
        public InvalidAmountChildrenException(string message, Exception inner) : base(message, inner) {}
    }
}