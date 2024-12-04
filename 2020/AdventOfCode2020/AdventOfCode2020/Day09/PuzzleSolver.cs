using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2020.Day09
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "09";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("127", SolvePuzzle(await LoadTestInputAsync(1), 5)),
                ("62", SolvePuzzleExtended(await LoadTestInputAsync(1), 5)),
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 25);
        }

        private string SolvePuzzle(string input, int preamble)
        {
            var numbers = GetNumbersInput(input);
            return GetInvalidNumber(numbers, preamble).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzleExtended(input, 25);
        }

        private string SolvePuzzleExtended(string input, int preamble)
        {
            var numbers = GetNumbersInput(input);
            var invalid = GetInvalidNumber(numbers, preamble);

            for (var i = 0; i < numbers.Length; i++)
            {
                for (var j = i; j > -1; j--)
                {
                    var candidates = numbers[new Range(j, i + 1)];

                    if (candidates.Sum() == invalid)
                    {
                        return (candidates.Min() + candidates.Max()).ToString();
                    }
                }
            }

            return string.Empty;
        }

        private static long GetInvalidNumber(long[] numbers, int preamble)
        {
            for (var i = preamble; i < numbers.Length; i++)
            {
                if (numbers[new Range(i - preamble, i)].Subsets(2).All(a => a.Sum() != numbers[i]))
                {
                    return numbers[i];
                }
            }

            return 0;
        }
    }
}
