using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2019.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            return (GetNumbersInput(input)).Select(a => a/3-2).Sum().ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var modules = GetNumbersInput(input);

            var totalFuel = 0L;

            foreach (var module in modules)
            {
                var remainingFuel = module;
                var neededFuel = 0L;
                while (true)
                {
                    remainingFuel = remainingFuel / 3 - 2;

                    if (remainingFuel < 1)
                    {
                        break;
                    }

                    neededFuel += remainingFuel;
                }

                totalFuel += neededFuel;
            }

            return totalFuel.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                ("34241", SolvePuzzle(await LoadTestInputAsync(1))),
                ("51316", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}