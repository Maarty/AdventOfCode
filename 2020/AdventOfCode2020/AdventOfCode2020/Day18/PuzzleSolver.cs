using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day18
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "18";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("463", SolvePuzzle(await LoadTestInputAsync(1))),
                ("13632", SolvePuzzle(await LoadTestInputAsync(2))),
                ("97", SolvePuzzleExtended(await LoadTestInputAsync(3))),
                ("1445", SolvePuzzleExtended(await LoadTestInputAsync(4))),
                ("669060", SolvePuzzleExtended(await LoadTestInputAsync(5))),
                ("23340", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        private string SolvePuzzle(string input, bool additionPriority)
        {
            return GetLinesInput(input).Select(a => a.Replace(" ", "")).Select(x => Calculate(x, additionPriority)).Sum().ToString();
        }

        private static long Calculate(string line, bool additionPriority)
        {
            var bracesClose = Regex.Matches(line, @"\)").Select(a => a.Index).ToArray();
            var bracesOpen = Regex.Matches(line, @"\(").Select(a => a.Index).ToArray();

            var lastOperator = 'x';
            long numberToMultiply = 0;
            long sum = 0;

            for (var i = 0; i < line.Length; i++)
            {
                var token = line[i];

                if (long.TryParse(token.ToString(), out var n))
                {
                    sum = ProcessNumber(n);
                    continue;
                }

                switch (token)
                {
                    case '+':
                    case '*':
                        lastOperator = token;
                        continue;
                    case '(':
                    {
                        var closingBrace = 0;
                        var x = 0;

                        foreach (var index in bracesClose)
                        {
                            x++;
                            var preceding = bracesOpen.Count(a => a < index && a != i) + 1;

                            if (preceding != x) continue;
                            closingBrace = index;
                            break;
                        }
                        
                        sum = ProcessNumber(Calculate(line[(i + 1)..(closingBrace)], additionPriority));

                        i = closingBrace;
                        break;
                    }
                }
            }

            if (numberToMultiply != 0 && additionPriority) sum *= numberToMultiply;

            return sum;

            long ProcessNumber(long n)
            {
                switch (lastOperator)
                {
                    case '+' when numberToMultiply != 0 && additionPriority:
                        numberToMultiply += n;
                        break;
                    case '+':
                    case 'x':
                        sum += n;
                        break;
                    case '*':
                    {

                        if (additionPriority)
                        {
                            if (numberToMultiply != 0)
                            {
                                sum *= numberToMultiply;
                            }

                            numberToMultiply = n;

                            break;
                        }

                        sum *= n;

                        break;
                    }
                }

                return sum;
            }
        }
    }
}