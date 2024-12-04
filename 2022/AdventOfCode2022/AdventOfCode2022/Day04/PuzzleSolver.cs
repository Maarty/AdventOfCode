using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day04
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "04";

        protected override string SolvePuzzle(string input)
        {
            var pairs = GetPairs(input);

            return pairs.Count(a => IsFullyContained(a.Item1, a.Item2)).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var pairs = GetPairs(input);

            return pairs.Count(a => Overlaps(a.Item1, a.Item2)).ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("2", SolvePuzzle(await LoadTestInputAsync(1))),
                ("4", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        public record Section(int Start, int End);

        private List<(Section, Section)> GetPairs(string input)
        {
            var lines = GetLinesInput(input);

            var pairs = new List<(Section, Section)>();

            foreach (var line in lines)
            {
                var ps = line.Split(',');
                var section1 = new Section(int.Parse(ps[0].Split('-')[0]), int.Parse(ps[0].Split('-')[1]));
                var section2 = new Section(int.Parse(ps[1].Split('-')[0]), int.Parse(ps[1].Split('-')[1]));
                pairs.Add((section1, section2));
            }

            return pairs;
        }

        private bool IsFullyContained(Section first, Section second)
        {
            return (first.Start >= second.Start && first.End <= second.End) ||
                   (second.Start >= first.Start && second.End <= first.End);
        }

        private bool Overlaps(Section first, Section second)
        {
            return (first.Start >= second.Start && first.Start <= second.End) ||
                   (second.Start >= first.Start && second.Start <= first.End);
        }
    }
}