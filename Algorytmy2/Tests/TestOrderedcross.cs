using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2.Tests
{
    public class TestOrderedCross
    {
        public List<Point> g1 = new List<Point>();
        public List<Point> g2 = new List<Point>();
        public Graph gp1;
        public Graph gp2;
        public TestOrderedCross()
        {
            Graph gp1 = MakeGraph(g1);
            Graph gp2 = MakeGraph(g2);
            Operators.OrderCrossover(gp1, gp2, 3, 5);
        }
        public Graph MakeGraph(List<Point> points)
        {
            List<Point> graphTempPath = points.ToList();
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Point p = new Point()
                {
                    X = r.NextDouble(),
                    Y = r.NextDouble(),
                    ID = r.Next(0, 10)
                };
                while (graphTempPath.Where(x => x.ID == p.ID).FirstOrDefault() != null)
                {
                    p.ID = r.Next(0, 10);
                }
                graphTempPath.Add(p);
            }
            return new Graph(graphTempPath.ToArray());
        }
    }
}

