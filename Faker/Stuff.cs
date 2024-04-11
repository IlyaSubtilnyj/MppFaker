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

    public class Container
    {

        private ConcurrentDictionary<string, cctor> objects = new();

        public bool has(string id)
        {
            return objects.ContainsKey(id) || class_exists(id);
        }

        private protected bool class_exists(string className)
        {
            return Type.GetType(className) != null;
        }

        public object get(string id)
        {

            return has(id)
              ? this.objects[id]()
              : this.prepareObject(id);
        }

        public bool empty(object? o)
        {
            return o != null;
        }


        public delegate object cctor();

        internal class ReflectionConstructor
        {
            private ConstructorInfo ctor;
            public ReflectionConstructor(ConstructorInfo ctor)
            {
                this.ctor = ctor;
            }
            public ParameterInfo[] getParameters()
            {
                return new ParameterInfo[0];
            }
            public cctor CreateFabricMethod()
            {

                cctor closure = () =>
                {
                    return ctor.Invoke(new object[0]);
                };

                return closure;
            }
        }

        internal class ReflectionClass
        {
            private Type T;
            public ReflectionClass(Type t)
            {
                T = t;
            }

            public ReflectionConstructor? getConstructor()
            {
                return null;
                ConstructorInfo[] ctors = T.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                List<object> ctorParams = new();
                foreach (ConstructorInfo ctor in ctors)
                {
                    ParameterInfo[] parameters = ctor.GetParameters();

                    foreach (ParameterInfo parameter in parameters)
                    {
                        ctorParams.Add(Activator.CreateInstance(parameter.ParameterType));
                    }
                    break;
                }

                return new ReflectionConstructor(ctors[0]);
            }
        }

        private object prepareObject(string id)
        {
            Type @class = Type.GetType(id);
            var classReflector = new ReflectionClass(@class);

            var constructReflector = classReflector.getConstructor();
            if (empty(constructReflector))
            {
                return Activator.CreateInstance(@class);
            }

            var constructArguments = constructReflector.getParameters();
            if (empty(constructArguments))
            {
                return Activator.CreateInstance(@class);
            }

            //*to-do*//
            return new object();
        }

    }

}