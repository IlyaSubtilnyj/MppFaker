﻿using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    class LongFormulator : IFormulator<long>
    {
        private readonly Random _random = new();

        public long Generate()
        {
            return _random.Next((int)DateTime.MinValue.Ticks, Int32.MaxValue);
        }
    }
}
