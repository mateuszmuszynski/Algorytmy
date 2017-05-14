﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Algorytmy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void scaleToFitCanvas(List<Coordinate> coordinates)
        {
            var minX = coordinates.Min(x => x.X);
            var maxX = coordinates.Max(x => x.X);

            var maxY = coordinates.Max(x => x.Y);
            var minY = coordinates.Min(x => x.Y);

            var xDiff = maxX - minX;
            var yDiff = maxY - minY;

            var scaleX = Width/xDiff*0.7;
            var scaleY = Height/yDiff*0.7;

            var scale = scaleX > scaleY ? scaleY : scaleX;

            foreach (var coordinate in coordinates)
            {
                coordinate.X = (coordinate.X - minX)*scaleX;
                coordinate.Y = (coordinate.Y - minY)*scaleY;
            }
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            PathResult result;

            var hillClimbing = new HillClimbing();

            List<Coordinate> data;

            var numberOfElementsToTake = int.Parse(elementsToTakeTextbox.Text);
            var numberOfGenerations = int.Parse(this.numberOfGenerations.Text);
            var numberOf2Opts = int.Parse(numberOfTwoOpts.Text);

            if (OurData.IsChecked.Value)
            {
                var cities = new List<City>
                {
                    new City
                    {
                        Name = "Bialystok",
                        Coordinate = new Coordinate
                        {
                            X = 53.13248859999999,
                            Y = 23.1688403
                        }
                    },
                    new City
                    {
                        Name = "Warszawa",
                        Coordinate = new Coordinate
                        {
                            X = 52.2296756,
                            Y = 21.0122287
                        }
                    },
                    new City
                    {
                        Name = "Krakow",
                        Coordinate = new Coordinate
                        {
                            X = 50.06465009999999,
                            Y = 19.9449799
                        }
                    },
                    new City
                    {
                        Name = "Lodz",
                        Coordinate = new Coordinate
                        {
                            X = 51.7592485,
                            Y = 19.4559833
                        }
                    },
                    new City
                    {
                        Name = "Wroclaw",
                        Coordinate = new Coordinate
                        {
                            X = 51.1078852,
                            Y = 17.0385376
                        }
                    },
                    new City
                    {
                        Name = "Poznan",
                        Coordinate = new Coordinate
                        {
                            X = 52.406374,
                            Y = 16.9251681
                        }
                    },
                    new City
                    {
                        Name = "Gdansk",
                        Coordinate = new Coordinate
                        {
                            X = 54.35202520000001,
                            Y = 18.6466384
                        }
                    },
                    new City
                    {
                        Name = "Szczecin",
                        Coordinate = new Coordinate
                        {
                            X = 53.4285438,
                            Y = 14.5528116
                        }
                    },
                    new City
                    {
                        Name = "Bydgoszcz",
                        Coordinate = new Coordinate
                        {
                            X = 53.12348040000001,
                            Y = 18.0084378
                        }
                    },
                    new City
                    {
                        Name = "Lublin",
                        Coordinate = new Coordinate
                        {
                            X = 51.2464536,
                            Y = 22.5684463
                        }
                    }
                };


                var loader = new DataLoader(@"..\..\daneNasze.txt", NumberStyles.Float);
                data = loader.GetData();

                result = hillClimbing.GetPath(cities[0].Coordinate, data, numberOfElementsToTake, numberOfGenerations,
                    numberOf2Opts);
            }
            else
            {
                var loader = new DataLoader(@"..\..\test.txt", NumberStyles.Float);
                data = loader.GetData();
                result = hillClimbing.GetPath(data[0], data, numberOfElementsToTake, numberOfGenerations, numberOf2Opts);
            }

            scaleToFitCanvas(data);

            foreach (var child in canvas.Children)
            {
                if (child is Polyline)
                {
                    ((Polyline) child).Points.Clear();
                }
            }

            foreach (var item in result.Path)
            {
                var currentCoordinate = data[item];

                polyline.Points.Add(new Point(currentCoordinate.Y, currentCoordinate.X));
            }

            var firstItem = data[result.Path.First()];

            Canvas.SetTop(ellipse, firstItem.X - 5);
            Canvas.SetLeft(ellipse, firstItem.Y - 5);

            distanceLabel.Content = "Distance: " + result.Distance;

            var fileData = result.Path.Select(x => (x+1).ToString()).Aggregate((current, next) => current + " " + next);
            var fileName = @"D:\result" + string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".txt";

            StreamWriter file = new System.IO.StreamWriter(fileName, true);
            file.WriteLine(fileData);

            file.Close();
        }
    }
}