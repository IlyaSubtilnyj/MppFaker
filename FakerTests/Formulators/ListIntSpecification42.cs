using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerTests.Formulators
{
    internal class ListIntSpecification42 : IFormulator<List<int>>
    {
        public List<int> Generate()
        {
            
            return new List<int>() { 42 };
        }
    }
}
