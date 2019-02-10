using System.Collections.Generic;
using System.Linq;

namespace RamerDouglasPeuckerNet
{
    class RamerDouglasPeucker
    {
        /// <summary>
        /// Uses the Douglas Peucker algorithm to reduce the number of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public static Point[] Reduce(Point[] points, double tolerance)
        {
            if (points == null || points.Count() < 3) return points;
            if (double.IsInfinity(tolerance) || double.IsNaN(tolerance)) return points;
            tolerance *= tolerance;
            if (tolerance <= float.Epsilon) return points;

            int firstPoint = 0;
            int lastPoint = points.Count() - 1;
            List<int> pointIndexsToKeep = new List<int>();

            // Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            // The first and the last point cannot be the same
            while (points[firstPoint].Equals(points[lastPoint]))
            {
                lastPoint--;
            }

            Reduce(points, firstPoint, lastPoint, tolerance, ref pointIndexsToKeep);

            int l = pointIndexsToKeep.Count;
            Point[] returnPoints = new Point[l];
            pointIndexsToKeep.Sort();

            unsafe
            {
                fixed (Point* ptr = points, result = returnPoints)
                {
                    Point* res = result;
                    for (int i = 0; i < l; ++i)
                        *(res + i) = *(ptr + pointIndexsToKeep[i]);
                }
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <param name="pointIndexsToKeep">The point index to keep.</param>
        private static void Reduce(Point[] points, int firstPoint,
            int lastPoint, double tolerance, ref List<int> pointIndexsToKeep)
        {
            double maxDistance = 0;
            int indexFarthest = 0;

            unsafe
            {
                fixed (Point* samples = points)
                {
                    Point point1 = *(samples + firstPoint);
                    Point point2 = *(samples + lastPoint);
                    double distXY = point1.X * point2.Y - point2.X * point1.Y;
                    double distX = point2.X - point1.X;
                    double distY = point1.Y - point2.Y;
                    double bottom = distX * distX + distY * distY;

                    for (int index = firstPoint; index < lastPoint; index++)
                    {
                        //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
                        //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
                        //Area = .5*Base*H                                          *Solve for height
                        //Height = Area/.5/Base

                        Point point = *(samples + index);
                        double area = distXY + distX * point.Y + distY * point.X;
                        double distance = (area / bottom) * area;

                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            indexFarthest = index;
                        }
                    }
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);
                Reduce(points, firstPoint, indexFarthest, tolerance, ref pointIndexsToKeep);
                Reduce(points, indexFarthest, lastPoint, tolerance, ref pointIndexsToKeep);
            }
        }
    }
}
