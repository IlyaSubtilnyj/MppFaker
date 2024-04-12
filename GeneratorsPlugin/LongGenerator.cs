using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(long))]
    class LongGenerator : IGenerator
    {
        private readonly Random _random = new();

        public object Generate()
        {
            return _random.Next((int)DateTime.MinValue.Ticks, Int32.MaxValue);
        }
    }
}
