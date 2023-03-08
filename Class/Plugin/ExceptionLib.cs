using System;

namespace OctogeddonUnpack.Class
{
    public class FileCorruptionException : Exception
    {
        public FileCorruptionException()
        {
        }

        public FileCorruptionException(string message) : base(message)
        {
        }

        public FileCorruptionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
