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

    internal class ReflectionParameter
    {
        private ParameterInfo _param;

        public ReflectionParameter(ParameterInfo parameter)
        {

            this._param = parameter;
        }

        public Type getType()
        {

            return _param.ParameterType;
        }
    }

    public delegate object ConstructorDecorator();

    internal class ReflectionConstructor
    {

        private ConstructorInfo _ctor;
        private List<ReflectionParameter> _ctorParams;

        public ReflectionConstructor(ConstructorInfo ctor)
        {
            /*  Outside of closure for garbage collection reasons  */
            this._ctor = ctor;
            this._ctorParams = new();

            ParameterInfo[] parameters = _ctor.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                _ctorParams.Add(new(parameter));
            }
        }

        public ReflectionParameter[] getParameters()
        {
          
            return _ctorParams.ToArray();
        }
        public ConstructorDecorator Snapshot()
        {
            var constructor = this._ctor;
            var parameters  = this.getParameters();

            ConstructorDecorator closure = () =>
            {
                List<object> args = new();

                foreach (ReflectionParameter parameter in parameters)
                {
                    //var intg = new Int32Generator();
                    //if (typeof(IGenerator).IsAssignableFrom(intg.GetType())) {

                    //    args.Add(intg.Generate());
                    //}
                }

                return constructor.Invoke(args.ToArray());
            };

            return closure;
        }
    }

    internal class ReflectionClass
    {
        private Type _classType;
        private ReflectionConstructor _ctor;

        public ReflectionClass(string className)
        {

            this._classType = Type.GetType(className)!;
            ConstructorInfo[] ctors = this._classType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            this._ctor = new ReflectionConstructor(ctors[0]);
        }

        public ReflectionConstructor getConstructor()
        {
            
            return this._ctor;
        }
    }

    public class Container : IContainer
    {

        private ConcurrentDictionary<string, ConstructorDecorator> services = new();

        public bool has(string id)
        {
            return isset(id) || class_exists(id);
        }

        public bool isset(string id)
        {
            return services.ContainsKey(id);
        }

        private protected bool class_exists(string className)
        {
            return Type.GetType(className) != null;
        }

        public object get(string id)
        {
            if (!this.has(id)) {
            
                throw new NotFoundException("Invalid object passed to get method.");
            }
            
            return isset(id) ? this.services[id]()
                             : this.prepareObject(id);
        }

        private object prepareObject(string id)
        {
            var classReflector = new ReflectionClass(id);
            var constructorReflector = classReflector.getConstructor();

            var constructorDecorator = constructorReflector.Snapshot();
            services.TryAdd(id, constructorDecorator);

            return constructorDecorator();
        }

    }
}
