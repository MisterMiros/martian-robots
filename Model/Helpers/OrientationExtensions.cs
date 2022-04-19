namespace Model.Helpers
{
    public static class OrientationExtensions
    {
        // return new orientation, after number of turns
        // positive turns -- right turns, negative -- left turns
        public static Orientation Turn90(this Orientation orientation, int turns)
        {
            return (Orientation)(((int)(orientation + turns) % 4 + 4) % 4);
        }
    }
}