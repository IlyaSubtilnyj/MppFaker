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
    using Target = Type;

    public static class Composer
    {

        private static Dictionary<Type, List<FormulatorDecorator>> ComposersMap = new();

        public static void Remember(Type outputTarget, FormulatorDecorator formulator)
        {

            List<FormulatorDecorator>? targetQueue = null;
            if (ComposersMap.TryGetValue(outputTarget, out targetQueue))
                targetQueue.Add(formulator);
            else ComposersMap.Add(outputTarget, new List<FormulatorDecorator>{ formulator });
        }

        public static object Compose(Target target)
        {

            if (!ComposersMap.TryGetValue(target, out var composers))
            {

                var targetDef = target.GetGenericTypeDefinition();

                if (ComposersMap.TryGetValue(targetDef, out composers))
                {

                    var mostCompatibleFormulator = composers.Aggregate(composers[0], delegate (FormulatorDecorator mostCompatible, FormulatorDecorator current)
                    {

                        if (FormulatorDecorator.MoreCompatible(current, mostCompatible, target))
                            return current;
                        else return mostCompatible;
                    });

                    if (FormulatorDecorator.IsCompatible(mostCompatibleFormulator, target))
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

        public class FormulatorDecorator
        {
            private static Type DTOContract = typeof(IFormulator<>);

            private Type    _classMeta;
            private Type    _outTargetInstanceType;
            private Type[]  _args;
            private Type    _specialization;

            private bool isTemplated(Type t)
            {
                return t.FullName == null;
            }

            public FormulatorDecorator(Type contender)
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

            public static bool MoreCompatible(FormulatorDecorator f1, FormulatorDecorator f2, Target target)
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

            public static bool IsCompatible(FormulatorDecorator formulator, Target target)
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

            public FormulatorDecorator Specialize(Target target)
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
            Type[] pluginTypes = pluginAssembly.GetTypes().Where(t => FormulatorDecorator.isFullfillContract(t)).ToArray();
            foreach (var pluginType in pluginTypes)
            {

                new FormulatorDecorator(pluginType);
            }
        }

    }
}
