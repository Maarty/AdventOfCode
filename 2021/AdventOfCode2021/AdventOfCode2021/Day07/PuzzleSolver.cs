using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2021.Day07
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "07";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var lines = input.Split(",").Select(int.Parse).ToArray();

            double avg = (double)(lines.Sum()-1) / (lines.Length);

            var average = (int)Math.Round(avg);

            double result = 0;

            var dict = new Dictionary<int, double>();

            foreach (var line in lines)
            {
                var diff =Math.Abs(average - line);

                if (diff == 0)
                {
                    continue;
                }
                if (!dict.TryGetValue(diff, out var path))
                {
                    path = Enumerable.Range(1, diff).Sum();
                    dict[diff] = path;
                }

                result += path;
            }

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("37", SolvePuzzle(await LoadTestInputAsync(1))),
                ("168", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool med)
        {
            var lines = input.Split(",").Select(int.Parse).OrderBy(a => a).ToArray();

            var median = lines[lines.Length / 2];

            var result = 0;

            foreach (var line in lines)
            {
                result += Math.Abs(median - line);
            }

            return result.ToString();
        }
    }
}