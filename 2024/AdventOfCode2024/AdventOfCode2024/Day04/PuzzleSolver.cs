using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day04
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "04";

        protected override string SolvePuzzle(string input)
        {
            var result = 0;

            var matrix = LoadMatrix<char>(input);

            var directions = new Point[]
            {
                new(1, 0),// Right
                new(1, 1),// Down-right (diagonal)
                new(0, 1), // Down
                new(-1, 1), // Down-left (diagonal)
                new(-1, 0), // Left
                new(-1, -1), //Up-left (diagonal)
                new(0, -1), // Up
                new(1, -1), // Up-right (diagonal)
            };

            IterateMatrix(matrix,
                (x, y) =>
            {
                foreach (var direction in directions)
                {
                    if (matrix[x,y] != 'X')
                    {
                        continue;
                    }
                    var points = new Point(x, y).GetPointsToDirection(direction, matrix, 3);
                    if (points.Count == 3 &&
                        matrix.GetValue(points[0]) == 'M' &&
                        matrix.GetValue(points[1]) == 'A' &&
                        matrix.GetValue(points[2]) == 'S')
                    {
                        result++;
                    }
                }
            });

            return result.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var result = 0;

            var matrix = LoadMatrix<char>(input);

            var directions = new Point[]
            {
                new(1, 1),// Down-right (diagonal)
                new(-1, 1), // Down-left (diagonal)
                new(-1, -1), //Up-left (diagonal)
                new(1, -1), // Up-right (diagonal)
            };

            IterateMatrix(matrix,
                (x, y) =>
                {
                    if (matrix[x,y] == 'A')
                    {
                        var downRight = new Point(x, y) + directions[0];
                        var downLeft = new Point(x, y) + directions[1];
                        var upLeft = new Point(x, y) + directions[2];
                        var upRight = new Point(x, y) + directions[3];

                        if (downRight.IsInsideMatrix(matrix) && downLeft.IsInsideMatrix(matrix) && upLeft.IsInsideMatrix(matrix) && upRight.IsInsideMatrix(matrix))
                        {
                            var first = $"{matrix.GetValue(downLeft)}A{matrix.GetValue(upRight)}";
                            var second = $"{matrix.GetValue(upLeft)}A{matrix.GetValue(downRight)}";
                            if (first is "MAS" or "SAM" && second is "MAS" or "SAM")
                            {
                                result++;
                            }
                        }
                    }
                });

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("18", SolvePuzzle(await LoadTestInputAsync(1))),
                ("9", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}