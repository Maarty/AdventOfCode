using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day03
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "03";

        protected override string SolvePuzzle(string input)
        {
            var rucksacks = GetLinesInput(input).Select(a => new Rucksack(a));

            return rucksacks.Sum(a => GetItemPriority(a.GetRepeatingItem())).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var rucksacks = GetLinesInput(input).Select(a => new Rucksack(a)).ToList();

            var result = 0;
            for (var i = 0; i < rucksacks.Count; i += 3)
            {
                result += GetItemPriority(GetBadgeItem(rucksacks.Skip(i).Take(3).ToList()));
            }

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("157", SolvePuzzle(await LoadTestInputAsync(1))),
                ("70", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private sealed class Rucksack
        {
            private readonly List<char> _firstCompartment;

            private readonly List<char> _secondCompartment;
            public Rucksack(string contents)
            {
                _firstCompartment = contents[..(contents.Length / 2 )].ToList();
                _secondCompartment = contents[(contents.Length / 2)..].ToList();
                Contents = contents.ToList();
            }

            public List<char> Contents { get; }

            public char? GetRepeatingItem()
            {
                return _firstCompartment.FirstOrDefault(a => _secondCompartment.Contains(a));
            }
        }

        private int GetItemPriority(char? item)
        {
            if (item == null)
            {
                return 0;
            }
            var value = (int)item;

            return value < 97 ? value - 38 : value - 96;
        }

        private char GetBadgeItem(List<Rucksack> ruckacks)
        {
            var otherSacks = ruckacks.ToArray()[1..];
            foreach (var item in ruckacks.ElementAt(0).Contents)
            {
                if (otherSacks.All(s => s.Contents.Contains(item)))
                {
                    return item;
                }
            }

            throw new InvalidOperationException("meh");
        }
    }
}