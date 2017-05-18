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
                inverted[j] = toInvert[toInvert.Length - j - 1];
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
            int startPosition = startPos > 0 ? (startPos < parent1.Length ? startPos : parent1.Length - 1) : 1;
            Point[] pointsInP1 = parent1.Points.Skip(1).ToArray();
            Point[] pointsInP2 = parent2.Points.Skip(1).ToArray();
            Point[] child = new Point[parent1.Points.Length];
            child[0] = parent1.Points[0];
            int length = pointsInP2.Count();
            for (int i = 0; i < elementCount; i++)
            {
                child[startPosition + i] = parent1.Points[startPosition + i];
            }
            //indeks punktowWewnetrznych
            int index = startPosition + elementCount;
            for (int i = 0; i < pointsInP2.Length; i++)
            {
                if (child[index] == null)
                {
                    if (!child.Contains(pointsInP2[(startPosition - 1 + elementCount + i) % length]))
                    {
                        child[index] = pointsInP2[(startPosition - 1 + elementCount + i) % length];                        
                        index = (index + 1) % length;
                    }
                }
                else
                {
                    index = (index + 1) % length;
                }
            }
            return child;
        }
    }
}
