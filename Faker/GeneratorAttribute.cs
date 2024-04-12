using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class GeneratorAttribute : Attribute
    {
        public Type GenereatorType
        { get; set; }

        public GeneratorAttribute(Type genereatorType)
        {
            GenereatorType = genereatorType;
        }
    }
}
