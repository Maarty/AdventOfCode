using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AdventOfCode2021.Helpers;

namespace AdventOfCode2021.Day15
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "15";

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
                ("40", SolvePuzzle(await LoadTestInputAsync(1))),
                ("315", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool extendedMap)
        {
            var lines = GetLinesInput(input);
            var directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0),
            };

            var length = lines.Length;
            var matrixLength = extendedMap ? length * 5 : length;

            var matrix = new Node[matrixLength, matrixLength];

            IterateMatrix(matrix,
                (x, y) =>
                {
                    var initialX = x % length;
                    var initialY = y % length;
                    var toIncrease = x / length + y / length;

                    var cost = int.Parse(lines[initialY][initialX].ToString());
                    if (extendedMap)
                    {
                        cost += toIncrease;
                        if (cost > 9)
                        {
                            cost -= 9;
                        }
                    }

                    matrix[x, y] = new Node{ Risk = cost, Coordinates = new Point(x, y)};
                });

            IterateMatrix(matrix,
                (x, y) =>
                {
                    matrix[x, y].Adjacent = matrix[x, y].Coordinates.GetAdjacentPoints(directions, matrix).Select(a => matrix[a.X, a.Y]).ToList();
                });


            var end = matrix[matrixLength - 1, matrixLength - 1];

            EvaluateRisks(matrix[0,0], end);
            return (end.RiskFromStart).ToString();
        }

        private void EvaluateRisks(Node start, Node end)
        {
            var toExamine = new PriorityQueue<Node, int>();
            toExamine.Enqueue(start, 0);

            while (toExamine.TryDequeue(out var node, out _))
            {
                if (node.Visited) continue;

                foreach (var adjacent in node.Adjacent.OrderBy(x => x.Risk))
                {
                    if (adjacent.RiskFromStart != 0 && node.RiskFromStart + adjacent.Risk >= adjacent.RiskFromStart) continue;

                    adjacent.RiskFromStart = node.RiskFromStart + adjacent.Risk;
                    adjacent.ClosestToStart = node;

                    toExamine.Enqueue(adjacent, adjacent.RiskFromStart);
                }

                node.Visited = true;
                if (node == end) return;
            }
        }

        private class Node
        {
            public Point Coordinates { get; set; }
            public List<Node> Adjacent { get; set; }
            public int RiskFromStart { get; set; }
            public int Risk { get; set; }
            public bool Visited { get; set; }
            public Node ClosestToStart { get; set; }
        }
    }
}