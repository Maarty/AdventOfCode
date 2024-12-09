using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day08
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "08";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return Solve(input, true);
        }

        private string Solve(string input, bool extended = false)
        {
            var matrix = LoadMatrix<char>(input);

            var antiNodes = new HashSet<Point>();

            IterateMatrix(matrix,
                (x, y) =>
                {
                    if (IsAntenna(x, y))
                    {
                        if (extended)
                        {
                            antiNodes.Add(new Point(x, y));
                        }

                        IterateMatrix(matrix,
                            (xx, yy) =>
                            {
                                if (matrix[x, y] == matrix[xx, yy] && (x != xx || y != yy))
                                {
                                    var currentAntenna = new Point(x, y);
                                    var secondAntenna = new Point(xx, yy);
                                    var antiNodeDirection = (currentAntenna - secondAntenna) * -1;
                                    while (true)
                                    {
                                        var antiNode = currentAntenna - antiNodeDirection;
                                        if (!antiNode.IsInsideMatrix(matrix))
                                        {
                                            return;
                                        }

                                        antiNodes.Add(antiNode);

                                        if (!extended)
                                        {
                                            break;
                                        }

                                        currentAntenna = antiNode;
                                    }
                                }
                            });
                    }

                    bool IsAntenna(int i1, int i2)
                    {
                        return matrix[i1, i2] != '#' && matrix[i1, i2] != '.';
                    }
                });

            return antiNodes.Count.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("14", SolvePuzzle(await LoadTestInputAsync(1))),
                ("34", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            ];
        }
    }
}