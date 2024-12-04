using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day05
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "05";

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
                ("5", SolvePuzzle(await LoadTestInputAsync(1))),
                ("12", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool considerDiagonal)
        {
            var vents = GetLinesInput(input)
                .Select(line => line.Split(new[] {',',  '-', '>'}, StringSplitOptions.RemoveEmptyEntries))
                .Select(split =>
                    new Vent(int.Parse(split[0]), int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[3])))
                .ToList();

            var result = new Dictionary<string, int>();

            foreach (var vent in vents)
            {
                if (vent.x1 == vent.x2 || vent.y1 == vent.y2)
                {
                    var xEquals = vent.x1 == vent.x2;
                    var start = xEquals ? Math.Min(vent.y1, vent.y2) : Math.Min(vent.x1, vent.x2);
                    var end = xEquals ? Math.Max(vent.y1, vent.y2) : Math.Max(vent.x1, vent.x2);
                    for (var i = start; i < end + 1; i++)
                    {
                        UpdateMap(xEquals ? vent.x1 : i, xEquals ? i : vent.y1, result);
                    }
                }

                if (!considerDiagonal)
                {
                    continue;
                }
                
                if (Math.Abs(vent.x1 - vent.x2) == Math.Abs(vent.y1 - vent.y2))
                {
                    var start = Math.Min(vent.x1, vent.x2);
                    var end = Math.Max(vent.x1, vent.x2);
                    var startY = vent.x1 == start ? vent.y1 : vent.y2;
                    var endY = vent.x1 == end ? vent.y1 : vent.y2;

                    var diff = Math.Abs(vent.x1 - vent.x2);
                    var j = startY;
                    var i = start;

                    for (var k = 0; k < diff + 1; k++)
                    {
                        UpdateMap(i, j, result);

                        j = UpdateIndex(startY, endY, j);
                        i = UpdateIndex(start, end, i);
                    }
                }
            }

            return result.Count(a => a.Value > 1).ToString();
        }

        private static int UpdateIndex(int startY, int endY, int j)
        {
            if (startY > endY)
            {
                j--;
            }
            else
            {
                j++;
            }

            return j;
        }

        private static void UpdateMap(int i, int j, Dictionary<string, int> result)
        {
            var key = $"{i}+{j}";
            if (!result.ContainsKey(key))
            {
                result[key] = 0;
            }

            result[key] += 1;
        }
    }

    public record Vent(int x1, int x2, int y1, int y2);
}