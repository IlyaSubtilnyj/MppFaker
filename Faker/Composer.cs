using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    public static class Composer
    {

        private static Dictionary<Type, Type> keyValuePairs = new();
        private static Dictionary<Type, List<Type>> genericKeyValuePairs = new();

        public static void load(string path)
        {
            Assembly pluginAssembly = Assembly.LoadFrom(path);

            var contructType = typeof(IFormulator<>);
            Type[] pluginTypes = pluginAssembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == contructType)).ToArray();

            foreach (var pluginType in pluginTypes)
            {

                var target = pluginType.GetInterface(contructType.Name)!.GetGenericArguments()[0];

                if (target.FullName == null)
                {
                    Type genericDefinition = target.GetGenericTypeDefinition();
                    List<Type>? queue = null;
                    genericKeyValuePairs.TryGetValue(genericDefinition, out queue);
                    if (queue == null)
                    {
                        genericKeyValuePairs.Add(genericDefinition, new List<Type> { pluginType });
                    }
                    else
                    {
                        queue.Add(pluginType);
                    }
                }
                else
                {
                    Type? pluginTypep = null;
                    keyValuePairs.TryGetValue(target, out pluginTypep);
                    if (pluginTypep == null)
                        keyValuePairs.Add(target, pluginType);
                }

            }
        }

        public static void isCompatible(Type t1, Type t2, out int comp)
        {
            if (t1.FullName == null) { comp = 1; }
            else if (t1 == t2) { comp = 2; }
            else comp = 0;
        }

        public static bool isFullCompatible(Type[] types, Type[] template)
        {
            int result = 2;
            for(int i = 0; i < template.Length; i++)
            {
                isCompatible(types[i], template[i], out result);
                if (result == 0)
                    return false;
            }
            return true;
        }

        public static bool isMoreCompatible(Type[] types1, Type[] types2, Type[] template)
        {
            var length = template.Length;
            for (var i = 0; i < length; i++)
            {
                int t1Comp;
                int t2Comp;
                isCompatible(types1[i], template[i], out t1Comp);
                isCompatible(types2[i], template[i], out t2Comp);

                if (t1Comp > t2Comp) return true;
                else if (t1Comp < t2Comp) return false;
            }
            return false;

        }

        public static object Formulate(Type target)
        {
        
            Type? isNotTemplated = null;
            keyValuePairs.TryGetValue(target, out isNotTemplated);

            if (isNotTemplated == null)
            {

                if (!target.IsGenericType) { return default; }

                var formerGenericDefinition = target.GetGenericTypeDefinition();
                var formerGenericDefinitionArguments = target.GetGenericArguments();

                List<Type>? genericQueue = null;
                if (genericKeyValuePairs.TryGetValue(formerGenericDefinition, out genericQueue))
                {
                    var mostCompatibleFormulator = genericQueue.Aggregate(genericQueue.FirstOrDefault()!, delegate (Type acc, Type current)
                    {
                        var curArgs = current.GetInterface(typeof(IFormulator<>).Name).GetGenericArguments()[0].GetGenericArguments();
                        var accArgs = acc.GetInterface(typeof(IFormulator<>).Name).GetGenericArguments()[0].GetGenericArguments();
                        var tArgs = target.GetGenericArguments();

                        if (isMoreCompatible(curArgs, accArgs, tArgs))
                            return current;
                        else return acc;
                    });

                    //if (!isFullCompatible(mostCompatibleFormulator.GetInterface(typeof(IFormulator<>).Name).GetGenericArguments()[0].GetGenericArguments(), target.GetGenericArguments()))
                    //    return default;

                    var genericType = mostCompatibleFormulator.MakeGenericType(formerGenericDefinitionArguments[0]);
                    var generic = Activator.CreateInstance(genericType);
                    MethodInfo method = genericType.GetMethod("Generate");
                    return method.Invoke(generic, null);
                }

            }
            else
            {
                var generic = Activator.CreateInstance(isNotTemplated);
                MethodInfo method = isNotTemplated.GetMethod("Generate");
                return method.Invoke(generic, null);
            }

            return default;
        }
    }
}
