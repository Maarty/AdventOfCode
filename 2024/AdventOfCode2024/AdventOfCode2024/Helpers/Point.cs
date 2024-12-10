namespace AdventOfCode2024.Helpers
{
    public record Point(int X, int Y) : IEqualityComparer<Point>
    {
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public static Point operator *(Point a, int b)
        {
            return new Point(a.X * b, a.Y * b);
        }

        public bool Equals(Point x, Point y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Y == y.Y;
        }

        public string OrientationString()
        {
            switch (X)
            {
                case 1 when Y == 0: return "Right";
                case 1 when Y == 1: return "Down-right (diagonal)";
                case 0 when Y == 1: return "Down";
                case -1 when Y == 1: return "Down-left (diagonal)";
                case -1 when Y == 0: return "Left";
                case -1 when Y == -1: return "Up-left (diagonal)";
                case 0 when Y == -1: return "Up";
                case 1 when Y == -1: return "Up-right (diagonal)";
            }

            return string.Empty;
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

        public static List<Point> GetPointsToDirection<T>(this Point point, Point direction, T[,] matrix, int length = 0)
        {
            var allDirections = new List<Point>();
            for (var i = 0; i < (length > 0 ? length : matrix.GetLength(0)); i++)
            {
                var multiplier = direction * new Point(i, i);
                allDirections.Add(direction + multiplier);
            }

            return allDirections
                .Select(d => point + d)
                .Where(p => p.IsInsideMatrix(matrix))
                .ToList();
        }

        public static T GetValue<T>(this T[,] matrix, Point point)
        {
            return matrix[point.X, point.Y];
        }
    }
}