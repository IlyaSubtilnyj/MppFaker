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
            Composer.load("D:\\workspace\\Visual_Studio_workspace\\studing_workspace\\Faker-proj\\GeneratorsPlugin\\bin\\Debug\\net6.0\\GeneratorsPlugin.dll");
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
