using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace RamerDouglasPeuckerNet.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Point[] points = File.ReadAllLines(@"D:\Trading\Quantitative\Research\Data\large_random.csv")
                .Select(line =>
                new Point(DateTime.Parse(line.Split(',')[0].Trim()).ToOADate(),
                  double.Parse(line.Split(',')[1].Trim()))).ToArray();

            points = points.Concat(points).ToArray();
            points = points.Concat(points).ToArray();
            points = points.Concat(points).ToArray();
            points = points.Concat(points).ToArray();
            points = points.Concat(points).ToArray();

            Console.WriteLine(points.Count().ToString("#,0"));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var reducePoints = RamerDouglasPeucker.Reduce(points, 0.5);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds / 1_000.0 + "s");
            Console.WriteLine(reducePoints.Count().ToString("#,0"));
            Console.ReadKey();
            //File.WriteAllLines(@"screenpoints_reduced.csv", reducePoints.Select(x => x.X + "," + x.Y));
        }
    }
}
