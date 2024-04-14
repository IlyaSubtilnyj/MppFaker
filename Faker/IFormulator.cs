using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public interface IFormulator<out T>
    {
        T Generate();
    }
}
