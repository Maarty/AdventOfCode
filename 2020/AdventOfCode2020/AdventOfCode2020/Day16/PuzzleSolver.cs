using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day16
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "16";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("71", SolvePuzzle(await LoadTestInputAsync(1))),
                ("156", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var areas = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var r = Regex.Matches(areas[0], "[0-9]+-[0-9]+");

            var ranges = Regex.Matches(areas[0], "[0-9]+-[0-9]+").Select(a => a.Value)
                .Select(a => (int.Parse(a.Split('-')[0]), int.Parse(a.Split('-')[1]))).ToList();

            var values = areas[2][16..].Split(new []{",", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Select(a => int.Parse(a.Trim())).ToList();

            var invalid = 0;
            foreach (var val in values)
            {
                if (!ranges.Any(a => a.Item1 <= val && a.Item2 >= val))
                {
                    invalid += val;
                }
            }

            return invalid.ToString();

        }

        protected override string SolvePuzzleExtended(string input)
        {
            var areas = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            var fields = GetLinesInput(areas[0]);
            var fieldRanges = fields.Select(
                a => (a.Split(": ")[0], Regex.Matches(a.Split(": ")[1], "[0-9]+-[0-9]+").Select(b => b.Value)
                    .Select(c => (int.Parse(c.Split('-')[0]), int.Parse(c.Split('-')[1]))).ToList())).ToList();

            var tickets = GetLinesInput(areas[2])[1..].Select(a => a.Split(',').Select(int.Parse).ToArray());

            var positions = new Dictionary<int, List<string>>();

            foreach (var ticket in tickets)
            {
                var valid = true;
                foreach (var val in ticket)
                {
                    if (!fieldRanges.Any(c => c.Item2.Any(a => a.Item1 <= val && a.Item2 >= val)))
                    {
                        valid = false;
                        break;
                    }
                }

                if (!valid)
                {
                    continue;
                }

                for (int i = 0; i < ticket.Count(); i++)
                {
                    var validRanges = fieldRanges.Where(
                        a => a.Item2.Any(x => x.Item1 <= ticket[i] && x.Item2 >= ticket[i])).Select(a => a.Item1).ToList();

                    if (!positions.ContainsKey(i))
                    {
                        positions[i] = new List<string> (validRanges);
                    }
                    else
                    {
                        positions[i] = positions[i].Intersect(validRanges).ToList();
                    }
                }
            }

            while (positions.Any(x => x.Value.Count > 1))
            {
                var singles = positions.Where(a => a.Value.Count == 1).Select(a => a.Value.First()).ToList();
                var notSingles = positions.Where(a => a.Value.Count > 1).ToList();

                foreach (var pos in notSingles)
                {
                    pos.Value.RemoveAll(x => singles.Contains(x));
                }
            }

            var myTicket = areas[1].Split(Environment.NewLine)[1].Split(',').Select(int.Parse).ToArray();

            var departures = positions.Where(a => a.Value.First().StartsWith("departure")).Select(a => a.Key);

            long result = 1;
            foreach (var departure in departures)
            {
                result *= myTicket[departure];
            }

            return result.ToString();
        }
    }
}