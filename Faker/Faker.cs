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

        private static Container container;

        /// <summary>
        /// Каждый Faker пользуется общим DTO resolving контейнером.
        /// </summary>
        static Faker()
        {
            container = new();
        }

        public T Create<T>()
        {

            T? result = default;
            Type type = typeof(T);

            if (container.Has(type))
            {
                result = (T)container.Get(type);
            }         

            return result;
        }
    }
}
