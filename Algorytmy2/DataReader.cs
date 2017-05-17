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
        Timer t = new Timer(24000);
        List<Point> Points { get; set; }
        public static List<Edge> Edges { get; set; }
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
                Operators op = new Operators();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił problem z odzcyztem danych wejściowych");
            }
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            return 
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
                    if (p != q /*&& !Paths.Exists(x => x.A == q && x.B == p)*/ )
                    {
                        Edges.Add(new Edge(p, q));
                    }
                }
            }
        }
        public static Edge[] GetEdges(Point[] points)
        {
            Edge[] graphEdges = new Edge[points.Count()];
            for (int i = 0; i <= points.Count() - 1; i++)
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
