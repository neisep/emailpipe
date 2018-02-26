using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailpipeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Startup startup = new Startup();
            startup.Load();
            startup.Execute();


            Console.WriteLine("Running application");
            Console.ReadLine();
        }
    }
}
