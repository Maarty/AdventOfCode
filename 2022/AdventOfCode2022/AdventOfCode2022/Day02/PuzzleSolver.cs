using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        protected override string SolvePuzzle(string input)
        {
            var strategies = GetLinesInput(input).Select(a => a.Split(" ")).Select(a => new Strategy(a[0][0], a[1][0]));
            return strategies.Sum(a => a.TotalScore).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var strategies = GetLinesInput(input).Select(a => a.Split(" ")).Select(a => new Strategy(a[0][0], GetChoice(a[0][0], a[1][0])));
            return strategies.Sum(a => a.TotalScore).ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("15", SolvePuzzle(await LoadTestInputAsync(1))),
                ("12", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private char GetChoice(char elf, char Choice)
        {
            switch (elf)
            {
                case 'A' when Choice == 'X':
                    return 'Z';
                case 'A' when Choice == 'Y':
                    return 'X';
                case 'A' when Choice == 'Z':
                    return 'Y';
                case 'B' when Choice == 'X':
                    return 'X';
                case 'B' when Choice == 'Y':
                    return 'Y';
                case 'B' when Choice == 'Z':
                    return 'Z';
                case 'C' when Choice == 'X':
                    return 'Y';
                case 'C' when Choice == 'Y':
                    return 'Z';
                case 'C' when Choice == 'Z':
                    return 'X';
                default:
                    return 'f';
            }
        }

        public record Strategy(char Elf, char You)
        {
            public int TotalScore => MatchScore + ChoiceScore;

            public int MatchScore
            {
                get
                {
                    switch (Elf)
                    {
                        case 'A' when You == 'X':
                            return 3;
                        case 'A' when You == 'Y':
                            return 6;
                        case 'A' when You == 'Z':
                            return 0;
                        case 'B' when You == 'X':
                            return 0;
                        case 'B' when You == 'Y':
                            return 3;
                        case 'B' when You == 'Z':
                            return 6;
                        case 'C' when You == 'X':
                            return 6;
                        case 'C' when You == 'Y':
                            return 0;
                        case 'C' when You == 'Z':
                            return 3;
                        default:
                            return 10000;
                    }
                }
            }

            public int ChoiceScore
            {
                get
                {
                    switch (You)
                    {
                        case 'X':
                            return 1;
                        case 'Y':
                            return 2;
                        case 'Z':
                            return 3;
                        default:
                            return 10000;
                    }
                }
            }
        }
    }
}