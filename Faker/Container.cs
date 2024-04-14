using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DataTransferObject.Container;

namespace DataTransferObject
{

    public class Container : IContainer
    {

        public bool has(Type id)
        {
            return isDto(id);
        }

        public bool isDto(Type id)
        {

            Attribute? attribute = id.GetCustomAttribute(typeof(DtoAttribute));
            return attribute is not null;
        }

        public object get(Type id)
        {
            if (!this.has(id))
            {
                throw new NotFoundException("Invalid object passed to get method.");
            }
            return this.prepareObject(id);
        }

        private object prepareObject(Type id)
        {
            object result = new();

            var classReflector = new ReflectionClass(id);
            var constructorReflector = classReflector.getConstructor();
            var constructorParams = constructorReflector?.getParameters();

            if (constructorReflector == null || constructorParams == null)
            {

                result =  ReflectionConstructor.ExtConstructor(classReflector.ClassType());
            } else
            {

                List<object> args = new();
                foreach (ReflectionParameter parameter in constructorParams)
                {
                    if (has(parameter.ParameterType()))
                    {
                       
                        object dependency = prepareObject(parameter.ParameterType());
                        args.Add(dependency);
                    }
                    else
                    {
                        var generatedParamenter = Composer.Formulate(parameter.ParameterType());
                        args.Add(generatedParamenter);
                    }
                    //если дто, то сгенерить dto рекурсивно
                    //буффер, чтобы предотвратить рекурсивность (isset)

                }

                result = constructorReflector.Execute(args.ToArray());
            }

            var classPublicFields = classReflector.getPublicFields();

            if (classPublicFields != null)
            {

                foreach (ReflectionField field in classPublicFields)
                {

                    //check on recursion
                    var generatedField = Composer.Formulate(field.FieldType());
                    field.SetValue(result, generatedField);
                }
            }

            var classPublicProperties = classReflector.getPublicProperties();

            if (classPublicProperties != null)
            {

                foreach (ReflectionProperty property in classPublicProperties)
                {

                    object? generatedProperty = null;

                    if (has(property.PropertyType()))
                    {
                        generatedProperty = prepareObject(property.PropertyType()); 
                    }
                    else
                    {
                        generatedProperty = Composer.Formulate(property.PropertyType());
                    }    
                    
                    property.SetValue(result, generatedProperty);
                }
            }

            return result;
        }

    }
}
