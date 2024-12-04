using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Helpers
{
    public record Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }

    public static class PointExtensions
    {
        public static bool IsInsideMatrix<T>(this Point point, T[,] matrix)
        {
            return point.Y >= 0 && point.X >= 0 && point.Y < matrix.GetLength(0) && point.X < matrix.GetLength(1);
        }

        public static List<Point> GetAdjacentPoints<T>(this Point point, Point[] directions, T[,] matrix)
        {
            return directions
                .Select(direction => point + direction)
                .Where(p => p.IsInsideMatrix(matrix))
                .ToList();
        }
    }
}