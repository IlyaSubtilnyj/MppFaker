using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DataTransferObject.Container;

namespace DataTransferObject
{
    public class Container : IContainer
    {

        private ConcurrentDictionary<string, cctor> objects = new();

        public bool has(string id)
        {
            return isset(id) || class_exists(id);
        }

        public bool isset(string id)
        {
            return objects.ContainsKey(id);
        }

        private protected bool class_exists(string className)
        {
            return Type.GetType(className) != null;
        }

        public object get(string id)
        {
            if (!this.has(id)) {
            
                throw new NotFoundExceptionInterface("ti popusk");
            }
            
            return isset(id)
              ? this.objects[id]()
              : this.prepareObject(id);
        }

        public bool empty(object? o)
        {
            return o == null;
        }


        public delegate object cctor(object?[]? args);

        internal class ReflectionConstructor
        {
            private ConstructorInfo ctor;
            List<Type> ctorParams;
            public ReflectionConstructor(ConstructorInfo ctor)
            {
                this.ctor = ctor;
            }
            public List<Type> getParameters()
            {
                ctorParams = new();
                ParameterInfo[] parameters = ctor.GetParameters();
                foreach (ParameterInfo parameter in parameters)
                {
                    ctorParams.Add(parameter.ParameterType);
                }
                return ctorParams;
            }
            public cctor Snapshot()
            {
                ConstructorInfo ctor = this.ctor;
                cctor closure = (object?[]? args) =>
                {
                    return ctor.Invoke(args);
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
                ConstructorInfo[] ctors = T.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                return new ReflectionConstructor(ctors[0]);
            }
        }

        private object prepareObject(string id)
        {
            Type @class = Type.GetType(id)!;

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

            List<object> args = new();
            foreach (var argument in constructArguments) {

                object arg = this.get(argument.Name);
                args.Add(arg);
            }

            var ctorDecorator = constructReflector.Snapshot();
            objects.TryAdd(id, ctorDecorator);

            return ctorDecorator(args);
        }

    }
}
