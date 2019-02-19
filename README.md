# RamerDouglasPeuckerNet Core
Ramer-Douglas-Peucker algorithm for 2D data in C#.

Given a curve composed of line segments (Polyline), find a similar curve with fewer points.

https://en.wikipedia.org/wiki/Ramer%E2%80%93Douglas%E2%80%93Peucker_algorithm

From Wikipedia:
The Ramer–Douglas–Peucker algorithm, also known as the Douglas–Peucker algorithm and iterative end-point fit algorithm, is an algorithm that decimates a curve composed of line segments to a similar curve with fewer points. 

The purpose of the algorithm is, given a curve composed of line segments (which is also called a Polyline in some contexts), to find a similar curve with fewer points. The algorithm defines 'dissimilar' based on the maximum distance between the original curve and the simplified curve (i.e., the Hausdorff distance between the curves). The simplified curve consists of a subset of the points that defined the original curve. 

## Optimisations
- Do not use Sqrt function
- Use unsafe code
- Avoid duplicate computations in loop

## Sources
https://www.codeproject.com/Articles/18936/A-C-Implementation-of-Douglas-Peucker-Line-Approxi

https://codereview.stackexchange.com/questions/29002/ramer-douglas-peucker-algorithm
