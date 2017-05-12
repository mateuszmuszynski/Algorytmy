using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    public static class SolutionChecker
    {
        public static bool Check(int[] path, double[,] distanceMatrix)
        {
            var duplicatedPoints =
                path.ToList().GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y).ToList();

            if (duplicatedPoints.Count > 1 || duplicatedPoints.First().Count() > 2)
            {
                return false;
            };

            var first = path[0];
            var last = path[path.Length - 1];

            if (first != last)
            {
                return false;
            }

            return true;
        }
    }
}
