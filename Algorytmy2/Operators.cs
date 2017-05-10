using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2
{
    public class Operators
    {
        public static List<Edge> EdgeCollection { get; set; }
        public static Graph InvertOrder(Graph baseGraph,int elementCount,int startPosition)
        {
            Point[] points = new Point[baseGraph.Points.Count()];
            
            for(int i=0;i< baseGraph.Points.Count();i++)
            {
                if(i >= startPosition && i<= startPosition + elementCount)
                {
                    int it = 0;
                    for (int j=startPosition;j<startPosition + elementCount%2;j++)
                    {
                        Point clipboard = points[j + it];
                        points[j + it] = points[startPosition + elementCount - it];
                        points[startPosition + elementCount - it] = clipboard;
                        it++;
                    }
                }
                else
                {
                    points[i] = baseGraph.Points[i];
                }
            }
            return new Graph(points);
        }

    }
}
