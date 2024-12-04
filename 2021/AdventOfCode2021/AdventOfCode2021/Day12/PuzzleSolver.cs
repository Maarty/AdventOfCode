using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day12
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "12";

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
                ("10", SolvePuzzle(await LoadTestInputAsync(1))),
                ("19", SolvePuzzle(await LoadTestInputAsync(2))),
                ("226", SolvePuzzle(await LoadTestInputAsync(3))),
                ("36", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("103", SolvePuzzleExtended(await LoadTestInputAsync(2))),
                ("3509", SolvePuzzleExtended(await LoadTestInputAsync(3))),
            };
        }

        private string SolvePuzzle(string input, bool canSmallBeVisitedTwice)
        {
            var lines = GetLinesInput(input).OrderBy(a => a).ToList();
            var split = lines.Select(a => a.Split('-')).ToList();
            var allNodes = split
                .Select(a => a[0])
                .Union(split.Select(a => a[1]))
                .Distinct()
                .Select(x => new Node(x, char.IsUpper(x[0]), new List<Node>()))
                .ToList();

            foreach (var node in allNodes)
            {
                node.PossiblePaths = split
                    .Where(a => a[0] == node.Name || a[1] == node.Name)
                    .Select(
                    x => x[0] == node.Name ? allNodes.First(n => n.Name == x[1]) : allNodes.First(n => n.Name == x[0]))
                    .ToList();
            }

            var start = allNodes.First(a => a.Name == "start");
            var end = allNodes.First(a => a.Name == "end");

            var possiblePaths = new List<List<Node>>();

            Traverse(start, new List<Node>(), false);

            return possiblePaths.Count.ToString();

            void Traverse(Node startNode, List<Node> currentPath, bool smallAlreadyVisitedTwice)
            {
                if (!startNode.IsBig && currentPath.Contains(startNode) && (!canSmallBeVisitedTwice || smallAlreadyVisitedTwice))
                {
                    return;
                }

                if(!smallAlreadyVisitedTwice && !startNode.IsBig && currentPath.Contains(startNode) && canSmallBeVisitedTwice)
                {
                    smallAlreadyVisitedTwice = true;
                }

                currentPath.Add(startNode);

                if (startNode == end)
                {
                    possiblePaths.Add(currentPath);
                    return;
                }

                foreach (var node in startNode.PossiblePaths.Where(a => a.Name != "start"))
                {
                    Traverse(node, new List<Node>(currentPath), smallAlreadyVisitedTwice);
                }
            }
        }

        private class Node
        {
            public Node(string name, bool isBig, List<Node> possiblePaths)
            {
                Name = name;
                IsBig = isBig;
                PossiblePaths = possiblePaths;
            }

            public string Name { get; }
            public bool IsBig { get; }
            public List<Node> PossiblePaths { get; set; }
        }
    }
}