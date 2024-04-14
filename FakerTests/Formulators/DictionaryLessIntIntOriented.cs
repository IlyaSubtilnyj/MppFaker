using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerTests.Formulators
{
    internal class DictionaryLessIntIntOriented<T> : IFormulator<Dictionary<T, int>> where T : notnull
    {
        public Dictionary<T, int> Generate()
        {
            return new Dictionary<T, int> { { default(T), default(int) + 1 } };
        }
    }
}
