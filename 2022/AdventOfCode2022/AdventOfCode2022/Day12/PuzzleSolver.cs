using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day12
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "12";

        protected override string SolvePuzzle(string input)
        {
            var (_, start, end) = LoadMatrix(input);

            ShortestPath(start, end);

            return end.DistanceFromStart.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var (matrix, _, end) = LoadMatrix(input);

            var starts = matrix.Cast<Node>().Where(a => a.Height == 'a');

            var shortest = int.MaxValue;

            foreach (var st in starts)
            {
                ShortestPath(st, end);
                if (end.DistanceFromStart < shortest && end.DistanceFromStart > 0)
                {
                    shortest = end.DistanceFromStart;
                }
                IterateMatrix(matrix,
                    (x, y) =>
                    {
                        matrix[x, y].DistanceFromStart = 0;
                        matrix[x, y].Visited = false;
                        matrix[x, y].ClosestToStart = null;
                    });
            }

            return shortest.ToString();
        }

        private (Node[,] Matrix, Node Start, Node End) LoadMatrix(string input)
        {
            Node start = null;
            Node end = null;
            var lines = GetLinesInput(input);
            var directions = new Point[]
            {
                new(0, 1),
                new(0, -1),
                new(1, 0),
                new(-1, 0),
            };

            var lengthY = lines.Length;
            var lengthX = lines[0].Length;

            var matrix = new Node[lengthX, lengthY];

            IterateMatrix(
                matrix,
                (x, y) =>
                {
                    var height = lines[y][x];
                    var node = new Node { Height = height, Coordinates = new Point(x, y) };
                    switch (height)
                    {
                        case 'S':
                            node.Height = 'a';
                            start = node;
                            break;
                        case 'E':
                            node.Height = 'z';
                            end = node;
                            break;
                    }

                    matrix[x, y] = node;
                });

            IterateMatrix(
                matrix,
                (x, y) =>
                {
                    var adjacent = matrix[x, y].Coordinates.GetAdjacentPoints(directions, matrix);
                    matrix[x, y].Adjacent = adjacent
                        .Where(m => matrix[m.X, m.Y].Height - 1 <= matrix[x, y].Height)
                        .Select(a => matrix[a.X, a.Y]).ToList();
                });
            return (matrix, start, end);
        }

        private static void ShortestPath(Node start, Node end)
        {
            var toExamine = new PriorityQueue<Node, int>();
            toExamine.Enqueue(start, 0);

            while (toExamine.TryDequeue(out var node, out _))
            {
                if (node.Visited) continue;

                foreach (var adjacent in node.Adjacent.Where(adjacent => adjacent.DistanceFromStart == 0 || node.DistanceFromStart + adjacent.DistanceFromStart < adjacent.DistanceFromStart))
                {
                    adjacent.DistanceFromStart = node.DistanceFromStart + 1;
                    adjacent.ClosestToStart = node;

                    toExamine.Enqueue(adjacent, adjacent.DistanceFromStart);
                }

                node.Visited = true;
                if (node == end) return;
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("31", SolvePuzzle(await LoadTestInputAsync(1))),
                ("29", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        private class Node
        {
            public Point Coordinates { get; set; }
            public List<Node> Adjacent { get; set; }
            public int DistanceFromStart { get; set; }
            public char Height { get; set; }
            public bool Visited { get; set; }
            public Node ClosestToStart { get; set; }
        }
    }
}