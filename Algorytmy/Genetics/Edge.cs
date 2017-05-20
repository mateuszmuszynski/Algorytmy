using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
namespace Algorytmy2
{
    public class Edge
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public double Length
        {
            get
            {
                return Sqrt(Pow(B.X - A.X, 2.0) + Pow(B.Y - A.Y, 2.0));
            }
        }
        public Edge(Point x,Point y)
        {
            A = x;
            B = y;
        }
    }
}
