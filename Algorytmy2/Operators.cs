using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2
{
    public static class Operators
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
        public static Graph OrderCrossover(Graph parent1,Graph parent2,int startPosition,int elementCount)
        {
            Point[] points = new Point[parent1.Points.Count()];
            for (int i=startPosition;i<startPosition+elementCount;i++)
            {
                points[i] = parent1.Points[i];
            }
            int positionCopied = startPosition + elementCount;
            int positionInserted = startPosition + elementCount;
            for (int i=0;i<parent2.Points.Count();i++)
            {
                Point copiedPoint = parent2.Points[positionCopied + i % parent2.Points.Count()];
                if (!points.Where(x => x != null).Contains(copiedPoint))
                {
                    points[positionInserted % parent2.Points.Count()] = copiedPoint;
                    positionInserted ++;
                }
            }
            return new Graph(points);
        }
    }
}
