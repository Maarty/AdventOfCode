using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day04
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "04";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("2", SolvePuzzle(await LoadTestInputAsync(1))),
                ("4", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, a => a.Count == 8 || a.Count == 7 && !a.ContainsKey("cid"));
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, ValidateExtended);
        }


        private string SolvePuzzle(string input, Func<Dictionary<string, string>, bool> validator)
        {
            var batches = input.Split(new[] { Environment.NewLine + Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            var passports = batches.Select(ParsePassport);

            return passports.Count(validator).ToString();
        }

        private bool ValidateExtended(Dictionary<string, string> passport)
        {
            foreach (var values in passport)
            {
                switch (values.Key)
                {
                    case "byr":
                        if (!ValidateNumber(values.Value, 1920, 2002))
                        {
                            return false;
                        }

                        break;
                    case "iyr":
                        if (!ValidateNumber(values.Value, 2010, 2020))
                        {
                            return false;
                        }

                        break;
                    case "eyr":
                        if (!ValidateNumber(values.Value, 2020, 2030))
                        {
                            return false;
                        }

                        break;

                    case "hgt":
                        if (values.Value.EndsWith("cm"))
                        {
                            if (!ValidateNumber(values.Value.Replace("cm", ""), 150, 193))
                            {
                                return false;
                            }
                        }
                        else if (values.Value.EndsWith("in"))
                        {
                            if (!ValidateNumber(values.Value.Replace("in", ""), 59, 76))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    case "hcl":
                        if (!Regex.IsMatch(values.Value, "^#([A-Fa-f0-9]{6})$"))
                        {
                            return false;
                        }

                        break;
                    case "ecl":
                        if (!new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(values.Value))
                        {
                            return false;
                        }
                        break;
                    case "pid":
                        if (!Regex.IsMatch(values.Value, "^\\d{9}$"))
                        {
                            return false;
                        }
                        break;
                }
            }

            static bool ValidateNumber(string value, int low, int high)
            {
                if (!int.TryParse(value, out var year))
                {
                    return false;
                }

                return year >= low && year <= high;
            }

            return passport.Count == 8 || passport.Count == 7 && !passport.ContainsKey("cid");
        }

        private Dictionary<string, string> ParsePassport(string input)
        {
            var fields = input.Split(
                new[] { Environment.NewLine, " " },
                StringSplitOptions.RemoveEmptyEntries);

            var passport = new Dictionary<string, string>();

            foreach (var field in fields)
            {
                var fieldValue = field.Split(':');
                passport[fieldValue[0]] = fieldValue[1];
            }

            return passport;
        }
    }
}
