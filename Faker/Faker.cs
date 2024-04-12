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
            container = new Container();
        }

        public static T Create<T>() where T : IDto
        {

            T? result = default(T);

            var className = typeof(T).AssemblyQualifiedName!;
            if (container.has(className))
            {
                result = (T)container.get(className);
            }         

            return result!;
        }
    }
}
