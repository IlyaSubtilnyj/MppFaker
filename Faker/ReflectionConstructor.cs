using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
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

    }
}
