using System;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Model;
using NUnit.Framework;
using UnitTests.Helpers;
using VectorInt;

namespace UnitTests
{
    public class RobotTests
    {
        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            generator = fixture.Create<Generator<int>>();
        }

        [Test]
        [AutoData]
        public void Robot_ShouldStayInPlace_WhenNoInstructionsProvided(int x, int y, Orientation orientation)
        {
            var grid = new Grid(x + 1, y + 1);
            var robot = grid.DeployRobot(Vector(x, y), orientation);
            robot.ExecuteInstructions("");
            robot.Position.Should().Be(Vector(x, y));
            robot.Orientation.Should().Be(orientation);
            robot.Lost.Should().BeFalse();
        }

        [Test]
        [AutoData]
        public void Robot_ShouldExecuteCorrectInstructions(int x, int y, Orientation orientation)
        {
            var grid = new Grid(x + 1, y + 1);
            var position = Vector(x, y);
            var robot = grid.DeployRobot(position, orientation);
            robot.ExecuteInstructions("RFRFRFRF"); // move in square, return to initial position and orientation
            robot.Orientation.Should().Be(orientation);
            robot.Position.Should().Be(position);
            robot.Lost.Should().BeFalse("should not get out of grid");
        }
        
        [Test]
        [AutoData]
        public void Robot_ShouldIgnoreUnknownInstructions(int x, int y, Orientation orientation)
        {
            var grid = new Grid(x + 1, y + 1);
            var position = Vector(x, y);
            var robot = grid.DeployRobot(position, orientation);
            robot.ExecuteInstructions("R1FRC** FRFgRrflF"); // move in square, return to initial position and orientation
            robot.Orientation.Should().Be(orientation);
            robot.Position.Should().Be(position);
            robot.Lost.Should().BeFalse("should not get out of grid");
        }
        
        [Test]
        [AutoData]
        public void Robot_ShouldGetLost_WhenOutOfGrid_AndAddScent(int x, int y, Orientation orientation)
        {
            var grid = new Grid(x + 1, y + 1);
            var position = Vector(x, y);
            var robot = grid.DeployRobot(position, orientation);
            robot.ExecuteInstructions(new string('F', Math.Max(grid.X, grid.Y))); // guaranteed to move out of grid
            robot.Orientation.Should().Be(orientation, "no turns");
            robot.Lost.Should().BeTrue("should get out of grid");
            grid.IsOutOfGrid(robot.Position).Should().BeFalse("last known position should be just out of grid");
            grid.IsScent(robot.Position).Should().BeTrue("on lost should add to scent");
        }
        
        [Test]
        [AutoData]
        public void Robot_ShouldNotGetLost_IfTryToMoveOutOfScent(int x, int y, Orientation orientation)
        {
            var grid = new Grid(x + 2, y + 2);
            var position = Vector(x + 1, y + 1);
            
            var badRobot = grid.DeployRobot(position, orientation);
            badRobot.ExecuteInstructions(new string('F', Math.Max(grid.X, grid.Y))); // guaranteed to move out of grid
            badRobot.Orientation.Should().Be(orientation, "no turns");
            badRobot.Lost.Should().BeTrue("should get out of grid");
            grid.IsOutOfGrid(badRobot.Position).Should().BeFalse("last known position should be just out of grid");
            grid.IsScent(badRobot.Position).Should().BeTrue("on lost should add to scent");

            var goodRobot = grid.DeployRobot(position, orientation);
            goodRobot.ExecuteInstructions(new string('F', Math.Max(grid.X, grid.Y)) + "RRFRR"); // move forward to scent, then move backwards
            goodRobot.Lost.Should().BeFalse();
            grid.IsOutOfGrid(goodRobot.Position).Should().BeFalse("last known position should be just out of grid");
            grid.IsScent(goodRobot.Position).Should().BeFalse();
        }

        [Test]
        [AutoData]
        public void Robot_ShouldWork_WithTestCases()
        {
            var grid = new Grid(5, 3);
            var robot1 = grid.DeployRobot(Vector(1, 1), Orientation.East);
            robot1.ExecuteInstructions("RFRFRFRF");
            robot1.Orientation.Should().Be(Orientation.East);
            robot1.Position.Should().Be(Vector(1, 1));
            robot1.Lost.Should().BeFalse();

            var robot2 = grid.DeployRobot(Vector(3, 2), Orientation.North);
            robot2.ExecuteInstructions("FRRFLLFFRRFLL");
            robot2.Orientation.Should().Be(Orientation.North);
            robot2.Position.Should().Be(Vector(3, 3));
            robot2.Lost.Should().BeTrue();
            grid.IsScent(robot2.Position).Should().BeTrue();

            var robot3 = grid.DeployRobot(Vector(0, 3), Orientation.West);
            robot3.ExecuteInstructions("LLFFFLFLFL");
            robot3.Orientation.Should().Be(Orientation.South);
            robot3.Position.Should().Be(Vector(2, 3));
            robot3.Lost.Should().BeFalse();
        }

        private static VectorInt2 Vector(int x, int y)
        {
            return new VectorInt2(x, y);
        }

        private Grid CreateBigGrid()
        {
            return new Grid(generator.GreaterThan(1), generator.GreaterThan(1));
        }

        private VectorInt2 PositionSomewhereInGrid(Grid grid)
        {
            return Vector(generator.Between(1, grid.X - 1), generator.Between(1, grid.Y - 1));
        }

        private Fixture fixture;
        private Generator<int> generator;
    }
}