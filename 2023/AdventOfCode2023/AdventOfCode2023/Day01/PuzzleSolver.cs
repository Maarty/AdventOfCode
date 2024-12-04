using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            var numbers = GetNumbers(input);

            return numbers.Sum().ToString();
        }

        private List<int> GetNumbers(string input)
        {
            var mapping = new Dictionary<string, string>
            {
                { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" }, { "five", "5" },
                { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" }
            };

            var numbers = new List<int>();
            foreach (var line in GetLinesInput(input))
            {
                var foundNumbers = mapping.Where(a => line.Contains(a.Key)).ToList();
                var mappings = new List<(int, string)>();
                foreach (var m in foundNumbers.ToList())
                {
                    var allIndexes = AllIndexesOf(line, m.Key).ToList();
                    mappings.AddRange(allIndexes.Select(a => (a, m.Value)));
                }
                var digit = line.Select(a => a.ToString()).Where(c => int.TryParse(c.ToString(), out _));
                foreach (var m in digit)
                {
                    var allIndexes = AllIndexesOf(line, m).ToList();
                    mappings.AddRange(allIndexes.Select(a => (a, m)));
                }

                var first = mappings.MinBy(a => a.Item1).Item2;
                var last = mappings.MaxBy(a => a.Item1).Item2;

                numbers.Add(int.Parse($"{first}{last}"));
            }

            return numbers;
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var numbers = GetNumbers(input);

            return string.Empty;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("142", SolvePuzzle(await LoadTestInputAsync(1))),
                //("45000", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private static IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }
    }
}