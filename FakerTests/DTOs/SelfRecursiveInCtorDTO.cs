using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerTests.DTOs
{
    [Dto]
    internal class SelfRecursiveInCtorDTO
    {
        private int dummy;
        public SelfRecursiveInCtorDTO(SelfRecursiveInCtorDTO recursion) { }
    }
}
