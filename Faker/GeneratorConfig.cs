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

        public static void Load(string pluginPath)
        {
            Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);

            // Find types that implement your plugin interface
            Type pluginInterfaceType = typeof(IGenerator);
            Type[] pluginTypes = pluginAssembly.GetTypes().Where(t => pluginInterfaceType.IsAssignableFrom(t)).ToArray();

            // Instantiate plugin objects
            foreach (Type pluginType in pluginTypes)
            {
                IGenerator plugin = (IGenerator)Activator.CreateInstance(pluginType)!;
                // Do something with the plugin object
            }
        }
    }
}
