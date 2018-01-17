using System;
using System.Collections.Generic;

namespace AppDomainRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var domainSetup = new AppDomainSetup
            {
                ApplicationBase = @"C:\Temp\Plugins"
            };

            var subAppDomain = AppDomain.CreateDomain("a", null, domainSetup);

            var plugins = new List<IDoStuff>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IDoStuff).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = subAppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                        plugins.Add((IDoStuff)instance);
                    }
                }
            }

            foreach (var plugin in plugins)
            {
                plugin.DoStuff();
            }

            Console.Read();
        }
    }

    internal interface IDoStuff
    {
        void DoStuff();
    }

    //[Serializable]
    public class CoolStuffPlugin :MarshalByRefObject, IDoStuff
    {
        public void DoStuff()
        {
            Console.WriteLine("Doing cool stuff");
        }
    }
}