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
        private Faker _faker = new Faker();

        public IEnumerable<T> Generate()
        {

            while (true)
                yield return _faker.Create<T>();
        }

    }
}
