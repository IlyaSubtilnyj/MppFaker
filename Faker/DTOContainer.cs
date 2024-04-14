using System.Collections.Concurrent;
using System.Reflection;

namespace DataTransferObject
{

    internal class DTOContainer : IDTOContainer
    {

        private ConcurrentDictionary<int, Dictionary<Type, object>> recursivelyEncounteredObjects = new();
        private Dictionary<Type, object> Encountered => recursivelyEncounteredObjects.GetOrAdd(Thread.CurrentThread.ManagedThreadId, _ => new());

        public bool Has(Type id)
        {
            return IsDto(id);
        }

        public object Get(Type id)
        {
            if (!this.Has(id))
            {
                throw new NotFoundException("Invalid object passed to get method.");
            }

            return this.prepareObject(id);
        }

        private bool IsDto(Type id)
        {

            Attribute? attribute = id.GetCustomAttribute(typeof(DtoAttribute));
            return attribute is not null;
        }

        public bool resolveDependency(Type id, ref object dependant, Type dep_id, out object dependency)
        {
            Encountered.Add(id, dependant);

            bool is_set = Encountered.ContainsKey(dep_id);
            dependency = is_set ? Encountered[dep_id] : this.prepareObject(dep_id);

            Encountered.Remove(id, out var temp);
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
