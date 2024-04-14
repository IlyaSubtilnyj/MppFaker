using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using DataTransferObject;

namespace GeneratorsPlugin
{
    public class IEnurableFormulator<T> : IFormulator<IEnumerable<T>>
    {

        public IEnumerable<T> Generate()
        {
            while (true)
                yield return Faker.Create<T>();
        }

    }
}
