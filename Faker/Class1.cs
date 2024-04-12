using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    internal class ReflectionParameter
    {
        private ParameterInfo _param;

        public ReflectionParameter(ParameterInfo parameter)
        {

            this._param = parameter;
        }

        public Type ParameterType()
        {

            return _param.ParameterType;
        }
    }

    public delegate object ConstructorDecorator();

    internal class ReflectionConstructor
    {

        private ConstructorInfo? _ctor;
        private List<ReflectionParameter>? _ctorParams;

        public static object ExtConstructor(Type t)
        {
            return Activator.CreateInstance(t, true)!;
        }

        public ReflectionConstructor(ConstructorInfo? ctor)
        {
            this._ctor = ctor;
            this._ctorParams = null;

            if (ctor != null)
            {
                ParameterInfo[] parameters = _ctor.GetParameters();
                if (parameters.Length > 0)
                {
                    _ctorParams = new List<ReflectionParameter>(parameters.Length);
                    foreach (ParameterInfo parameter in parameters)
                    {
                        _ctorParams.Add(new(parameter));
                    }
                }
            }

        }

        public object Execute(object[] args)
        {
            return this._ctor!.Invoke(args);
        }

        public ReflectionParameter[]? getParameters()
        {

            return _ctorParams?.ToArray();
        }
        public ConstructorDecorator Snapshot()
        {
            /*  Outside of closure for garbage collection reasons  */
            var constructor = this._ctor;
            var parameters = this.getParameters();

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
        private ReflectionConstructor? _ctor;

        public ReflectionClass(string className)
        {

            this._classType = Type.GetType(className)!;
            this._ctor = null;

            ConstructorInfo[] ctors = this._classType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Length != 0)
            {

                this._ctor = new ReflectionConstructor(ctors[0]);
            }
        }

        public ReflectionConstructor? getConstructor()
        {

            return this._ctor;
        }

        public Type ClassType()
        {
            return this._classType;
        }
    }
}
