using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day06
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "06";

        protected override string SolvePuzzle(string input)
        {
            var lastFour = new Queue<char>();
            for (var i = 0; i < input.Length; i++)
            {
                lastFour.Enqueue(input[i]);

                if (lastFour.Count != 14) continue;
                if (lastFour.All(a => lastFour.Count(x => x == a) == 1))
                {
                    return (i + 1).ToString();
                }

                lastFour.Dequeue();
            }

            return "xxx";
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return string.Empty;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("7", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("MCD", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}