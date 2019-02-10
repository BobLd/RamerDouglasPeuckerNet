using System.IO;
using System.Linq;

namespace RamerDouglasPeuckerNet.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Point[] points = File.ReadAllLines(@"screenpoints.csv")
                .Select(line =>
                    new Point(double.Parse(line.Split(',')[0].Trim()),
                              double.Parse(line.Split(',')[1].Trim()))).ToArray();

            var reducePoints = RamerDouglasPeucker.Reduce(points, 1.0);
            File.WriteAllLines(@"screenpoints_reduced.csv", reducePoints.Select(x => x.X + "," + x.Y));
        }
    }
}
