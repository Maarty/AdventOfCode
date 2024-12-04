using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022.Helpers
{
    public record Point(int X, int Y) : IEqualityComparer<Point>
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public bool Equals(Point x, Point y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(Point obj)
        {
            return HashCode.Combine(obj.X, obj.Y);
        }
    }

    public static class PointExtensions
    {
        public static bool IsInsideMatrix<T>(this Point point, T[,] matrix)
        {
            return point.Y >= 0 && point.X >= 0 && point.Y < matrix.GetLength(1) && point.X < matrix.GetLength(0);
        }

        public static bool IsOnTheEdgeOfMatrix<T>(this Point point, T[,] matrix)
        {
            return point.Y == 0 || point.X == 0 || point.Y == matrix.GetLength(0)-1 || point.X == matrix.GetLength(1)-1;
        }

        public static List<Point> GetAdjacentPoints<T>(this Point point, Point[] directions, T[,] matrix)
        {
            return directions
                .Select(direction => point + direction)
                .Where(p => p.IsInsideMatrix(matrix))
                .ToList();
        }

        public static List<Point> GetPointsToDirection<T>(this Point point, Point direction, T[,] matrix)
        {
            var allDirections = new List<Point>();
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                var multiplier = direction * new Point(i, i);
                allDirections.Add(direction + multiplier);
            }

            return allDirections
                .Select(d => point + d)
                .Where(p => p.IsInsideMatrix(matrix))
                .ToList();
        }
    }
}