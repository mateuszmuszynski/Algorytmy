using System;
using System.Collections.Generic;
using System.Globalization;
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
            var loader = new DataLoader(@"..\..\test.txt", NumberStyles.Float);
            var data = loader.GetData();

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

            var hillClimbing = new HillClimbing();
            var result = hillClimbing.GetPath(cities.First().Coordinate, data, 250);

            foreach (var item in result)
            {
                var currentCoordinate = data[item];

                polyline.Points.Add(new Point(currentCoordinate.X, currentCoordinate.Y));
            }
        }
    }
}