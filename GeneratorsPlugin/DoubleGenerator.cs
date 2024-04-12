using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(double))]
    public class DoubleGenerator : IGenerator
    {
        public const double LowerBound = 0.0;

        public const double UpperBound = 10.0;

        private readonly Random _random = new();
        public object Generate()
        {
            return _random.NextDouble() * (UpperBound - LowerBound) + LowerBound;
        }
    }
}
