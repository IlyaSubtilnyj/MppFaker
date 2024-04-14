using DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    using static DataTransferObject.Composer;
    using Target = Type;

    public static class Composer
    {

        private static Dictionary<Type, Type> keyValuePairs = new();
        private static Dictionary<Type, List<Type>> genericKeyValuePairs = new();

        private static Dictionary<Type, List<Formulator>> ComposersMap = new();

        public static void Remember(Type outputTarget, Formulator formulator)
        {

            List<Formulator>? targetQueue = null;
            if (ComposersMap.TryGetValue(outputTarget, out targetQueue))
                targetQueue.Add(formulator);
            else ComposersMap.Add(outputTarget, new List<Formulator>{ formulator });
        }

        public static object Compose(Target target)
        {

            if (!ComposersMap.TryGetValue(target, out var composers))
            {

                var targetDef = target.GetGenericTypeDefinition();

                if (ComposersMap.TryGetValue(targetDef, out composers))
                {

                    var mostCompatibleFormulator = composers.Aggregate(composers[0], delegate (Formulator mostCompatible, Formulator current)
                    {

                        if (Formulator.MoreCompatible(current, mostCompatible, target))
                            return current;
                        else return mostCompatible;
                    });

                    if (Formulator.IsCompatible(mostCompatibleFormulator, target))
                    {

                        return mostCompatibleFormulator.Specialize(target).Formulate();
                    }                         
                }
            }
            else
            {
                return composers[0].Formulate();
            }

            return default;
        }

        public class Formulator
        {
            private static Type DTOContract = typeof(IFormulator<>);

            private Type _classMeta;
            private Type _outTargetInstanceType;
            private Type[] _args;
            private Type _specialization;

            private bool isTemplated(Type t)
            {
                return t.FullName == null;
            }

            public Formulator(Type contender)
            {

                this._classMeta = contender;
                this._specialization = contender;

                var outTargetInstanceType = _classMeta.GetInterface(DTOContract.Name)!.GetGenericArguments()[0];
                this._args = outTargetInstanceType.GetGenericArguments();

                if (isTemplated(outTargetInstanceType))
                    this._outTargetInstanceType = outTargetInstanceType.GetGenericTypeDefinition();      
                else
                    this._outTargetInstanceType = outTargetInstanceType;

                Composer.Remember(this._outTargetInstanceType, this);
            }

            public Target Target()
            {

                return this._outTargetInstanceType;
            }

            public Type[] Args()
            {

                return this._args;
            }

            public static int ArgIsCompatible(Type arg, Type targetArg)
            {
                if (arg.FullName == null) { return 1; }
                else if (arg == targetArg) { return 2; }
                else return 0;
            }

            public static bool MoreCompatible(Formulator f1, Formulator f2, Target target)
            {
                var arguments = target.GetGenericArguments();
                var f1Args = f1.Args();
                var f2Args = f2.Args();

                for (var i = 0; i < arguments.Length; i++)
                {
                    var f1Compatability = ArgIsCompatible(f1Args[i], arguments[i]);
                    var f2Compatability = ArgIsCompatible(f2Args[i], arguments[i]);

                    if (f1Compatability > f2Compatability) return true;
                    else if (f1Compatability < f2Compatability) return false;
                }
                return false;
            }

            public static bool IsCompatible(Formulator formulator, Target target)
            {
                var arguments = target.GetGenericArguments();
                var formulatorArgs = formulator.Args();

                for (var i = 0; i < arguments.Length; i++)
                {

                    var compatability = ArgIsCompatible(formulatorArgs[i], arguments[i]);
                    if (compatability == 0) return false;
                }

                return true;
            }

            public Formulator Specialize(Target target)
            {
       
                var args = target.GetGenericArguments();
                var classArgs = _classMeta.GetGenericArguments();
                var interfaceArgs = this.Args();

                List<Type> resultArgs = new List<Type>();

                for(int i = 0; i < args.Length; i++)
                {

                    if (interfaceArgs[i].FullName == null)
                        resultArgs.Add(args[i]);
                }

                this._specialization = this._classMeta.MakeGenericType(resultArgs.ToArray());
                return this;
            }

            public object Formulate()
            {

                var specInstance = Activator.CreateInstance(this._specialization);
                var generationMethod = this._specialization.GetMethod("Generate");
                return generationMethod.Invoke(specInstance, null)!;
            }

            public static bool isFullfillContract(Type t)
            {

                return t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == DTOContract);
            }

        }

        public static void Load(string path)
        {

            Assembly pluginAssembly = Assembly.LoadFrom(path);           
            Type[] pluginTypes = pluginAssembly.GetTypes().Where(t => Formulator.isFullfillContract(t)).ToArray();
            foreach (var pluginType in pluginTypes)
            {

                new Formulator(pluginType);
            }
        }

    }
}
