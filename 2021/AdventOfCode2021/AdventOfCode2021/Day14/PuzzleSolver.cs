using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day14
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "14";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 10);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 40);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("1588", SolvePuzzle(await LoadTestInputAsync(1))),
               ("2188189693529", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, int steps)
        {
            var lines = GetLinesInput(input);
            var polymer = lines[0];

            var pairCount = new Dictionary<string, long>();
            var mappings = new Dictionary<string, string>();
            var charCounts = new Dictionary<char, long>();

            foreach (var mappingsLine in lines[2..])
            {
                var split = mappingsLine.Split(" ");
                mappings[split[0]] = split[2];
            }

            for (var i = 0; i < polymer.Length - 1; i++)
            {
                charCounts[polymer[i]] = charCounts.GetValueOrDefault(polymer[i]) + 1;
                var pair = $"{polymer[i]}{polymer[i + 1]}";
                pairCount[pair] = pairCount.GetValueOrDefault(pair) + 1;
            }

            charCounts[polymer[^1]] = charCounts.GetValueOrDefault(polymer[^1]) + 1;

            for (var i = 0; i < steps; i++)
            {
                var change = new Dictionary<string, long>();

                foreach ((var pair, var count) in pairCount)
                {
                    change[pair] = change.GetValueOrDefault(pair) - count;
                    var newPair1 = $"{pair[0]}{mappings[pair]}";
                    var newPair2 = $"{mappings[pair]}{pair[1]}";

                    change[newPair1] = change.GetValueOrDefault(newPair1) + count;
                    change[newPair2] = change.GetValueOrDefault(newPair2) + count;

                    charCounts[mappings[pair][0]] = charCounts.GetValueOrDefault(mappings[pair][0]) + count;
                }

                foreach ((var pair, var count) in change)
                {
                    pairCount[pair] = pairCount.GetValueOrDefault(pair) + count;
                }
            }

            var sorted = charCounts.Select(a => a.Value).OrderBy(a => a).ToList();

            return (sorted.Last() - sorted.First()).ToString();
        }
    }
}