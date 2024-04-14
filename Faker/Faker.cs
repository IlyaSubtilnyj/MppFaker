using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public sealed class Faker
    {

        private static Container container;

        /// <summary>
        /// Каждый Faker пользуется общим DTO resolving контейнером.
        /// </summary>
        static Faker()
        {
           container = new();
        }

        public static T Create<T>()
        {

            T? result = default;
            Type type = typeof(T);

            if (container.has(type))
            {
                result = (T)container.get(type);
            }         

            return result;
        }
    }
}
