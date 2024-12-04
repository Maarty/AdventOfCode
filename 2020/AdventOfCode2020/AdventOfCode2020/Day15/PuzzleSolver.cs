using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day15
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "15";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("436", SolvePuzzle(await LoadTestInputAsync(1))),
                ("175594", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 2020);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 30000000);
        }

        private string SolvePuzzle(string input, int turn)
        {
            var numbas = input.Split(',').Select(int.Parse).ToArray();

            var lastNumba = 0;
            var mem = new Dictionary<int, (int, int)>();

            for (var i = 1; i < turn + 1; i++)
            {
                if (i < numbas.Length + 1)
                {
                    lastNumba = numbas[i - 1];
                    mem[lastNumba] = (0, i);
                }
                else
                {
                    mem.TryGetValue(lastNumba, out var order);
                    lastNumba = order.Equals(default) ? 0 : order.Item1;
                    mem[lastNumba] = mem.ContainsKey(lastNumba) ? (i - mem[lastNumba].Item2, i) : (0, i);
                }
            }

            return lastNumba.ToString();
        }
    }
}