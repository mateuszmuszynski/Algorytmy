using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy2
{
    public static class Operators
    {
        public static Point[] InvertOrder(Graph baseGraph, int elementCount, int startPosition)
        {
            Point[] points = new Point[baseGraph.Points.Count()];

            int it = 0;
            Point[] toInvert = baseGraph.Points.ToList().GetRange(startPosition, elementCount).ToArray();
            Point[] inverted = new Point[toInvert.Length];
            for (int j = 0; j < (int)Math.Ceiling((double)toInvert.Length); j++)
            {
                inverted[j] = toInvert[toInvert.Length - j-1];
            }
            for (int i = 0; i < baseGraph.Points.Count(); i++)
            {
                if (i >= startPosition && i <= startPosition + elementCount - 1)
                {
                    points[i] = inverted[i - startPosition];
                }
                else
                {
                    points[i] = baseGraph.Points[i];
                }
            }
            return points;
        }
        public static Point[] OrderCrossover(Graph parent1, Graph parent2, int startPos, int count)
        {
            int elementCount = startPos + count < parent1.Points.Count() ? count : parent1.Points.Count() - startPos - 1; 
            int startPosition = startPos > 0 ? startPos : 1;
            Point[] pointsFrom1 = new Point[elementCount];
            pointsFrom1 = parent1.Points.ToList().GetRange(startPosition,elementCount).ToArray();
            Point[] childPoints = new Point[parent1.Points.Count()];
            for (int i = 0; i < elementCount; i++)
            {
                childPoints[(i + startPosition) % childPoints.Length] = pointsFrom1[i];
            }
            int index = startPosition + elementCount;
            for (int j = 0; j < parent2.Points.Count(); j++)
            {
                if (childPoints[index] == null)
                {
                    if (!childPoints.Contains(parent2.Points[(startPosition + elementCount + j) % parent2.Points.Count()]))
                    {
                        childPoints[index] = parent2.Points[(startPosition + elementCount + j) % parent2.Points.Count()];
                        index = (index + 1) % parent2.Points.Count();
                    }
                }
            }
            return childPoints;
        }
    }
}
