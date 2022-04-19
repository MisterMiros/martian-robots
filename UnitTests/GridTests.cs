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
    public class GridTests
    {
        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
        }
        
        [Test]
        [AutoData]
        public void IsOutOfGrid_ShouldWork(Grid grid)
        {
            var generator = fixture.Create<Generator<int>>();

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Between(0, grid.X),
                Y = generator.Between(0, grid.Y),
            }).Should().BeFalse("in grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.GreaterThan(grid.X),
                Y = generator.Between(0, grid.Y),
            }).Should().BeTrue("X greater than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Between(0, grid.X),
                Y = generator.GreaterThan(grid.Y),
            }).Should().BeTrue("Y greater than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.GreaterThan(grid.X),
                Y = generator.GreaterThan(grid.Y),
            }).Should().BeTrue("both greater than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Negative(),
                Y = generator.Between(0, grid.Y),
            }).Should().BeTrue("X lesser than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Between(0, grid.X),
                Y =generator.Negative(),
            }).Should().BeTrue("Y lesser than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Negative(),
                Y = generator.Negative(),
            }).Should().BeTrue("both lesser than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.Negative(),
                Y = generator.GreaterThan(grid.Y)
            }).Should().BeTrue("X lesser than grid, Y greater than grid");

            grid.IsOutOfGrid(new VectorInt2
            {
                X = generator.GreaterThan(grid.X),
                Y = generator.Negative(),
            }).Should().BeTrue("X greater than grid, Y lesser than grid");
        }

        [Test]
        [AutoData]
        public void AddScent_And_IsScent_ShouldWork(Grid grid, VectorInt2 scent)
        {
            grid.IsScent(scent).Should().BeFalse();
            Assert.DoesNotThrow(() => grid.AddScent(scent));
            grid.IsScent(scent).Should().BeTrue();
        }
        
        [Test]
        [AutoData]
        public void DeployRobot_ShouldCreateRobot(Grid grid)
        {
            var generator = fixture.Create<Generator<int>>();
            var position = new VectorInt2(generator.Between(0, grid.X), generator.Between(0, grid.Y));
            var orientation = fixture.Create<Orientation>();
            var robot = grid.DeployRobot(position, orientation);
            robot.Should().NotBeNull();
        }
        
        [Test]
        [AutoData]
        public void DeployRobot_OutOfGrid_ShouldReturnNull(Grid grid)
        {
            var generator = fixture.Create<Generator<int>>();
            var position = new VectorInt2(generator.GreaterThan(grid.X), generator.Negative());
            var orientation = fixture.Create<Orientation>();
            var robot = grid.DeployRobot(position, orientation);
            robot.Should().BeNull();
        }

        private Fixture fixture;
    }
}