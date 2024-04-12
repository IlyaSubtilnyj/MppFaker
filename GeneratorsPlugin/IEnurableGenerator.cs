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
    [Generator(typeof(IEnumerable<>))]
    public class IEnurableGenerator<T> : IGenerator
    {

        private static readonly int SIZE = 5;

        public IEnumerable<U> GenenerateEnumerable<U>()
        {
            while (true) 
                yield return Faker.Create<U>();
        }

        public object Generate()
        {
            return GenenerateEnumerable<T>();
        }

    }
}
