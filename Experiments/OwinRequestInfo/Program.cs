using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinRequestInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var app = WebApp.Start<Startup>("http://+:5000"))
            {
                Console.ReadLine();
            }
        }
    }
}
