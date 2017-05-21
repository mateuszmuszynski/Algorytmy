using Microsoft.Maps.MapControl.WPF;
using System;
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
using System.Timers;

namespace Algorytmy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    /// Bing MAPS DOWNLOAD: https://www.microsoft.com/en-us/download/details.aspx?displaylang=en&id=27165
    public partial class MainWindow : Window
    {
        private Algorytmy2.DataReader rdr;
        public Algorytmy2.DataReader Rdr
        {
            get
            {
                if (rdr == null)
                {
                    rdr = new Algorytmy2.DataReader(int.Parse(startPointTextBox.Text), int.Parse(elementsToTakeTextbox.Text));
                }
                return rdr;
            }
        }
        Timer t = new Timer(35000);
        Timer t2 = new Timer(35000);
        Algorytmy2.Point startPoint;
        public MainWindow()
        {
            InitializeComponent();

            t.Elapsed += T_Elapsed;
            t2.Elapsed += T2_Elapsed;
        }

        public void scaleToFitCanvas(List<Coordinate> coordinates)
        {
            var minX = coordinates.Min(x => x.X);
            var maxX = coordinates.Max(x => x.X);

            var maxY = coordinates.Max(x => x.Y);
            var minY = coordinates.Min(x => x.Y);

            var xDiff = maxX - minX;
            var yDiff = maxY - minY;

            var scaleX = canvas.ActualWidth / xDiff;
            var scaleY = canvas.ActualHeight / yDiff;

            var scale = scaleX > scaleY ? scaleY : scaleX;

            foreach (var coordinate in coordinates)
            {
                coordinate.X = (coordinate.X - minX) * scaleX;
                coordinate.Y = (coordinate.Y - minY) * scaleY;
            }
        }

        public void scaleToFitCanvas(List<Algorytmy2.Point> coordinates)
        {
            var minX = coordinates.Min(x => x.X);
            var maxX = coordinates.Max(x => x.X);

            var maxY = coordinates.Max(x => x.Y);
            var minY = coordinates.Min(x => x.Y);

            var xDiff = maxX - minX;
            var yDiff = maxY - minY;

            var scaleX = canvas.ActualWidth / xDiff;
            var scaleY = canvas.ActualHeight / yDiff;

            var scale = scaleX > scaleY ? scaleY : scaleX;

            foreach (var coordinate in coordinates)
            {
                coordinate.X = (coordinate.X - minX) * scaleX;
                coordinate.Y = (coordinate.Y - minY) * scaleY;
            }
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e)
        {
            PathResult result;

            var hillClimbing = new HillClimbing();

            List<Coordinate> data;

            var numberOfElementsToTake = int.Parse(elementsToTakeTextbox.Text);

            var startPoint = int.Parse(startPointTextBox.Text);

            var loader = new DataLoader(@"..\..\test.txt", NumberStyles.Float);
            data = loader.GetData();
            result = hillClimbing.GetPath(data[startPoint - 1], data, numberOfElementsToTake);

            scaleToFitCanvas(data.Where(x => result.Path.Contains(data.IndexOf(x))).ToList());

            foreach (var child in canvas.Children)
            {
                if (child is Polyline)
                {
                    ((Polyline)child).Points.Clear();
                }
            }

            foreach (var item in result.Path)
            {
                var currentCoordinate = data[item];

                polyline.Points.Add(new Point(currentCoordinate.X, currentCoordinate.Y));
            }

            var firstItem = data[result.Path.First()];

            Canvas.SetTop(ellipse, firstItem.Y - 5);
            Canvas.SetLeft(ellipse, firstItem.X - 5);

            distanceLabel.Content = "Distance: " + result.Distance;

            var fileData = result.Path.Select(x => (x + 1).ToString()).Aggregate((current, next) => current + " " + next);
            var fileName = @"C:\temp\result" + string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".txt";

            StreamWriter file = new System.IO.StreamWriter(fileName, true);
            file.WriteLine(fileData);

            file.Close();
        }

        private void calculateButtonMap_Click(object sender, RoutedEventArgs e)
        {
            PathResult result;

            var hillClimbing = new HillClimbing();

            List<Coordinate> data;

            var numberOfElementsToTake = int.Parse(elementsToTakeTextbox.Text);

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

            var selectedName = ((ComboBoxItem)cityComboBox.SelectedValue).Content;

            var currentCity = cities.First(x => x.Name == (string)selectedName);

            result = hillClimbing.GetPath(currentCity.Coordinate, data, numberOfElementsToTake);

            mapPolygon.Locations.Clear();


            var childsToRemove = map.Children.OfType<Pushpin>().Cast<UIElement>().ToList();

            foreach (var child in childsToRemove)
            {
                map.Children.Remove(child);
            }

            var pin = new Pushpin();
            pin.Location = new Location(currentCity.Coordinate.X, currentCity.Coordinate.Y);
            map.Children.Add(pin);

            foreach (var item in result.Path)
            {
                var location = data[item];

                mapPolygon.Locations.Add(new Location(location.X, location.Y));
            }

            distanceLabelMap.Content = "Distance: " + result.Distance;

            var fileData = result.Path.Select(x => (x + 1).ToString()).Aggregate((current, next) => current + " " + next);
            var fileName = @"C:\temp\result" + string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".txt";

            StreamWriter file = new System.IO.StreamWriter(fileName, true);
            file.WriteLine(fileData);

            file.Close();
        }

        private void calculateButton2_Click(object sender, RoutedEventArgs e)
        {
            t.Start();
            try {
                rdr = new Algorytmy2.DataReader(int.Parse(startPointTextBox2.Text), int.Parse(elementsToTakeTextbox.Text));
            }
            catch(Exception)
            {
                t.Stop();
            }
            
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                distanceLabel2.Content = rdr.FastestRoadLength;



                var startPoint = int.Parse(startPointTextBox2.Text);

                var loader = new DataLoader(@"..\..\test.txt", NumberStyles.Float);


                scaleToFitCanvas(rdr.FastestRoad.ToList());

                foreach (var child in canvas2.Children)
                {
                    if (child is Polyline)
                    {
                        ((Polyline)child).Points.Clear();
                    }
                }

                foreach (Algorytmy2.Point item in rdr.FastestRoad)
                {
                    var currentCoordinate = item;

                    polyline2.Points.Add(new Point(currentCoordinate.X, currentCoordinate.Y));
                    if (rdr.FastestRoad.Last() == currentCoordinate)
                    {
                        currentCoordinate = rdr.FastestRoad.First();
                        polyline2.Points.Add(new Point(currentCoordinate.X, currentCoordinate.Y));
                    }
                }

                var firstItem = rdr.FastestRoad.First();

                Canvas.SetTop(ellipse2, firstItem.Y - 5);
                Canvas.SetLeft(ellipse2, firstItem.X - 5);

                var fileData = rdr.FastestRoad.Select(x => x.ID.ToString()).Aggregate((current, next) => current + " " + next);
                var fileName = @"C:\temp\result" + string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".txt";

                StreamWriter file = new System.IO.StreamWriter(fileName, true);
                file.WriteLine(fileData);

                file.Close();
            });
            t.Stop();
        }

        private void calculateButtonMapGenetic_Click(object sender, RoutedEventArgs e)
        {
            var numberOfElementsToTake = int.Parse(elementsToTakeTextbox.Text);

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
            var selectedName = ((ComboBoxItem)cityGeneticComboBox.SelectedValue).Content;
            var currentCity = cities.First(x => x.Name == (string)selectedName);

            startPoint = new Algorytmy2.Point()
            {
                ID = numberOfElementsToTake,
                X = currentCity.Coordinate.X,
                Y = currentCity.Coordinate.Y,
            };
            t2.Start();
            rdr = new Algorytmy2.DataReader(numberOfElementsToTake, startPoint);

        }


        private void T2_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                mapGeneticPolygon.Locations.Clear();
                var childsToRemove = mapGenetic.Children.OfType<Pushpin>().Cast<UIElement>().ToList();

                foreach (var child in childsToRemove)
                {
                    mapGenetic.Children.Remove(child);
                }
                var pin = new Pushpin();
                pin.Location = new Location(startPoint.X, startPoint.Y);
                mapGenetic.Children.Add(pin);

                foreach (Algorytmy2.Point item in rdr.FastestRoad)
                {
                    var location = item;
                    mapGeneticPolygon.Locations.Add(new Location(location.X, location.Y));
                }

                distanceLabelMapGenetic.Content = "Distance: " + rdr.FastestRoadLength;

                var fileData = rdr.FastestRoad.Select(x => x.ID.ToString()).Aggregate((current, next) => current + " " + next);
                var fileName = @"C:\temp\result" + string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".txt";

                StreamWriter file = new System.IO.StreamWriter(fileName, true);
                file.WriteLine(fileData);

                file.Close();
            
            });
        }
    }
}