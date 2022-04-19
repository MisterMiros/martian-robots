using System.Collections.Generic;
using Model.Helpers;
using VectorInt;

namespace Model
{
    public class Robot
    {
        public Grid Grid { get; private set; }
        public  VectorInt2 Position { get; private set; }
        public  Orientation Orientation { get; private set; }
        public  bool Lost { get; private set; }

        public Robot(Grid grid, VectorInt2 position, Orientation orientation)
        {
            Grid = grid;
            Position = position;
            Orientation = orientation;
        }

        public void ExecuteInstructions(string instructions)
        {
            if (Lost)
            {
                return;
            }

            foreach (var instruction in instructions)
            {
                ExecuteInstruction(instruction);
                if (Lost)
                {
                    break;
                }   
            }
        }
        private void ExecuteInstruction(char instruction)
        {
            switch (instruction)
            {
                case 'R':
                    Orientation = Orientation.Turn90(1);
                    return;
                case 'L':
                    Orientation = Orientation.Turn90(-1);
                    return;
                case 'F' :
                    Move();
                    return;
                default:
                    return;
            }
        }

        private void Move()
        {
            var newPosition = Position + positionShiftByOrientation[Orientation];
            if (Grid.IsOutOfGrid(newPosition))
            {
                if (!Grid.IsScent(Position))
                {
                    Lost = true;
                    Grid.AddScent(Position);
                }
                return;
            }
            Position = newPosition;
        }

        private static readonly Dictionary<Orientation, VectorInt2> positionShiftByOrientation = new Dictionary<Orientation, VectorInt2>
        {
            { Orientation.North, VectorInt2.Down },
            { Orientation.East, VectorInt2.Right },
            { Orientation.South, VectorInt2.Up },
            { Orientation.West, VectorInt2.Left },

        };
    }
}