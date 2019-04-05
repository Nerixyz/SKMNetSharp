using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SKMNET.Client;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Scripting
{
    public class ScriptManager
    {
        public const string NS_NETWORK_CLIENT = "SKMNET.Client.Networking.Client";
        public const string NS_SKMNET = "SKMNET";
        public const string AS_SKMNET = "SKM.Net";

        public Context Context { get; set; }

        public Dictionary<string, string> Scripts { get; }

        public ScriptManager(Context ctx)
        {
            this.Context = ctx;
            this.Scripts = new Dictionary<string, string>();
        }

        public static Context SetupContext(LightingConsole console, string name = "lightingConsole")
        {
            Context ctx = new Context();
            IEnumerable<Type> query = from a in AppDomain.CurrentDomain.GetAssemblies()
                                      where a.GetName().Name.Equals(AS_SKMNET)
                                      from type in a.GetTypes()
                                      where type.Namespace.StartsWith(NS_SKMNET)
                                      select type;

            foreach(Type t in query)
            {
                if (!((t.IsClass && !t.IsAbstract) || t.IsEnum))
                    continue;

                try
                {
                        ctx.DefineConstructor(t);
                }
                catch (InvalidOperationException) { }
            }
            ctx.DefineVariable(name).Assign(JSValue.Marshal(console));

            return ctx;
        }

        public static string BuildDocs()
        {
            IEnumerable<Type> query = from a in AppDomain.CurrentDomain.GetAssemblies()
                                      where a.GetName().Name.Equals(AS_SKMNET)
                                      from type in a.GetTypes()
                                      where type.Namespace.StartsWith(NS_SKMNET)
                                      select type;

            JArray root = new JArray();
            foreach (Type t in query) {
                if (!((t.IsClass && !t.IsAbstract) || t.IsEnum))
                    continue;

                JObject current = new JObject
                {
                    { "name", new JValue(t.Name) }
                };
                if(t.IsClass || t.IsValueType)
                {
                    //Constructors
                    JArray constructors = new JArray();
                    foreach(ConstructorInfo cInfo in t.GetConstructors())
                    {
                        JObject constructor = new JObject() { { "name", new JValue(cInfo.Name) } };
                        JArray parameters = new JArray();
                        foreach(var pInfo in cInfo.GetParameters())
                        {
                            parameters.Add(new JObject { { pInfo.Name, new JValue(pInfo.ParameterType.Name) } });
                        }
                        constructor.Add("parameters", parameters);
                        constructors.Add(constructor);
                    }
                    current.Add("constructors", constructors);

                    //Fields
                    JArray fields = new JArray();
                    foreach(var fInfo in t.GetFields())
                    {
                        fields.Add(new JObject() { { fInfo.Name, new JValue(fInfo.FieldType.Name) } });
                    }
                    current.Add("fields", fields);

                    //Methods
                    JArray methods = new JArray();
                    foreach(var mInfo in t.GetMethods())
                    {
                        JObject method = new JObject() { { "name", new JValue(mInfo.Name) } };
                        JArray parameters = new JArray();
                        foreach(var pInfo in mInfo.GetParameters())
                        {
                            parameters.Add(new JObject() { { pInfo.Name, new JValue(pInfo.ParameterType.Name) } });
                        }
                        method.Add("parameters", parameters);
                    }
                    current.Add("methods", methods);
                }else if (t.IsEnum)
                {
                    JArray values = new JArray();
                    foreach(var name in t.GetEnumNames())
                    {
                        values.Add(name);
                    }
                    current.Add("values", values);
                }


                root.Add(current);
            }
            return root.ToString();
        }

        public void LoadScript(string path, string name)
        {
            string text = File.ReadAllText(path);
            Scripts.Add(name, text);
        }

        public async Task LoadScriptAsync(string path, string name)
        {
            string text = await File.ReadAllTextAsync(path).ConfigureAwait(false);
            Scripts.Add(name, text);
        }

        public void ExecuteScript(string name)
        {
            Context.Eval(Scripts[name]);
        }
    }
}
