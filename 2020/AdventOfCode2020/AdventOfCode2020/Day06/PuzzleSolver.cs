using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day06
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "06";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("11", SolvePuzzle(await LoadTestInputAsync(1))),
                ("6", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var groups = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            return groups.Select(a => a.Replace(Environment.NewLine, "").Distinct().Count()).Sum().ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var groups = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var count = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(@group => @group.Split(Environment.NewLine).Select(a => a.Distinct())).Select(
                    passengers => groups.Select(a => a.Replace(Environment.NewLine, "").Distinct()).SelectMany(a => a)
                        .Distinct().Count(a => passengers.All(x => x.Contains(a)))).Sum();

            return count.ToString();
        }
    }
}
