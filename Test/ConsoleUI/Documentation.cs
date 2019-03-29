using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ConsoleUI
{
    public static class Documentation
    {
        public static void Document(Type t)
        {
            ConstructorInfo[] infos = t.GetConstructors();
            Console.WriteLine("Documentation for " + t.Name + ":");
            Console.WriteLine("\n");
            for(int i = 0; i < infos.Length; i++)
            {
                ConstructorInfo info = infos[i];
                Console.WriteLine("Constructor [" + i + "] " + t.Name + info.Name + ":");
                Constructor(info);
            }
        }

        public static void Constructor(ConstructorInfo constructorInfo)
        {
            Console.WriteLine("Parameters: ");
            ParameterInfo[] pInfos = constructorInfo.GetParameters();
            foreach(ParameterInfo info in pInfos)
            {
                Parameter(info);
            }
        }

        public static void Parameter(ParameterInfo info)
        {
            Console.WriteLine($"{info.Name}: {info.ParameterType.Name}");
        }
    }
}
