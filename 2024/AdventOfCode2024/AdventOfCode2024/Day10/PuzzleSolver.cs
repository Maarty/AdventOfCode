using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day10
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        private readonly Point[] _directions =
        [
            new(1, 0),// Right
            new(0, 1), // Down
            new(-1, 0), // Left
            new(0, -1) // Up
        ];

        public override string Day => "10";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return Solve(input, true);
        }

        private string Solve(string input, bool uniqueTrails = false)
        {
            long result = 0;

            var lines = GetLinesInput(input);
            var matrix = new int[lines[0].Length, lines.Length];

            IterateMatrix(matrix, (x, y) => matrix[x, y] = lines[y][x] == '.' ? -1 : (int)Convert.ChangeType(lines[y][x].ToString(), typeof(int)));

            IterateMatrix(
                matrix,
                (x, y) =>
                {
                    if (matrix[x, y] != 0) return;

                    var foundPaths = 0;
                    var foundTrails = new HashSet<string>();

                    FindTrails(new Point(x, y), matrix, ref foundPaths, foundTrails);

                    result += uniqueTrails ? foundPaths : foundTrails.Count;
                });

            return result.ToString();
        }

        private void FindTrails(Point trailHead, int[,] matrix, ref int foundPaths, HashSet<string> foundTrails)
        {
            foreach (var direction in _directions)
            {
                var newPoint = trailHead + direction;
                if (!newPoint.IsInsideMatrix(matrix))
                {
                    continue;
                }

                if (matrix.GetValue(newPoint) != matrix.GetValue(trailHead) + 1) continue;

                if (matrix.GetValue(newPoint) == 9)
                {
                    foundPaths++;
                    foundTrails.Add($"{newPoint.X}-{newPoint.Y}");
                    continue;
                }

                FindTrails(newPoint, matrix, ref foundPaths, foundTrails);
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("1", SolvePuzzle(await LoadTestInputAsync(1))),
                ("2", SolvePuzzle(await LoadTestInputAsync(2))),
                ("4", SolvePuzzle(await LoadTestInputAsync(3))),
                ("3", SolvePuzzle(await LoadTestInputAsync(4))),
                ("36", SolvePuzzle(await LoadTestInputAsync(5))),
                ("81", SolvePuzzleExtended(await LoadTestInputAsync(5)))
            ];
        }
    }
}