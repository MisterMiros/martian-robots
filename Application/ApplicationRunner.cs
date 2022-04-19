using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.ConsoleManager;
using Model;
using VectorInt;

namespace Application
{
    public class ApplicationRunner
    {
        private readonly IConsoleManager consoleManager;

        public ApplicationRunner(IConsoleManager consoleManager)
        {
            this.consoleManager = consoleManager;
        }
        
        public void Run()
        {
            var gridLine = consoleManager.ReadLine();
            if (gridLine == null)
            {
                return;
            }
            var grid = ParseGrid(gridLine);
            if (grid == null)
            {
                consoleManager.WriteLine("Can't parse grid");
                return;
            }

            while (true)
            {
                var robotLine = consoleManager.ReadLine();
                if (robotLine == null)
                {
                    return;
                }

                var instructions = consoleManager.ReadLine();
                if (instructions == null)
                {
                    return;
                }

                var robot = ParseRobot(grid, robotLine);
                if (robot == null)
                {
                    consoleManager.WriteLine("Can't parse robot");
                    return;
                }
                robot.ExecuteInstructions(instructions);
                consoleManager.WriteLine(GetRobotStatus(robot));
            }
        }

        private static string GetRobotStatus(Robot robot)
        {
            return $"{robot.Position.X} {robot.Position.Y} {stringByOrientation[robot.Orientation]}{(robot.Lost ? " LOST" : "")}";
        }
        
        private static Grid ParseGrid(string input)
        {
            var match = GridRegex.Match(input);
            if (!match.Success)
            {
                return null;
            }

            var x = int.Parse(match.Groups["x"].Value);
            var y = int.Parse(match.Groups["y"].Value);
            if (x <= 0 || y <= 0)
            {
                return null;
            }
            return new Grid(x, y);
        }

        private static Robot ParseRobot(Grid grid, string input)
        {
            var match = RobotRegex.Match(input);
            if (!match.Success)
            {
                return null;
            }

            var x = int.Parse(match.Groups["x"].Value);
            var y = int.Parse(match.Groups["y"].Value);
            
            if (x < 0 || y < 0)
            {
                return null;
            }

            var orientation = ParseRobotOrientation(match.Groups["orientation"].Value);
            if (orientation == null)
            {
                return null;
            }

            return grid.DeployRobot(new VectorInt2(x, y), orientation.Value);
        }

        private static readonly Dictionary<Orientation, string> stringByOrientation = new Dictionary<Orientation, string>
        {
            { Orientation.North, "N" },
            { Orientation.East, "E" },
            { Orientation.South, "S" },
            { Orientation.West, "W" },
        };

        private static Orientation? ParseRobotOrientation(string input)
        {
            return input switch
            {
                "N" => Orientation.North,
                "E" => Orientation.East,
                "S" => Orientation.South,
                "W" => Orientation.West,
                _ => null,
            };
        }

        private static readonly Regex GridRegex = new Regex("(?<x>\\d+) (?<y>\\d+)");
        private static readonly Regex RobotRegex = new Regex("(?<x>\\d+) (?<y>\\d+) (?<orientation>[NESW])");
    }
}