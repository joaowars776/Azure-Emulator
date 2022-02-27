using System;

namespace Azure.Database.Manager.Database.Database_Exceptions
{
    public class TransactionException : Exception
    {
        public TransactionException(string message) : base(message) { }
    }
}