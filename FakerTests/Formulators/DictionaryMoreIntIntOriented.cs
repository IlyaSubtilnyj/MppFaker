using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerTests.Formulators
{
    internal class DictionaryMoreIntIntOriented<T> : IFormulator<Dictionary<int, T>>
    {
        public Dictionary<int, T> Generate()
        {
            return new Dictionary<int, T> { { default(int) + 1, default(T) } };
        }
    }
}
