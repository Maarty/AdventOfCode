using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("2", SolvePuzzle(await LoadTestInputAsync(1))),
                ("1", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, VerifyPasswordExtended);
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, VerifyPassword);
        }

        private string[][] ParseLines(string input)
        {
            var lines = GetLinesInput(input);

            return lines.Select(line => line.Split(':', ' ', '-')).ToArray();
        }

        private string SolvePuzzle(string input, Func<int, int, char, string, bool> evaluator)
        {
            var lines = ParseLines(input);

            var result = 0;

            foreach (var parsedLine in lines)
            {
                if (evaluator(int.Parse(parsedLine[0]), int.Parse(parsedLine[1]), char.Parse(parsedLine[2]), parsedLine[4]))
                {
                    result++;
                }
            }

            return result.ToString();

        }

        private bool VerifyPassword(int min, int max, char letter, string password)
        {
            var count = password.Count(s => s == letter);
            return  count >= min && count <= max;
        }

        private bool VerifyPasswordExtended(int position1, int position2, char letter, string password)
        {
            return password[position1 - 1] == letter && password[position2 - 1] != letter ||
                   password[position2 - 1] == letter && password[position1 - 1] != letter;
        }
    }
}
