using System.Linq;
using AutoFixture;

namespace UnitTests.Helpers
{
    public static class FixtureExtensions
    {
        
        public static int GreaterThan(this Generator<int> generator, int value)
        {
            return generator.First(x => x > value);
        }
        
        public static int Negative(this Generator<int> generator)
        {
            return generator.First(x => x > 0) * -1;
        }
        
        public static int Between(this Generator<int> generator, int from, int to)
        {
            return generator.First(x => x >= from && x <= to);
        }
    }
}