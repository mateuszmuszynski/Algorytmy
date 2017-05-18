using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Globalization;
using System.Timers;

namespace Algorytmy2
{
    public class DataReader
    {
        int? Count;
        Timer t = new Timer(360000);
        List<Point> Points { get; set; }
        public static List<Edge> Edges { get; set; }
        public static Point[] FastestRoad;
        public int FastestRoadLength;
        int operateCtr = 0;
        public DataReader(int startIndex)
        {
            t.Elapsed += T_Elapsed;
            try
            {
                Points = new List<Point>();
                Edges = new List<Edge>();
                ReadFile();
                CreatePaths();
                Graph[] start = CreateFirstRoads(startIndex);
                FastestRoad = start[0].Points;
                FastestRoadLength = (int)GetEdges(FastestRoad).Sum(u => u.Length);
                FindBestRoad(start[0], start[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił problem z odzcyztem danych wejściowych");
            }
        }

        private void FindBestRoad(Graph x, Graph y)
        {

            Point[] p = new Point[x.Points.Length];
            Point[] q = new Point[y.Points.Length];
            Random r = new Random();
            t.Start();
            int op = r.Next(0, 1);
            int elements = r.Next(0, Count.Value);
            int startPosition = r.Next(1, Count.Value - elements);
            int currentLength;
            switch (op)
            {
                case 0:
                    {
                        p = Operators.InvertOrder(x, elements, startPosition);
                        q = Operators.InvertOrder(y, elements, startPosition);
                        break;
                    }
                case 1:
                    {
                        p = Operators.OrderCrossover(x, y, startPosition, elements);
                        q = Operators.OrderCrossover(y, x, startPosition, elements);
                        break;
                    }
            }
            currentLength = (int)GetEdges(q).Sum(w => w.Length);
            if (currentLength < FastestRoadLength)
            {
                FastestRoad = q;
                FastestRoadLength = currentLength;
            }
            currentLength = (int)GetEdges(p).Sum(l => l.Length);
            if (currentLength > FastestRoadLength)
            {
                FastestRoad = p;
                FastestRoadLength = currentLength;
            }
            FindBestRoad(new Graph(p), new Graph(q));
        }
        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(FastestRoadLength);
            foreach (Point p in FastestRoad)
                Console.Write(p.ID + " =>");
        }

        private void ReadFile()
        {
            using (StreamReader file = new StreamReader(@"..\..\daneNasze.txt"))
            {
                Random r = new Random();
                while (!file.EndOfStream)
                {
                    if (!Count.HasValue)
                    {
                        Count = int.Parse(file.ReadLine());
                    }
                    string[] linia = file.ReadLine().Replace('.', ',').Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(linia[0]))
                    {
                        Point p = new Point()
                        {
                            X = double.Parse(linia[0], NumberStyles.Float),
                            Y = double.Parse(linia[1], NumberStyles.Float),
                            ID = r.Next(0, Count.Value)
                        };
                        while (Points.Where(x => x.ID == p.ID).FirstOrDefault() != null)
                        {
                            p.ID = r.Next(0, Count.Value);
                        }
                        Points.Add(p);
                    }
                }
            }
        }
        private void CreatePaths()
        {
            foreach (Point p in Points)
            {
                foreach (Point q in Points.Except(new List<Point> { p }))
                {
                    if (p != q)
                    {
                        Edges.Add(new Edge(p, q));
                    }
                }
            }
        }
        public static Edge[] GetEdges(Point[] points)
        {
            Edge[] graphEdges = new Edge[points.Count()];
            for (int i = 0; i < points.Count(); i++)
            {
                if (i < points.Count() - 1)
                {
                    graphEdges[i] = Edges.Where(x => x.A == points[i] && x.B == points[i + 1]).FirstOrDefault();
                }
                else
                {
                    graphEdges[i] = Edges.Where(x => x.A == points[i] && x.B == points[0]).FirstOrDefault();
                }
            }
            return graphEdges;
        }

        private Graph[] CreateFirstRoads(int startIndex)
        {
            List<Point> Road1 = new List<Point>();
            List<Point> Road2 = new List<Point>();
            Road1.Add(Points.Where(x => x.ID == startIndex).First());
            Road1.AddRange(Points.Where(x => x.ID != startIndex).OrderBy(x => x.ID).ToList());
            Road2.Add(Points.Where(x => x.ID == startIndex).First());
            Road2.AddRange(Points.Where(x => x.ID != startIndex).OrderByDescending(x => x.ID).ToList());
            Graph[] trasyPoczatkowe = new Graph[2];
            trasyPoczatkowe[0] = new Graph(Road1.ToArray());
            trasyPoczatkowe[1] = new Graph(Road2.ToArray());
            return trasyPoczatkowe;
        }
    }
}
