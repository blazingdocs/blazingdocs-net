using System;
using System.Net;

namespace BlazingDocs.Exceptions
{
    public class BlazingException : Exception
    {
        public int StatusCode { get; }

        public BlazingException(int httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        public BlazingException(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int)httpStatusCode;
        }

        public BlazingException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = (int)httpStatusCode;
        }

        public BlazingException(int httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        public BlazingException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = (int)httpStatusCode;
        }

        public BlazingException(int httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }

        public BlazingException()
            : base() { }
    }
}