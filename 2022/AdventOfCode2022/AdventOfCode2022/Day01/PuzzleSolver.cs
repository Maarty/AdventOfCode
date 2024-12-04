using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            return GetElves(GetLinesInput(input).ToList()).Max(a => a.TotalCalories).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return GetElves(GetLinesInput(input).ToList()).OrderByDescending(a => a.TotalCalories).Take(3)
                .Sum(a => a.TotalCalories).ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("24000", SolvePuzzle(await LoadTestInputAsync(1))),
                ("45000", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private static List<Elf> GetElves(List<string> lines)
        {
            var elves = new List<Elf>();
            var currentCalories = new List<int>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    elves.Add(new Elf(currentCalories.ToList()));
                    currentCalories = new List<int>();
                }
                else
                {
                    currentCalories.Add(int.Parse(line));
                }
            }

            elves.Add(new Elf(currentCalories.ToList()));

            return elves;
        }

        private record Elf(List<int> Calories)
        {
            public int TotalCalories => Calories.Sum();
        }
    }
}