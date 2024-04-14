using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class DtoAttribute : Attribute
    {
        public DtoAttribute()
        { }
    }
}
