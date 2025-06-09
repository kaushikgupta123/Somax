using System;

namespace Common.Structures
{
    public class UpdateIndexMismatchException : Exception
    {
        public UpdateIndexMismatchException(string message) : base(message) { } 
    }

    public class DatabaseNotAccessibleException : Exception
    {
        public DatabaseNotAccessibleException(string message) : base(message) { }
        public DatabaseNotAccessibleException(string message, Exception innerException) : base(message, innerException) { }
    }
}