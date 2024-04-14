using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    public class FloatFormulator : IFormulator<float>
    {
        public const double LowerBound = 0.0;

        public const double UpperBound = 10.0;

        private readonly Random _random = new();
        public float Generate()
        {
            return (float)(_random.NextSingle() * (UpperBound - LowerBound) + LowerBound);
        }
    }
}
