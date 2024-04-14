using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorsPlugin
{

    public class ListFormulator<T> : IFormulator<List<T>>
    {
        public List<T> Generate()
        {
            List<T> l = new List<T>();
            for (int i = 0; i < 5; i++)
            {
                l.Add(DataTransferObject.Faker.Create<T>());
            }
            return l;
        }
    }
    public class LisFormulator : IFormulator<List<int>>
    {
        public List<int> Generate()
        {
            List<int> l = new List<int>();
            for (int i = 1; i < 5 + 1; i++)
            {
                l.Add(DataTransferObject.Faker.Create<int>());
            }
            return l;
        }
    }

    public class DicFormulatorr<U, T> : IFormulator<Dictionary<T, U>>
    {
        public Dictionary<T, U> Generate()
        {
            return new();
        }
    }



}
