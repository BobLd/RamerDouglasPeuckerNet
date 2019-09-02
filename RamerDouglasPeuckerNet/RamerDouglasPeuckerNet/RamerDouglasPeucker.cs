using System;
using System.Collections.Generic;
using System.Linq;

namespace RamerDouglasPeuckerNet
{
    /*
     * Sources:
     * https://www.codeproject.com/Articles/18936/A-C-Implementation-of-Douglas-Peucker-Line-Approxi
     * https://codereview.stackexchange.com/questions/29002/ramer-douglas-peucker-algorithm
     * Optimisations:
     *  - Do not use Sqrt function
     *  - Use unsafe code
     *  - Avoid duplicate computations in loop
     */
    public static class RamerDouglasPeucker
    {
        /// <summary>
        /// Uses the Ramer Douglas Peucker algorithm to reduce the number of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public static Point[] Reduce(Point[] points, double tolerance)
        {
            if (points == null || points.Length < 3) return points;
            if (double.IsInfinity(tolerance) || double.IsNaN(tolerance)) return points;
            tolerance *= tolerance;
            if (tolerance <= float.Epsilon) return points;

            int firstIndex = 0;
            int lastIndex = points.Length - 1;
            List<int> indexesToKeep = new List<int>();

            // Add the first and last index to the keepers
            indexesToKeep.Add(firstIndex);
            indexesToKeep.Add(lastIndex);

            // The first and the last point cannot be the same
            while (points[firstIndex].Equals(points[lastIndex]))
            {
                lastIndex--;
            }

            Reduce(points, firstIndex, lastIndex, tolerance, ref indexesToKeep);

            int l = indexesToKeep.Count;
            Point[] returnPoints = new Point[l];
            indexesToKeep.Sort();

            unsafe
            {
                fixed (Point* ptr = points, result = returnPoints)
                {
                    for (int i = 0; i < l; ++i)
                        *(result + i) = *(ptr + indexesToKeep[i]);
                }
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstIndex">The first point's index.</param>
        /// <param name="lastIndex">The last point's index.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="indexesToKeep">The points' index to keep.</param>
        private static void Reduce(Point[] points, int firstIndex, int lastIndex, double tolerance,
            ref List<int> indexesToKeep)
        {
            double maxDistance = 0;
            int indexFarthest = 0;

            unsafe
            {
                fixed (Point* samples = points)
                {
                    Point point1 = *(samples + firstIndex);
                    Point point2 = *(samples + lastIndex);
                    double distXY = point1.X * point2.Y - point2.X * point1.Y;
                    double distX = point2.X - point1.X;
                    double distY = point1.Y - point2.Y;
                    double bottom = distX * distX + distY * distY;

                    for (int i = firstIndex; i < lastIndex; i++)
                    {
                        // Perpendicular Distance
                        Point point = *(samples + i);
                        double area = distXY + distX * point.Y + distY * point.X;
                        double distance = (area / bottom) * area;

                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            indexFarthest = i;
                        }
                    }
                }
            }

            if (maxDistance > tolerance) // && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                indexesToKeep.Add(indexFarthest);
                Reduce(points, firstIndex, indexFarthest, tolerance, ref indexesToKeep);
                Reduce(points, indexFarthest, lastIndex, tolerance, ref indexesToKeep);
            }
        }
    }
}
