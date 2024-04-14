﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Web;

namespace DataTransferObject
{

    class DTOContainerException : Exception
    {
        public DTOContainerException() : base()
        { }

        public DTOContainerException(string message) : base(message)
        { }

        public DTOContainerException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    class NotFoundException : DTOContainerException
    {
        public NotFoundException() : base()
        { }

        public NotFoundException(string message) : base(message)
        { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    class UnresolvableRecursionException : DTOContainerException
    {
        public UnresolvableRecursionException() : base()
        { }

        public UnresolvableRecursionException(string message) : base(message)
        { }

        public UnresolvableRecursionException(string message, Exception innerException) : base(message, innerException)
        { }
    }

}