using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(string))]
    public class StringGenerator : IGenerator
    {
        public object Generate()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", String.Empty);
        }
    }
}
