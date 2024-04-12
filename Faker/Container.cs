using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DataTransferObject.Container;

namespace DataTransferObject
{

    public class Container : IContainer
    {

        private Dictionary<Type, IGenerator> _map;

        public Container(Dictionary<Type, IGenerator> d)
        {
            _map = d;
        }

        public bool has(string id)
        {
            return Type.GetType(id) != null;
        }

        public object get(string id)
        {
            if (!this.has(id))
            {
                throw new NotFoundException("Invalid object passed to get method.");
            }
            return this.prepareObject(id);
        }

        private object prepareObject(string id)
        {
            var classReflector = new ReflectionClass(id);
            var constructorReflector = classReflector.getConstructor();

            if (constructorReflector == null)
            {
                return ReflectionConstructor.ExtConstructor(classReflector.ClassType());
            }

            var constructorParams = constructorReflector.getParameters();

            if (constructorParams == null)
            {
                return ReflectionConstructor.ExtConstructor(classReflector.ClassType());
            }

            List<object> args = new();
            foreach (ReflectionParameter parameter in constructorParams)
            {
                
                IGenerator generator;
                var name = parameter.ParameterType().Namespace + "." + parameter.ParameterType().Name;


                if (this._map.TryGetValue(parameter.ParameterType(), out generator))
                {
                    args.Add(generator.Generate());
                } else
                {
                    args.Add(Activator.CreateInstance(parameter.ParameterType())!);
                }

            }

            return constructorReflector.Execute(args.ToArray());
        }

    }
}
