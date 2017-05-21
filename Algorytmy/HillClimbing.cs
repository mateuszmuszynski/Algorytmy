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
        public PathResult GetPath(Coordinate startCoordinate, List<Coordinate> coordinates, int numberOfPaths)
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

        private PathResult GetPaths(int startCoordinateIndex, List<Coordinate> coordinates, int numberOfPaths)
        {
            double[,] distanceMatrix = new double[coordinates.Count, coordinates.Count];

            for (var i = 0; i < coordinates.Count; i++)
            {
                for (var j = 0; j < coordinates.Count; j++)
                {
                    distanceMatrix[i, j] = Distance(coordinates[i], coordinates[j]);
                }
            }

            var path = CreatePaths(startCoordinateIndex, coordinates.Count, numberOfPaths, coordinates, distanceMatrix);

            TwoOpt(path, distanceMatrix);

            var optDistance = GetDistance(path, distanceMatrix);

            var result = SolutionChecker.Check(path, distanceMatrix);

            return new PathResult { Path = path, Distance = optDistance };
        }


        private double GetDistance(int[] path, double[,] distanceMatrix)
        {
            var total = 0.0;

            for (int i = 0; i < path.Length - 1; i++)
            {
                total += distanceMatrix[path[i], path[i+1]];
            }
            return total;
        }


        private int[] CreatePaths(int startItemIndex, int totalItems, int numberOfItemsToTake, List<Coordinate> coordinates, double[,] distanceMatrix)
        {
            int[] vertices = new int[numberOfItemsToTake + 1];

            vertices[0] = startItemIndex;
            vertices[numberOfItemsToTake] = startItemIndex;

            var startItem = coordinates[startItemIndex];

            var usedCoordinates = new List<Coordinate>();

            usedCoordinates.Add(startItem);

            Random random = new Random();

            for (int i = 1; i < numberOfItemsToTake; i++)
            {
                var currentRandomValue = random.Next(20);

                if (currentRandomValue < 10)
                {

                    var previousItem = coordinates[vertices[i - 1]];

                    var closestItem = coordinates.Where(x => !usedCoordinates.Contains(x)).OrderBy(x => Distance(x, previousItem)).First();
                    //var closestItem = coordinates.Where(x => !usedCoordinates.Contains(x)).OrderBy(x => Distance(x, startItem)).First();

                    usedCoordinates.Add(closestItem);

                    vertices[i] = coordinates.IndexOf(closestItem);
                }
                else
                {
                    var closestItem = coordinates.Where(x => !usedCoordinates.Contains(x)).OrderBy(x => Distance(x, startItem)).First();
                    usedCoordinates.Add(closestItem);

                    vertices[i] = coordinates.IndexOf(closestItem);

                    //var availableItems = coordinates.Where(x => !usedCoordinates.Contains(x)).ToList();

                    //var totalAvailableItems = availableItems.Count();

                    //var itemToTake = availableItems[random.Next(totalAvailableItems)];

                    //usedCoordinates.Add(itemToTake);

                    //vertices[i] = coordinates.IndexOf(itemToTake);
                }
            }

            return vertices;
        }

        private void TwoOpt(int[] path, double[,] distances)
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
