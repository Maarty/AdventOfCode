using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Day05
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "05";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, true);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected string SolvePuzzle(string input, bool countOkay)
        {
            var result = 0;

            var splitInput = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var rules = new Dictionary<string, HashSet<string>>();

            foreach (var rule in splitInput[0].Split(Environment.NewLine))
            {
                var rls = rule.Split('|');
                if (!rules.ContainsKey(rls[0]))
                {
                    rules[rls[0]] = [];
                }
                rules[rls[0]].Add(rls[1]);
            }

            foreach (var update in splitInput[1].Split(Environment.NewLine))
            {
                var fail = false;
                var pages = update.Split(',');
                Array.Sort(pages, ComparePages);
                var blah = (string.Join(",", pages));
                if (blah != update)
                {
                    fail = true;
                }

                if (fail == countOkay) continue;

                result += int.Parse(pages[pages.Length/2]);
            }

            return result.ToString();

            int ComparePages(string x, string y)
            {
                if (rules.TryGetValue(x, out var first))
                {
                    if (first.Contains(y))
                    {
                        return -1;
                    }
                }

                if (!rules.TryGetValue(y, out var second)) return 0;
                return second.Contains(x) ? 1 : 0;
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("143", SolvePuzzle(await LoadTestInputAsync(1))),
                ("123", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}