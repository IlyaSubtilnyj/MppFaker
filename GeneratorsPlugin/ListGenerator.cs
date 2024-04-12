using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{
    [Generator(typeof(List<>))]
    public class ListGenerator<T> : IGenerator
    {
        private static readonly int SIZE = 5;

        public List<U> GenenerateList<U>()
        {
            List<U> list = new List<U>();
            for (int i = 1; i < SIZE + 1; i++)
            {
                list.Add(Faker.Create<U>());
            }
            return list;
        }

        public object Generate()
        {
            return this.GenenerateList<T>();
        }
    }
}
