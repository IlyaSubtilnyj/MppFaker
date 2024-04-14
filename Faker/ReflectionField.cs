using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    internal class ReflectionField
    {
        private FieldInfo _field;

        public ReflectionField(FieldInfo field)
        {

            this._field = field;
        }

        public Type FieldType()
        {

            return _field.FieldType;
        }

        public void SetValue(object @object, object value)
        {

            this._field.SetValue(@object, value);
        }
    }
}
