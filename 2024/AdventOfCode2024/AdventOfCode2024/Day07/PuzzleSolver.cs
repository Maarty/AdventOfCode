using PermutationLibrary;

namespace AdventOfCode2024.Day07
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "07";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input, ['+', '*']);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return Solve(input, ['+', '*', '|']);
        }

        private string Solve(string input, char[] operators)
        {
            long result = 0;

            var lines = GetLinesInput(input);

            foreach (var line in lines)
            {
                var split = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                var testValue = long.Parse(split[0]);
                var numbers = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                var operatorsPossibilities = new Permutor<char>(numbers.Count - 1, operators, true).PermuteToList();

                foreach (var operatorsPossibility in operatorsPossibilities)
                {
                    var testResult = numbers[0];
                    for (var i = 0; i < operatorsPossibility.Length; i++)
                    {
                        switch (operatorsPossibility[i])
                        {
                            case '+':
                                testResult += numbers[i + 1];
                                break;
                            case '*':
                                testResult *= numbers[i + 1];
                                break;
                            case '|':
                                testResult = long.Parse($"{testResult}{numbers[i + 1]}");
                                break;
                        }
                    }

                    if (testResult != testValue) continue;
                    result += testResult;
                    break;
                }

            }
            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("3749", SolvePuzzle(await LoadTestInputAsync(1))),
                ("11387", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            ];
        }
    }
}