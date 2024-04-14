using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public class Faker
    {

        private DTOContainer _container = new();

        public T Create<T>()
        {

            T? result = default;
            Type type = typeof(T);

            if (_container.Has(type))
            {
                result = (T)_container.Get(type);
            }         

            return result;
        }
    }
}
