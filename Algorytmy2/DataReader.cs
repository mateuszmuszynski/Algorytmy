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
        Timer t = new Timer(10000);
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
                Graph[] start = CreateFirstRoads(startIndex, Count.Value);
                FastestRoad = start[0].Points;
                FastestRoadLength = (int)GetEdges(FastestRoad).Sum(u => u.Length);
                FindBestRoad(start[0], start[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił problem z odczytem danych wejściowych");
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
            int qLength = (int)GetEdges(q).Sum(w => w.Length);
            int pLength = (int)GetEdges(p).Sum(l => l.Length);
            if (qLength < FastestRoadLength && pLength < FastestRoadLength)
            {
                FastestRoadLength = pLength >= qLength ? pLength : qLength;
                FastestRoad = pLength >= qLength ? p : q;
                FindBestRoad(new Graph(p), new Graph(q));
            }
            else
            {
                if (qLength < FastestRoadLength)
                {
                    FastestRoad = q;
                    FastestRoadLength = qLength;
                    FindBestRoad(x, new Graph(q));
                }
                else
                {
                    if (pLength < FastestRoadLength)
                    {
                        FastestRoad = p;
                        FastestRoadLength = pLength;
                        FindBestRoad(new Graph(p), y);
                    }
                    else
                    {
                        FindBestRoad(x, y);
                    }
                }
            }
            currentLength = (int)GetEdges(q).Sum(w => w.Length);

            currentLength = (int)GetEdges(p).Sum(l => l.Length);
            if (currentLength > FastestRoadLength)
            {
                FastestRoad = p;
                FastestRoadLength = currentLength;
            }

        }
        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(FastestRoadLength);
            foreach (Point p in FastestRoad)
                Console.Write(p.ID + " =>");
            Console.WriteLine("");
        }

        private void ReadFile()
        {
            int px = 0;
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
                            ID = px++
                        };
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

        private Graph[] CreateFirstRoads(int startIndex, int itemsToTake)
        {
            List<Point> RoadShortest = new List<Point>();
            List<Point> RoadLongest = new List<Point>();
            List<Point> usedPoints = new List<Point>();
            Point startPoint = Points.Where(x => x.ID == startIndex).FirstOrDefault();
            RoadShortest.Add(startPoint);
            RoadLongest.Add(startPoint);
            Point previousItemShortest;
            Point previousItemLongest;
            for (int i = 0; i < itemsToTake - 1; i++)
            {
                previousItemShortest = Edges.Where(x => x.A == startPoint && !RoadShortest.Contains(x.B)).OrderBy(x => x.Length).FirstOrDefault().B;
                RoadShortest.Add(previousItemShortest);
                startPoint = previousItemShortest;
            }
            startPoint = Points.Where(x => x.ID == startIndex).FirstOrDefault();
            for (int i = 0; i < itemsToTake - 1; i++)
            {
                previousItemLongest = Edges.Where(x => x.A == startPoint && !RoadLongest.Contains(x.B)).OrderByDescending(x => x.Length).FirstOrDefault().B;
                RoadLongest.Add(previousItemLongest);
                startPoint = previousItemLongest;
            }
            //List<Point> Road1 = new List<Point>();
            //List<Point> Road2 = new List<Point>();
            //Road1.Add(Points.;i+ Where(x => x.ID == startIndex).First());
            //Road1.AddRange(Points.Where(x => x.ID != startIndex).OrderBy(x => x.ID).ToList());
            //Road2.Add(Points.Where(x => x.ID == startIndex).First());
            //Road2.AddRange(Points.Where(x => x.ID != startIndex).OrderByDescending(x => x.ID).ToList());
            Graph[] trasyPoczatkowe = new Graph[2];
            trasyPoczatkowe[0] = new Graph(RoadLongest.ToArray());
            trasyPoczatkowe[1] = new Graph(RoadShortest.ToArray());
            return trasyPoczatkowe;
        }
    }
}
