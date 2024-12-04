using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2020.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 2);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 3);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("514579", SolvePuzzle(await LoadTestInputAsync(1), 2)),
                ("241861950", SolvePuzzle(await LoadTestInputAsync(1), 3))
            };
        }

        private string SolvePuzzle(string input, int numberOfEntries)
        {
            return (GetNumbersInput(input)).Subsets(numberOfEntries).First(x => x.Sum() == 2020).Aggregate((x, y) => x * y).ToString();
        }
    }
}