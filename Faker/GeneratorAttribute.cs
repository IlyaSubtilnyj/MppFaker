using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
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
