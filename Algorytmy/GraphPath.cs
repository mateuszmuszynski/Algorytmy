using System;

namespace Algorytmy
{
    public class GraphPath
    {
        public Coordinate To { get; set; }
        public Coordinate From { get; set; }

        public double Distance
        {
            get {
                return Math.Sqrt(Math.Pow(Math.Abs(From.X - To.X), 2) +
                           Math.Pow(Math.Abs(From.Y - To.Y), 2));
            }
        }

        public int Order { get; set; }
    }
}