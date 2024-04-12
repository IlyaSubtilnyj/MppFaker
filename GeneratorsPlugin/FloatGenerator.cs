using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(float))]
    public class FloatGenerator : IGenerator
    {
        public const double LowerBound = 0.0;

        public const double UpperBound = 10.0;

        private readonly Random _random = new();
        public object Generate()
        {
            return _random.NextSingle() * (UpperBound - LowerBound) + LowerBound;
        }
    }
}
