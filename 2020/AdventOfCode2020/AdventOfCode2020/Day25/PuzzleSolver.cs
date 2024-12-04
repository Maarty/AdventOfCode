using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day25
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "25";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                ("14897079", SolvePuzzle(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var numbers = GetNumbersInput(input);

            var cardLoop = GetLoopSize(numbers[0]);
            
            return BigInteger.ModPow(numbers[1], cardLoop, 20201227).ToString();
        }

        private long GetLoopSize(long number)
        {
            var i = 1;

            while (true)
            {
                if (BigInteger.ModPow(7, i, 20201227) == number)
                {
                    return i;
                }

                i++;
            }
        }
    }
}