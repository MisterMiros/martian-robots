using System;

namespace Application.ConsoleManager
{
    public class ConsoleManager : IConsoleManager
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}