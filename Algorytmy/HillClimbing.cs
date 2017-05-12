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
        public PathResult GetPath(Coordinate startCoordinate, List<Coordinate> coordinates, int numberOfPaths, int numberOfIterations, int numberOf2Opts)
        {
            var startCoordinateItem =
                coordinates.FirstOrDefault(x => x.X == startCoordinate.X && x.Y == startCoordinate.Y);

            if (startCoordinateItem == null)
            {
                coordinates.Add(startCoordinate);
                startCoordinateItem = coordinates.Last();
            }

            return GetPaths(coordinates.IndexOf(startCoordinateItem), coordinates, numberOfPaths, numberOfIterations, numberOf2Opts);
        }

        private PathResult GetPaths(int startCoordinateIndex, List<Coordinate> coordinates, int numberOfPaths, int numberOfIterations, int numberOf2Opts)
        {
            double[,] distanceMatrix = new double[coordinates.Count,coordinates.Count];

            for (var i = 0; i < coordinates.Count; i++)
            {
                for (var j = 0; j < coordinates.Count; j++)
                {
                    distanceMatrix[i, j] = Distance(coordinates[i], coordinates[j]);
                }
            }

            int[] bestPath = new int[coordinates.Count + 1];
            var bestPathDistance = double.MaxValue;

            for (int i = 0; i < numberOfIterations; i++)
            {
                var path = CreatePaths(startCoordinateIndex, coordinates.Count, numberOfPaths);

                TwoOpt3(path, distanceMatrix, numberOf2Opts);

                var optDistance = GetDistance(path, distanceMatrix);

                if (optDistance < bestPathDistance)
                {
                    bestPath = path;
                    bestPathDistance = optDistance;
                }

                var result = SolutionChecker.Check(path, distanceMatrix);
            }

            return new PathResult {Path = bestPath, Distance = bestPathDistance};
        }

        private double GetDistance(int[] path, double[,] distanceMatrix)
        {
            var total = 0.0;

            for (int i = 0; i < path.Length - 2; i++)
            {
                total += distanceMatrix[path[i], path[i+1]];
            }
            return total;
        }

        private int[] CreatePaths(int startItemIndex, int totalItems, int numberOfItemsToTake)
        {
            int[] vertices = new int[numberOfItemsToTake + 1];

            var random = new Random();

            var randomNumbers =
                Enumerable.Range(0, totalItems)
                    .Where(x => x != startItemIndex)
                    .OrderBy(x => random.Next())
                    .Take(numberOfItemsToTake + 1).ToList();

            for (int i = 1; i < numberOfItemsToTake; i++)
            {
                vertices[i] = randomNumbers[i-1];
            }

            vertices[0] = startItemIndex;
            vertices[numberOfItemsToTake] = startItemIndex;

            return vertices;
        }

        private void TwoOpt(int[] path, double[,] distances, int numberOfIterations)
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

        private void TwoOpt3(int[] path, double[,] distances, int numberOfIterations)
        {
            double minChange;
            int minI = int.MaxValue;
            int minJ = int.MaxValue;

            do
            {
                minChange = 0;
                for (var i = 0; i < path.Length - 2; i++)
                {
                    for (var j = i + 2; j < path.Length - 1; j++)
                    {
                        var currentDistance = distances[path[i], path[i + 1]] +
                                              distances[path[j], path[j + 1]];

                        var newDistance = distances[path[i], path[j]] + distances[path[i + 1], path[j + 1]];

                        var change = newDistance - currentDistance;

                        if (minChange > change)
                        {
                            minChange = change;
                            minI = i;
                            minJ = j;
                        }
                    }
                }

                var temp = path[minI + 1];
                path[minI + 1] = path[minJ];
                path[minJ] = temp;
            } while (minChange < 0);
        }

        public double Distance(Coordinate coordinate1, Coordinate coordinate2)
        {
            return (Math.Sqrt(Math.Pow(coordinate1.X - coordinate2.X, 2) +
                                        Math.Pow(coordinate1.Y - coordinate2.Y, 2)));
        }
    }
}
