using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            var numbers = GetNumbersInput(input).ToList();
            return SolvePuzzle(numbers);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var numbers = GetNumbersInput(input);

            var sums = Enumerable.Range(2, numbers.Length - 2)
                .Select(a => numbers[a - 2] + numbers[a - 1] + numbers[a])
                .ToList();

            return SolvePuzzle(sums);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("7", SolvePuzzle(await LoadTestInputAsync(1))),
                ("5", SolvePuzzle(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(List<long> numbers)
        {
            return Enumerable.Range(1, numbers.Count - 1).Count(a => numbers[a - 1] < numbers[a]).ToString();
        }
    }
}