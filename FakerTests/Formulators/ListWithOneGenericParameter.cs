using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakerTests.Formulators
{
    internal class ListWithOneGenericParameter<T> : IFormulator<List<T>>
    {
        private Faker _faker = new Faker();

        public List<T> Generate()
        {

            List<T> list = new List<T>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(_faker.Create<T>());
            }
            return list;
        }
    }
}
