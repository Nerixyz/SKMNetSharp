using SKMNET;
using SKMNET.Client;
using SKMNET.Client.Networking.Client;
using SKMNET.Client.Stromkreise;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleUI
{
    public class ConsoleInterface
    {
        public static List<Type> RegisteredInstanceTypes = new List<Type>()
        {
            typeof(DmxData),
            typeof(DMXSelect),
            typeof(FixPar),
            typeof(FixParDimmer),
            typeof(Mailbox),
            typeof(PalEdit),
            typeof(ParList),
            typeof(ParSelect),
            typeof(MonitorSelect),
            typeof(SKAnwahl),
            typeof(SKGAnwahl),
            typeof(SKMSync),
            typeof(PalCommand),
            typeof(PalSelect),
            typeof(BedienfehlermeldungEvent),
            typeof(BedientasteEvent),
            typeof(LastkreistasteEvent),
            typeof(MacroEvent),
            typeof(MonitorEvent),
            typeof(MouseEvent),
            typeof(StellermeldungEvent),
            typeof(TextTastEvent)
        };

        public Action PreQuery;

        public void StartAndBlock(LightingConsole console, Action preQ)
        {
            string cmd;
            this.PreQuery = preQ;
            while(!(cmd = Console.ReadLine()).Equals("end"))
            {
                Loop(cmd, console);
            }
        }

        public void Loop(string cmd, LightingConsole console)
        {
            CArgument a = ParseString(cmd);
            if (a is CText)
            {
                throw new Exception("No Function in Command");
            }
            else
            {
                CFunction func = a as CFunction;
                HandleFunction(func, console);
            }
        }

        public void HandleFunction(CFunction func, LightingConsole console)
        {
            switch (func.Name)
            {
                case "s":
                    {
                        //expected 2 args
                        if (func.Arguments.Count != 2 || !(func.Arguments[0] is CFunction))
                            throw new ArgumentException("Expected two Arguments");

                        CFunction f = func.Arguments[0] as CFunction;
                        int index = int.Parse(func.Arguments[1].Text);
                        Type t = RegisteredInstanceTypes.Find((x) => x.Name.Equals(f.Name));
                        if (t is null)
                            throw new NullReferenceException("Could not find Type");

                        ConstructorInfo constructor = t.GetConstructors()[index];
                        ParameterInfo[] infos = constructor.GetParameters();
                        object[] args = new object[infos.Length];

                        for (int i = 0; i < infos.Length; i++)
                        {
                            args[i] = CreateParameters(f.Arguments[i], infos[i]);
                        }
                        object instance = constructor.Invoke(args);
                        PreQuery?.Invoke();
                        console.QueryAsync(instance as CPacket).Wait();

                        break;
                    }
                case "i":
                    {
                        if (func.Arguments.Count != 2)
                            throw new ArgumentException("Expected two Arguments");

                        ushort num = ushort.Parse(func.Arguments[0].Text);
                        byte intensity = byte.Parse(func.Arguments[1].Text);
                        SK sk = console.Stromkreise.Find((x) => x.Number == num);
                        sk.Intensity = intensity;
                        PreQuery?.Invoke();
                        console.QueryAsync(new FixParDimmer(Enums.FixParDst.Current, sk)).Wait();

                        break;
                    }
            }
        }

        private object CreateBase(CFunction f, Type t)
        {
            string a = f.Arguments[0].Text;
            switch (f.Name)
            {
                case "e":
                    {
                        return Enum.Parse(t, a);
                    }
                case "b":
                    {
                        return byte.Parse(a);
                    }
                case "i":
                    {
                        return int.Parse(a);
                    }
                case "d":
                    {
                        return double.Parse(a);
                    }
                case "s":
                    {
                        return short.Parse(a);
                    }
                case "us":
                    {
                        return ushort.Parse(a);
                    }
                default:
                    throw new ArgumentException("Could not find " + f.Name);
            }
        }

        public object CreateParameters(CArgument arg, ParameterInfo info)
        {
            if (arg is CFunction)
            {
                CFunction f = arg as CFunction;
                if (f.Name.Equals("null"))
                    return null;
                if (info.ParameterType.BaseType == typeof(ValueType) || info.ParameterType.BaseType == typeof(Enum))
                {
                    return CreateBase(f, info.ParameterType);
                }else if(info.ParameterType.BaseType == typeof(Array) && f.Name.Equals("a"))
                {
                    Array array = info.ParameterType.GetConstructors()[0].Invoke(new object[] { f.Arguments.Count }) as Array;
                    for(int i = 0; i < array.Length; i++)
                    {
                        if (f.Arguments[i] is CFunction)
                        {
                            object param = CreateBase(f.Arguments[i] as CFunction, info.ParameterType);
                            array.SetValue(param, i);
                        }
                    }
                    return array;
                }
                else
                {
                    if (f.Name.Equals("ctor"))
                    {
                        CFunction target = f.Arguments[0] as CFunction;
                        int index = int.Parse(f.Arguments[1].Text);

                        if (!info.ParameterType.Name.Equals(f.Arguments[0]))
                            throw new EntryPointNotFoundException("Could not find Constructor for: " + info.ParameterType.Name);

                        ConstructorInfo constructor = info.ParameterType.GetConstructors()[index];
                        ParameterInfo[] infos = constructor.GetParameters();
                        object[] args = new object[infos.Length];
                        for (int i = 0; i < infos.Length; i++)
                        {
                            args[i] = CreateParameters(target.Arguments[i], infos[i]);
                        }
                        return constructor.Invoke(args);
                    }
                    else
                    {
                        throw new Exception("Could not handle CFunction");
                    }
                }

            }
            else
            {
                return (arg as CText)?.Text;
            }
        }

        public CArgument ParseString(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            if (s.Contains('('))
            {
                int i = s.IndexOf('(', 0);
                string name = s.Substring(0, i);

                if (!s.EndsWith(')'))
                    throw new Exception("Could not parse String at " + i);

                string args = s.Substring(i + 1, s.Length - 1 - (i + 1));
                if (!args.Contains(","))
                {
                    return new CFunction(s)
                    {
                        Name = name,
                        Arguments = new List<CArgument>() { new CText(args) }
                    };
                }
                else
                {
                    CFunction f = new CFunction(s)
                    {
                        Name = name,
                        Arguments = new List<CArgument>()
                    };
                    i = 0;
                    int steps = 0;
                    StringBuilder builder = new StringBuilder();
                    while (i < args.Length)
                    {
                        char c = args[i];
                        if (c == '(')
                            steps++;
                        if (c == ')')
                            steps--;
                        if (c == ',' && steps == 0)
                        {
                            f.Arguments.Add(ParseString(builder.ToString()));
                            builder = new StringBuilder();
                            i++;
                            continue;
                        }
                        builder.Append(c);

                        i++;
                    }
                    f.Arguments.Add(ParseString(builder.ToString()));
                    return f;
                }
            }
            else
            {
                return new CText(s);
            }

        }

        public class CFunction : CArgument
        {
            public CFunction(string text) : base(text)
            {

            }

            public string Name { get; set; }

            public List<CArgument> Arguments { get; set; }
        }

        public class CText : CArgument
        {
            public CText(string text) : base(text)
            {
            }
        }

        public abstract class CArgument
        {
            public string Text { get; protected set; }

            protected CArgument(string text)
            {
                this.Text = text;
            }
        }
    }
}
