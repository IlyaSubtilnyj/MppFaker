using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    internal class ReflectionClass
    {
        private Type                        _classType;
        private ReflectionConstructor?      _ctor;
        private List<ReflectionField>?      _classPublicFields;
        private List<ReflectionProperty>?   _classPublicProperties;

        public ReflectionClass(Type classType)
        {

            this._classType = classType;
            this._ctor = null;
            this._classPublicFields = null;
            this._classPublicProperties = null;

            ConstructorInfo[] ctors = this._classType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (ctors.Length != 0)
            {

                this._ctor = new ReflectionConstructor(ctors[0]);
            }

            FieldInfo[] fields = this._classType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (fields.Length > 0)
            {
                _classPublicFields = new List<ReflectionField>(fields.Length);
                foreach (var field in fields)
                {
                    _classPublicFields.Add(new(field));
                }
            }

            PropertyInfo[] properties = this._classType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length > 0)
            {
                _classPublicProperties = new List<ReflectionProperty>(properties.Length);
                foreach (var property in properties)
                {
                    _classPublicProperties.Add(new(property));
                }
            }
        }

        public ReflectionConstructor? getConstructor()
        {

            return this._ctor;
        }

        public List<ReflectionField>? getPublicFields()
        {

            return this._classPublicFields;
        }
        public List<ReflectionProperty>? getPublicProperties()
        {

            return this._classPublicProperties;
        }

        public Type ClassType()
        {
            return this._classType;
        }
    }
}
