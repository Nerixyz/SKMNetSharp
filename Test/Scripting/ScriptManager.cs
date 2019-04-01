using NiL.JS.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using SKMNET.Client;
using System.IO;
using System.Threading.Tasks;

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
                if ((t.IsClass && !t.IsAbstract) || t.IsEnum)
                {
                    try
                    {
                        ctx.DefineConstructor(t);
                    }catch(InvalidOperationException _) { }
                }
            }
            ctx.DefineVariable(name).Assign(JSValue.Marshal(console));

            return ctx;
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
