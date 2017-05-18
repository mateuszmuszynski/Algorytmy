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
        //public static Point[] OrderCrossover(Graph parent1, Graph parent2, int startPos, int count)
        //{
        //    Point[] pointsInP1 = parent1.Points.Skip(1).ToArray();
        //    Point[] pointsInP2 = parent2.Points.Skip(1).ToArray();
        //    int elementCount = startPos + count < parent1.Points.Count() ? count : parent1.Points.Count() - startPos - 1; 
        //    int startPosition = startPos > 0 ? startPos : 1;
        //    Point[] pointsFrom1 = new Point[elementCount];
        //    pointsFrom1 = parent1.Points.ToList().GetRange(startPosition,elementCount).ToArray();
        //    Point[] childPoints = new Point[parent1.Points.Count()];
        //    childPoints[0] = parent1.Points[0];
        //    for (int i = 0; i < elementCount; i++)
        //    {
        //        childPoints[(i + startPosition) % childPoints.Length] = pointsFrom1[i];
        //    }
        //    int index = startPosition + elementCount - 1;
        //    for (int j=1;j <pointsInP2.Count() + 1;j++)
        //    {
        //        if(childPoints[index% pointsInP2.Count()] == null)
        //        {
        //            if(!childPoints.Contains(pointsInP2[(startPosition + elementCount - 1 + j)%pointsInP2.Count()]))
        //            {
        //                childPoints[index%childPoints.Length] = pointsInP2[(index + j) % pointsInP2.Count()];
        //                index = index + 1;
        //            }
        //        }
        //        else
        //        {
        //            index++;
        //        }
        //    }
        //    return childPoints;
        //}

        public static Point[] OrderCrossover(Graph parent1, Graph parent2, int startPos, int count)
        {
            int elementCount = startPos + count < parent1.Points.Count() ? count : parent1.Points.Count() - startPos - 1;
            int startPosition = startPos > 0 ? startPos : 1;
            Point[] pointsInP1 = parent1.Points.Skip(1).ToArray();
            Point[] pointsInP2 = parent2.Points.Skip(1).ToArray();
            Point[] child = new Point[parent1.Points.Length];
            child[0] = parent1.Points[0];
            for (int i = 0; i < elementCount; i++)
            {
                child[startPosition + i] = parent1.Points[startPosition + 1];
            }
            //indeks punktowWewnetrznych
            int index = startPosition + elementCount;
            for (int i = 0; i < pointsInP2.Length; i++)
            {
                if (child[index] == null)
                {
                    if (!child.Contains(pointsInP2[(startPosition - 1 + elementCount + i) % pointsInP2.Length]))
                    {
                        child[index+1] = pointsInP2[(startPosition - 1 + elementCount + i) % pointsInP2.Length];
                        index = (index + 1) % (pointsInP2.Length);
                    }
                }
                else
                {
                    index = (index + 1) % pointsInP2.Length;
                }
            }
            return child;
        }
    }
}
