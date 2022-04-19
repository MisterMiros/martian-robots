using System;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            new ApplicationRunner(new ConsoleManager.ConsoleManager()).Run();
        }
    }
}