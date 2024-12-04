using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day08
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "08";

        protected override string SolvePuzzle(string input)
        {
            var matrix = GetMatrix(input, out var directions);

            var visible = 0;

            IterateMatrix(matrix,
                (x, y) =>
                {
                    if (x == 0 || y == 0 || x == matrix.GetLength(0) - 1 || y == matrix.GetLength(0) - 1)
                    {
                        visible++;
                        return;
                    }

                    foreach (var direction in directions)
                    {
                        var lineOfSight = new Point(x, y).GetPointsToDirection(direction, matrix);
                        if (!lineOfSight.Any(a => matrix[a.X, a.Y] >= matrix[x,y]))
                        {
                            visible++;
                            return;
                        }
                    }

                });

            return visible.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var matrix = GetMatrix(input, out var directions);

            var scores = new int[matrix.GetLength(0), matrix.GetLength(1)];

            IterateMatrix(scores, (x, y) => scores[x,y] = 1);

            IterateMatrix(matrix,
                (x, y) =>
                {
                    var actual = matrix[x, y];

                    if (x == 0 || y == 0 || x == matrix.GetLength(0) - 1 || y == matrix.GetLength(0) - 1)
                    {
                        scores[x, y] = 0;
                        return;
                    }

                    foreach (var direction in directions)
                    {
                        var score = 0;
                        var lineOfSight = new Point(x, y).GetPointsToDirection(direction, matrix);
                        foreach (var point in lineOfSight)
                        {
                            score++;

                            if (matrix[point.X, point.Y] < actual && !point.IsOnTheEdgeOfMatrix(matrix)) continue;
                            scores[x, y] *= score > 0 ? score : 1;
                            break;
                        }
                    }
                });

            var maxScore = 0;

            IterateMatrix(
                scores,
                (x, y) =>
                {
                    if (scores[x,y] > maxScore)
                    {
                        maxScore = scores[x, y];
                    }
                });

            return maxScore.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("21", SolvePuzzle(await LoadTestInputAsync(1))),
                ("8", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private int[,] GetMatrix(string input, out Point[] directions)
        {
            var lines = GetLinesInput(input);
            var matrix = new int[lines[0].Length, lines.Length];

            IterateMatrix(matrix, (x, y) => matrix[x, y] = int.Parse(lines[y][x].ToString()));

            directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0)
            };
            return matrix;
        }
    }
}