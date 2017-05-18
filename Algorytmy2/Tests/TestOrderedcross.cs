using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2.Tests
{
    public class OperatorsTest
    {
        public List<Point> points = new List<Point>();
        public Graph gp1;
        public Graph gp2;
        public OperatorsTest()
        {
            MakePoints();
            Graph gp1 = MakeGraph(false,3);
            Graph gp2 = MakeGraph(true,3);
            Operators.OrderCrossover(gp1, gp2, 6, 34);
            Operators.InvertOrder(gp1, 5, 3);
        }
        public Graph MakeGraph(bool secondary,int startIndex)
        {
            List<Point> pt = new List<Point>();
            pt.Add(points.Where(x => x.ID == startIndex).First());
            pt.AddRange(points.Where(x => x.ID != startIndex).ToList());
            List<Point> qt = new List<Point>();
            qt.Add(points.Where(x => x.ID == startIndex).First());
            qt.AddRange(points.Where(x => x.ID != startIndex).OrderBy(x => x.ID).ToList());
            return new Graph(secondary ? pt.ToArray() : qt.ToArray());
        }
        public void MakePoints()
        {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Point p = new Point()
                {
                    X = r.NextDouble(),
                    Y = r.NextDouble(),
                    ID = r.Next(0, 10)
                };
                if (points.Count > 0)
                {
                    while (points.Where(x => x.ID == p.ID).FirstOrDefault() != null)
                    {
                        p.ID = r.Next(0, 10);
                    }
                }
                points.Add(p);
            }
        }
    }
}

