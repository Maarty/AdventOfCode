using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2021.Helpers;

namespace AdventOfCode2021.Day11
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "11";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("1656", SolvePuzzle(await LoadTestInputAsync(2))),
                ("195", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        private string SolvePuzzle(string input, bool calcSync)
        {
            var directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0),
                new(-1, -1),
                new(-1, 1),
                new(1, 1),
                new(1, -1),
            };

            var lines = GetLinesInput(input);
            var matrix = new int[lines[0].Length, lines.Length];

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[0].Length; x++)
                {
                    matrix[x, y] = int.Parse(lines[y][x].ToString());
                }
            }

            var totalFlash = 0;

            for (var step = 0; step < (calcSync ? 10000 : 100); step++)
            {
                var flashedThisRound = 0;
                var flashes = false;

                IterateMatrix(matrix, (x, y) =>
                    {
                        matrix[x, y]++;
                        if (matrix[x, y] > 9)
                        {
                            flashes = true;
                        }
                    });

                while(flashes)
                {
                    flashes = false;

                    IterateMatrix(matrix, (x, y) =>
                    {
                        if (matrix[x, y] <= 9) return;

                        flashes = true;
                        matrix[x, y] = 0;
                        totalFlash++;
                        flashedThisRound++;
                        var point = new Point(x, y);
                        foreach (var toIncrease in point.GetAdjacentPoints(directions, matrix)
                                     .Where(toIncrease => matrix[toIncrease.X, toIncrease.Y] > 0))
                        {
                            matrix[toIncrease.X, toIncrease.Y]++;
                        }
                    });
                }

                if (flashedThisRound == matrix.Length && calcSync)
                {
                    return (step+1).ToString();
                }
            }

            return totalFlash.ToString();
        }
    }
}