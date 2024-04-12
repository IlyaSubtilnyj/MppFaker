using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject;

namespace GeneratorsPlugin
{

    [Generator(typeof(int))]
    class Int32Generator : IGenerator
    {
        private readonly Random _random = new();

        public object Generate()
        {
            byte[] buffer = new byte[4];
            _random.NextBytes(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
