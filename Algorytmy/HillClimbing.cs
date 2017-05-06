using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    public class HillClimbing
    {
        public int[] GetPath(Coordinate startCoordinate, List<Coordinate> coordinates, int numberOfPaths)
        {
            var startCoordinateItem =
                coordinates.FirstOrDefault(x => x.X == startCoordinate.X && x.Y == startCoordinate.Y);

            if (startCoordinateItem == null)
            {
                coordinates.Add(startCoordinate);
                startCoordinateItem = coordinates.Last();
            }

            return GetPaths(coordinates.IndexOf(startCoordinateItem), coordinates, numberOfPaths);
        }

        private int[] GetPaths(int startCoordinate, List<Coordinate> coordinates, int numberOfPaths)
        {
            int[,] distanceMatrix = new int[coordinates.Count,coordinates.Count];

            for (var i = 0; i < coordinates.Count; i++)
            {
                for (var j = 0; j < coordinates.Count; j++)
                {
                    distanceMatrix[i, j] = Distance(coordinates[i], coordinates[j]);
                }
            }

            int[] bestPath = new int[coordinates.Count + 1];
            var bestPathDistance = int.MaxValue;

            for (int i = 0; i < 10; i++)
            {
                var path = CreatePaths(startCoordinate, coordinates.Count, numberOfPaths);

                TwoOpt(path, distanceMatrix, 200);

                var optDistance = GetDistance(path, distanceMatrix);

                if (optDistance < bestPathDistance)
                {
                    bestPath = path;
                    bestPathDistance = optDistance;
                }

                var result = SolutionChecker.Check(path, distanceMatrix);
            }

            return bestPath;
        }

        private int GetDistance(int[] path, int[,] distanceMatrix)
        {
            var total = 0;

            for (int i = 0; i < path.Length - 2; i++)
            {
                total += distanceMatrix[path[i], path[i+1]];
            }
            return total;
        }

        private int[] CreatePaths(int startItem, int totalItems, int numberOfItemsToTake)
        {
            int[] vertices = new int[numberOfItemsToTake + 1];

            var random = new Random();

            var randomNumbers =
                Enumerable.Range(0, totalItems)
                    .Where(x => x != startItem)
                    .OrderBy(x => random.Next())
                    .Take(numberOfItemsToTake + 1).ToList();

            for (int i = 1; i < numberOfItemsToTake; i++)
            {
                vertices[i] = randomNumbers[i-1];
            }

            return vertices;
        }

        private void TwoOpt(int[] path, int[,] distances, int numberOfIterations)
        {
            var iterator = 0;

            do
            {
                for (var i = 0; i < path.Length - 2; i++)
                {
                    for (var j = i + 2; j < path.Length - 1; j++)
                    {

                        var currentDistance = distances[path[i], path[i + 1]] +
                                              distances[path[j], path[j + 1]];

                        var newDistance = distances[path[i], path[j]] + distances[path[i + 1], path[j + 1]];

                        var change = newDistance - currentDistance;

                        if (change < 0)
                        {
                            var temp = path[i + 1];
                            path[i + 1] = path[j];
                            path[j] = temp;
                        }
                    }
                }

                iterator++;
            } while (iterator < numberOfIterations);
        }

        public int Distance(Coordinate coordinate1, Coordinate coordinate2)
        {
            return (int)(Math.Sqrt(Math.Pow(coordinate1.X - coordinate2.X, 2) +
                                        Math.Pow(coordinate1.Y - coordinate2.Y, 2)));
        }
    }
}
