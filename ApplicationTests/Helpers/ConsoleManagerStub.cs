using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Application.ConsoleManager;

namespace ApplicationTests.Helpers
{
    public class ConsoleManagerStub : IConsoleManager
    {
        private readonly IEnumerator<string> enumerator;

        private readonly List<string> writtenLines;

        public ConsoleManagerStub(IEnumerable<string> inputLines)
        {
            enumerator = inputLines.GetEnumerator();
            writtenLines = new List<string>();
        }

        public IImmutableList<string> GetWrittenLines()
        {
            return writtenLines.ToImmutableArray();
        }

        public string ReadLine()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            enumerator.Dispose();
            return null;
        }

        public void WriteLine(string line)
        {
            writtenLines.Add(line);
        }
    }
}