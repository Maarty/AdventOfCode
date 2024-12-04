using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day07
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "07";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("4", SolvePuzzle(await LoadTestInputAsync(1))),
                ("32", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("126", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var bags = GetBags(input);

            return bags.Count(a => BagContains(bags, a.Key, "shinygold")).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return (CountNumberOfInnerBags(GetBags(input), "shinygold") - 1).ToString();
        }

        private ImmutableDictionary<string, List<(int Number, string Bag)>> GetBags(string input)
        {
            return GetLinesInput(input).Select(GetBag).ToImmutableDictionary(t => t.Name, t => t.Contents);
        }

        private (string Name, List<(int Number, string Bag)> Contents) GetBag(string line)
        {
            var split = line.Split(" bags contain ");

            var bags = split[1].Split(',').Where(a => a != "no other bags.")
                .Select(a => a.Replace(" ", "").Replace("bags", "").Replace("bag", "").Replace(".",""))
                .Select(a => (int.Parse(a[0].ToString()), a[1..])).ToList();

            return (split[0].Replace(" ", ""), bags);
        }

        public int CountNumberOfInnerBags(ImmutableDictionary<string, List<(int Number, string Bag)>> allBags, string searchedBag)
        {
            return 1 + allBags[searchedBag].Sum(bag => bag.Number * CountNumberOfInnerBags(allBags, bag.Bag));
        }

        public bool BagContains (ImmutableDictionary<string, List<(int Number, string Bag)>> allBags, string searchedBag, string bagToFind)
        {
            if (!allBags.ContainsKey(searchedBag))
            {
                return false;
            }

            var contentBags = allBags[searchedBag];

            return contentBags.Any(a => a.Bag == bagToFind) || contentBags.Any(bag => BagContains(allBags, bag.Bag, bagToFind));
        }
    }
}
