using System.Collections.Concurrent;
using System.Reflection;
using System.Web;

namespace DataTransferObject
{

    public interface IDto
    {
    }

    class ContainerExceptionInterface : Exception
    {
    }

    class NotFoundExceptionInterface : ContainerExceptionInterface
    {

    }

}