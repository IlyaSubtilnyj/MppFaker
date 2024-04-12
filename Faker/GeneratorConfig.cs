using GeneratorsPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public class GeneratorConfig
    {

        public static Dictionary<Type, IGenerator> Load(string pluginPath)
        {

            Dictionary<Type, IGenerator> loadedGenerators = new();

            Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);

            Type pluginInterfaceType = typeof(IGenerator);
            Type[] pluginTypes = pluginAssembly.GetTypes().Where(t => pluginInterfaceType.IsAssignableFrom(t)).ToArray();

            foreach (Type pluginType in pluginTypes)
            {
                if (pluginType.IsGenericType)
                {

                    var gen_attr = (GeneratorAttribute)pluginType.GetCustomAttribute(typeof(GeneratorAttribute))!;
                    var type = gen_attr.GenereatorType;

                    //IGenerator plugin = (IGenerator)Activator.CreateInstance(pluginType)!;

                    loadedGenerators.Add(type, (IGenerator)Activator.CreateInstance(typeof(Int32Generator))!);

                } else
                {
                    var gen_attr = (GeneratorAttribute)pluginType.GetCustomAttribute(typeof(GeneratorAttribute))!;
                    var type = gen_attr.GenereatorType;

                    IGenerator plugin = (IGenerator)Activator.CreateInstance(pluginType)!;

                    loadedGenerators.Add(type, plugin);
                }

            }

            return loadedGenerators;
        }
    }
}
