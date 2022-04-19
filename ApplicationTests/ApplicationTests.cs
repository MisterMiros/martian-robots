using Application;
using ApplicationTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace ApplicationTests
{
    public class ApplicationTests
    {

        [Test]
        [TestCase("")]
        [TestCase("4")]
        [TestCase("AB 4")]
        [TestCase("4 fc")]
        [TestCase("ca fc")]
        [TestCase("4 -4")]
        [TestCase("0 4")]
        [TestCase("-4 0")]
        public void Application_ShouldWriteErrors_OnIncorrectGrid(string input)
        {
            var consoleManagerStub = new ConsoleManagerStub(
                new[]
                {
                     input,
                });
            var app = new ApplicationRunner(consoleManagerStub);
            app.Run();
            consoleManagerStub.GetWrittenLines().Should().BeEquivalentTo("Can't parse grid");
        }
        
        [Test]
        [TestCase("")]
        [TestCase("1")]
        [TestCase("1 1")]
        [TestCase("AB 1 N ")]
        [TestCase("1 fc N")]
        [TestCase("ca fc N")]
        [TestCase("1 -1 N")]
        [TestCase("1 1 O")]
        public void Application_ShouldWriteErrors_OnIncorrectRobot(string incorrectInput)
        {
            var consoleManagerStub = new ConsoleManagerStub(
                new[]
                {
                    "5 3",
                    "1 1 N",
                    "RFRFRFRF",
                    incorrectInput,
                    "F",
                    "1 1 E",
                    "RFRFRFRF",
                });
            var app = new ApplicationRunner(consoleManagerStub);
            app.Run();
            consoleManagerStub.GetWrittenLines().Should().BeEquivalentTo("1 1 N", "Can't parse robot");
        }
        
        [Test]
        public void Application_ShouldWork_OnSampleData()
        {
            var consoleManagerStub = new ConsoleManagerStub(
                new[]
                {
                    "5 3",
                    "1 1 E",
                    "RFRFRFRF",
                    "3 2 N",
                    "FRRFLLFFRRFLL",
                    "0 3 W",
                    "LLFFFLFLFL",
                });
            var app = new ApplicationRunner(consoleManagerStub);
            app.Run();
            consoleManagerStub.GetWrittenLines().Should().BeEquivalentTo("1 1 E", "3 3 N LOST", "2 3 S");
        }
    }
}