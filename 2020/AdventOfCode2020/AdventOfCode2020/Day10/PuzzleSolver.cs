using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day10
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "10";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("35", SolvePuzzle(await LoadTestInputAsync(1))),
                ("220", SolvePuzzle(await LoadTestInputAsync(2))),
                ("8", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("19208", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var numbers = GetNumbersInput(input).ToList();
            numbers.Add(0);
            numbers.Add(numbers.Max() + 3);
            var adapters = numbers.OrderBy(a => a).ToArray();
            long actualOutlet = 0;
            var jolts1 = 0;
            var jolts3 = 0;

            for (var i = 0; i < adapters.Length; i++)
            {
                if (i == adapters.Length - 1)
                {
                    continue;
                }

                if (adapters[i] != actualOutlet)
                {
                    continue;
                }

                var diff = adapters[i + 1] - adapters[i];

                if (diff < 1 || diff > 3) continue;

                actualOutlet = adapters[i + 1];
                switch (diff)
                {
                    case 1:
                        jolts1++;
                        break;
                    case 3:
                        jolts3++;
                        break;
                }
            }

            return (jolts1 * jolts3).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var numbers = GetNumbersInput(input).ToList();
            numbers.Add(0);
            numbers.Add(numbers.Max() + 3);
            var adapters = numbers.OrderBy(a => a).ToArray();

            var paths = new Dictionary<long, long> { { adapters.Max() + 3, 1 } };

            for (var i = adapters.Length - 1; i > -1; i--)
            {
                long num = 0;
                if (paths.ContainsKey(adapters[i] + 1))
                {
                    num += paths[adapters[i] + 1];
                }
                if (paths.ContainsKey(adapters[i] + 2))
                {
                    num += paths[adapters[i] + 2];
                }
                if (paths.ContainsKey(adapters[i] + 3))
                {
                    num += paths[adapters[i] + 3];
                }

                paths[adapters[i]] = num;
            }

            return paths[adapters[0]].ToString();
        }
    }
}
