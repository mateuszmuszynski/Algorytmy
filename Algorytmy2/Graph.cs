using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2
{
    public class Graph
    {
        public Point[] Points { get; set; }
        public Edge[] Edges
        {
            get
            {
                return DataReader.GetEdges(Points);
            }
        }
        private int length;
        public int Length
        {
            get
            {
                return (int)Edges.Sum(x => x.Length);      
            }
        }

        public Graph() { }
        public Graph(Point[] points)
        {
            Points = points;
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Edge e in Edges)
            {
                builder.Append((Edges.ElementAt(0) == e ? e.A.ID : e.B.ID) + "/");
            }
            return builder.ToString();
        }
    }
}
