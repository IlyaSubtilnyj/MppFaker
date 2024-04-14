using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    internal class ReflectionProperty
    {
        private PropertyInfo _property;

        public ReflectionProperty(PropertyInfo property)
        {

            this._property = property;
        }

        public Type PropertyType()
        {

            return _property.PropertyType;
        }

        public void SetValue(object @object, object value)
        {

            this._property.SetValue(@object, value);
        }
    }
}
