using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day20
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "20";

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
                ("900", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("5", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool sth)
        {
            var lines = GetLinesInput(input);

            return string.Empty;
        }
    }
}