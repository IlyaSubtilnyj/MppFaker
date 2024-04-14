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

        private ConcurrentDictionary<Type, object> objects = new();

        public bool Has(Type id)
        {
            return isDto(id);
        }

        public bool isDto(Type id)
        {

            Attribute? attribute = id.GetCustomAttribute(typeof(DtoAttribute));
            return attribute is not null;
        }

        public object Get(Type id)
        {
            if (!this.Has(id))
            {
                throw new NotFoundException("Invalid object passed to get method.");
            }

            return this.prepareObject(id);
        }

        public bool isset(Type id)
        {
            return objects.ContainsKey(id);
        }

        public bool resolveDependency(Type id, ref object dependant, Type did, out object dependency)
        {
            this.objects[id] = dependant;

            bool is_set = isset(did);
            dependency = is_set ? this.objects[did] : this.prepareObject(did);

            return is_set;
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
                    if (Has(parameter.ParameterType()))
                    {
                       
                        object dependency;
                        bool isRecursive = resolveDependency(id, ref result, parameter.ParameterType(), out dependency);

                        if (isRecursive)
                        {
                            throw new UnresolvableRecursionException($"Recursive DTO dependencies not allowed in DTO constructor: [{id} -> {parameter.ParameterType()}]");
                        }

                        args.Add(dependency);
                    }
                    else
                    {
                        var generatedParamenter = Composer.Compose(parameter.ParameterType());
                        args.Add(generatedParamenter);
                    }

                }

                result = constructorReflector.Execute(args.ToArray());
            }

            var classPublicFields = classReflector.getPublicFields();

            if (classPublicFields != null)
            {

                foreach (ReflectionField field in classPublicFields)
                {

                    object? generatedField = null;

                    if (Has(field.FieldType()))
                    {

                        bool isRecursive = resolveDependency(id, ref result, field.FieldType(), out generatedField);

                        if (isRecursive)
                        {
                            throw new UnresolvableRecursionException($"Recursive DTO dependencies not allowed in DTO public fields: [{id} -> {field.FieldType()}]");
                        }
                    }
                    else
                    {
                        generatedField = Composer.Compose(field.FieldType());
                    }

                    field.SetValue(result, generatedField);
                }
            }

            var classPublicProperties = classReflector.getPublicProperties();

            if (classPublicProperties != null)
            {

                foreach (ReflectionProperty property in classPublicProperties)
                {

                    object? generatedProperty = null;

                    if (Has(property.PropertyType()))
                    {

                        bool isRecursive = resolveDependency(id, ref result, property.PropertyType(), out generatedProperty);

                        if (isRecursive)
                        {
                            throw new UnresolvableRecursionException($"Recursive DTO dependencies not allowed in DTO public properties: [{id} -> {property.PropertyType()}]");
                        }
                    }
                    else
                    {
                        generatedProperty = Composer.Compose(property.PropertyType());
                    }    
                    
                    property.SetValue(result, generatedProperty);
                }
            }

            return result;
        }

    }
}
