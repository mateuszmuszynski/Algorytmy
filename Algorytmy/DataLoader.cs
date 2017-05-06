using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    public class DataLoader
    {
        private readonly string _filePath;
        private readonly NumberStyles _numberStyle;

        public DataLoader(string filePath, System.Globalization.NumberStyles numberStyle = NumberStyles.None)
        {
            _filePath = filePath;
            _numberStyle = numberStyle;
        }

        public List<Coordinate> GetData()
        {
            var results = new List<Coordinate>();

            using (StreamReader sr = new StreamReader(_filePath))
            {
                var text = sr.ReadToEnd().Replace('.', ',').Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                text.RemoveAt(0);

                foreach (var item in text)
                {
                    var splitted = item.Split(' ');
                    var coordinate = new Coordinate
                    {
                        X = float.Parse(splitted[0], _numberStyle),
                        Y = float.Parse(splitted[1], _numberStyle)
                    };

                    results.Add(coordinate);
                }
            }

            return results;
        }
    }
}
