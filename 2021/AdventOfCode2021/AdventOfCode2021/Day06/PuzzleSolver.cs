using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day06
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "06";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 80);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 256);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("26", SolvePuzzle(await LoadTestInputAsync(1), 18)),
                ("5934", SolvePuzzle(await LoadTestInputAsync(1), 80)),
                ("26984457539", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, int totalDays)
        {
            var allFish = input.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var fishTimers = Enumerable.Range(0, 9).ToArray();
            var fishCount = new Dictionary<int, long>();

            foreach (var fish in fishTimers) fishCount[fish] = 0;
            foreach (var fish in allFish) fishCount[fish]++;

            for (var i = 0; i < totalDays; i++)
            {
                var totalZeros = fishCount[0];
                for (var j = 1; j < 9; j++)
                {
                    fishCount[j - 1] = fishCount[j];
                }

                fishCount[6] += totalZeros;
                fishCount[8] = totalZeros;
            }

            return fishCount.Select(a => a.Value).Sum().ToString();
        }
    }
}