using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2021.Helpers;

namespace AdventOfCode2021.Day09
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "09";

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
                ("15", SolvePuzzle(await LoadTestInputAsync(1))),
                ("1134", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool calculateBasins)
        {
            var directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0),
            };

            var lines = GetLinesInput(input);

            var matrix = new int[lines.First().Length, lines.Length];

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    matrix[x, y] = (int)char.GetNumericValue(lines[y][x]);
                }
            }

            var risk = 0;
            var lowPoints = new List<Point>();

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var point = new Point(x, y);
                    var adjacentHeights = point.GetAdjacentPoints(directions, matrix).Select(a => matrix[a.X, a.Y]).ToList();

                    if (!adjacentHeights.All(h => h > matrix[x, y])) continue;

                    risk += matrix[x, y] + 1;
                    lowPoints.Add(point);
                }
            }

            if(!calculateBasins) return risk.ToString();

            var basinsSizes = new List<int>();

            foreach (var lowPoint in lowPoints)
            {
                var basins = new List<Point>();
                EvaluatePoint(lowPoint, basins);

                basinsSizes.Add(basins.Distinct().Count());
            }

            return basinsSizes.OrderByDescending(a => a).Take(3).Aggregate((x, y) => x * y).ToString();


            void EvaluatePoint(Point point, List<Point> points)
            {
                points.Add(point);

                foreach (var adjacentPoint in point.GetAdjacentPoints(directions, matrix))
                {
                    var adjacentHeight = matrix[adjacentPoint.X, adjacentPoint.Y];

                    if (adjacentHeight != 9 && adjacentHeight > matrix[point.X,point.Y])
                    {
                        EvaluatePoint(adjacentPoint, points);
                    }
                }
            }
        }
    }
}