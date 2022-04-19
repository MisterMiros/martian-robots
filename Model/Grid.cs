using System.Collections.Generic;
using System.Collections.Immutable;
using VectorInt;

namespace Model
{
    public class Grid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public IImmutableSet<VectorInt2> Scents => scents.ToImmutableHashSet();

        private readonly HashSet<VectorInt2> scents;
        public Grid(int x, int y)
        {
            X = x;
            Y = y;
            scents = new HashSet<VectorInt2>();
        }

        public Robot DeployRobot(VectorInt2 position, Orientation orientation)
        {
            if (IsOutOfGrid(position))
            {
                return null;
            }
            return new Robot(this, position, orientation);
        }

        public bool IsOutOfGrid(VectorInt2 position)
        {
            return position.X > X
                   || position.X < 0
                   || position.Y > Y ||
                   position.Y < 0;
        }

        public bool IsScent(VectorInt2 position)
        {
            return Scents.Contains(position);
        }

        public void AddScent(VectorInt2 position)
        {
            scents.Add(position);
        }
    }
}