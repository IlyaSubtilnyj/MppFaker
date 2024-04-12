using System.Collections.Concurrent;
using System.Reflection;
using System.Web;

namespace DataTransferObject
{

    public interface IDto
    {
    }

    class ContainerException : Exception
    {
        public ContainerException() : base()
        {
            // Дополнительная логика инициализации
        }

        public ContainerException(string message) : base(message)
        {
            // Дополнительная логика инициализации
        }

        public ContainerException(string message, Exception innerException) : base(message, innerException)
        {
            // Дополнительная логика инициализации
        }
    }

    class NotFoundException : ContainerException
    {
        public NotFoundException() : base()
        {
            // Дополнительная логика инициализации
        }

        public NotFoundException(string message) : base(message)
        {
            // Дополнительная логика инициализации
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            // Дополнительная логика инициализации
        }
    }

}