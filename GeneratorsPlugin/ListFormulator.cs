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

    public class DiccFormulatorr<T> : IFormulator<Dictionary<T, int>> where T : notnull
    {
        public Dictionary<T, int> Generate()
        {
            return new();
        }
    }

    public class DicFormulatorr<T> : IFormulator<Dictionary<int, T>>
    {
        public Dictionary<int, T> Generate()
        {
            return new();
        }
    }



}
