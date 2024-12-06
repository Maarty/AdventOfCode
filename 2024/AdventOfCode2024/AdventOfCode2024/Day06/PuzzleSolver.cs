using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day06
{
    public enum Orientation { Right, Down, Left, Up };

    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "06";

        protected override string SolvePuzzle(string input)
        {
            var result = 0;

            var matrix = LoadMatrix<char>(input);

            var (orientation, positionX, positionY) = GetGuard(matrix);
            var position = new Point(positionX, positionY);

            while (true)
            {
                if (matrix[position.X, position.Y] == '.')
                {
                    matrix[position.X, position.Y] = 'X';
                    result++;
                }
                if (position.IsOnTheEdgeOfMatrix(matrix))
                {
                    break;
                }

                var newPosition = position + GetDirection(orientation);
                if (matrix[newPosition.X, newPosition.Y] == '#')
                {
                    orientation = Rotate(orientation);
                }
                else
                {
                    position = newPosition;
                }
            }

            return (result + 1).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var result = 0;

            var matrix = LoadMatrix<char>(input);

            var (orientation, positionX, positionY) = GetGuard(matrix);
            var position = new Point(positionX, positionY);
            var originalPosition = position;
            var originalOrientation = orientation;

            IterateMatrix(matrix,
                (x, y) =>
                {
                    var visited = new Dictionary<int, Dictionary<int, HashSet<Orientation>>>();
                    var originalChar = matrix[x, y];
                    if (originalChar == '#') return;
                    matrix[x, y] = '#';
                    position = originalPosition;
                    orientation = originalOrientation;

                    while (true)
                    {
                        if (visited.TryGetValue(position.Y, out var yy) && yy.TryGetValue(position.X, out var oo) && oo.Contains(orientation))
                        {
                            result += 1;
                            break;
                        }

                        AddVisited();

                        if (position.IsOnTheEdgeOfMatrix(matrix))
                        {
                            break;
                        }

                        var newPosition = position + GetDirection(orientation);
                        if (matrix[newPosition.X, newPosition.Y] == '#')
                        {
                            orientation = Rotate(orientation);
                        }
                        else
                        {
                            position = newPosition;
                        }
                    }

                    void AddVisited()
                    {
                        if (!visited.ContainsKey(position.Y))
                        {
                            visited[position.Y] = new Dictionary<int, HashSet<Orientation>>();
                        }

                        if (!visited[position.Y].ContainsKey(position.X))
                        {
                            visited[position.Y][position.X] = new HashSet<Orientation>();

                        }
                        visited[position.Y][position.X].Add(orientation);
                    }
                });

            return result.ToString();
        }

        private Point GetDirection(Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Right => new Point(1, 0),
                Orientation.Up => new Point(0, -1),
                Orientation.Left => new Point(-1, 0),
                Orientation.Down => new Point(0, 1),
            };
        }

        private Orientation Rotate(Orientation orientation)
        {

            return orientation switch
            {
                Orientation.Right => Orientation.Down,
                Orientation.Up => Orientation.Right,
                Orientation.Left => Orientation.Up,
                Orientation.Down => Orientation.Left,
            };
        }

        private (Orientation, int, int) GetGuard(char[,] matrix)
        {
            var positionX = -1;
            var positionY = -1;
            var orientation = Orientation.Right;
            IterateMatrix(matrix,
                (x, y) =>
                {
                    if (positionX != -1 && positionY != -1) return;
                    switch (matrix[x,y])
                    {
                        case '^':
                        case '>':
                        case '<':
                        case 'v':
                            positionX = x;
                            positionY = y;
                            switch (matrix[x,y])
                            {
                                case '^':
                                    orientation = Orientation.Up;
                                    break;
                                case '>':
                                    orientation = Orientation.Right;
                                    break;
                                case '<':
                                    orientation = Orientation.Down;
                                    break;
                                case 'v':
                                    orientation = Orientation.Left;
                                    break;
                            }
                            break;

                    }
                });

            return (orientation, positionX, positionY);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("41", SolvePuzzle(await LoadTestInputAsync(1))),
                ("6", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}