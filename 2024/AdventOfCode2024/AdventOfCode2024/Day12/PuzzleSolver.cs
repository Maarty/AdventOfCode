using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day12
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "12";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return Solve(input);
        }

        private string Solve(string input)
        {
            long result = 0;

            var directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0),
            };

            var gardens = new List<HashSet<Point>>();
            var matrix = LoadMatrix<Plant>(input, c => new Plant(c));

            IterateMatrix(
                matrix,
                (x, y) =>
                {
                    if (matrix[x, y].Assigned) return;

                    var garden = new HashSet<Point>();
                    EvaluateGarden(new Point(x, y), garden);
                    gardens.Add(garden);
                });

            foreach (var garden in gardens)
            {
                result += garden.Sum(point => directions.Select(direction => new Point(point.X, point.Y) + direction).Count(neighbor => !garden.Contains(neighbor))) * garden.Count;
            }

            return result.ToString();

            void EvaluateGarden(Point point, HashSet<Point> garden)
            {
                garden.Add(point);
                matrix[point.X, point.Y].Assigned = true;

                foreach (var adjacentPoint in point.GetAdjacentPoints(directions, matrix))
                {
                    if (matrix[adjacentPoint.X, adjacentPoint.Y].Type == matrix[point.X, point.Y].Type
                        && !matrix[adjacentPoint.X, adjacentPoint.Y].Assigned)
                    {
                        EvaluateGarden(adjacentPoint, garden);
                    }
                }
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("140", SolvePuzzle(await LoadTestInputAsync(1))),
                ("772", SolvePuzzle(await LoadTestInputAsync(2))),
                ("80", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("436", SolvePuzzleExtended(await LoadTestInputAsync(2))),
                ("236", SolvePuzzleExtended(await LoadTestInputAsync(3))),
                ("368", SolvePuzzleExtended(await LoadTestInputAsync(4))),
                ("1206", SolvePuzzleExtended(await LoadTestInputAsync(5))),
            ];
        }
    }

    public class Plant(string type)
    {
        public bool Assigned { get; set; }
        public string Type { get; set; } = type;
    }
}