using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day19
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "19";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("2", SolvePuzzle(await LoadTestInputAsync(1))),
                ("12", SolvePuzzleExtended(await LoadTestInputAsync(2))),
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

        private string SolvePuzzle(string input, bool changeRules)
        {
            var split = input.Split(Environment.NewLine + Environment.NewLine);

            var rules = GetLinesInput(split[0]);
            var words = GetLinesInput(split[1]).ToList();

            var referenceMap = new Dictionary<string, List<List<string>>>();
            var referenceRules = new Dictionary<string, List<string>>();
            var finalRules = new Dictionary<string, string>();
            var subMatches = new Dictionary<string, bool>();

            foreach (var rule in rules)
            {
                var s = rule.Split(": ");

                if (s[1].Contains("\""))
                {
                    finalRules.Add(s[0], s[1][1].ToString());
                }
                else
                {
                    if (changeRules && s[0] == "8")
                    {
                        s[1] = "42 | 42 8";
                    }

                    if (changeRules && s[0] == "11")
                    {
                        s[1] = "42 31 | 42 11 31";
                    }

                    var refs = s[1].Split(" | ").Select(@ref => @ref.Split(" ").ToList()).ToList();
                    referenceMap.Add(s[0], refs);
                }
            }

            var maxLoops = 5; // brute force like a boss
            var maxWord = words.Max(a => a.Length);

            var possibilities = GetPossibilities("0", 0);

            return words.Count(a => possibilities.Any(x => x == a)).ToString();

            List<string> GetPossibilities(string rule, int depth)
            {
                if (referenceRules.TryGetValue(rule, out var r))
                {
                    return r;
                }

                if (finalRules.ContainsKey(rule))
                {
                    return new List<string> { finalRules[rule] };
                }

                var ors = referenceMap[rule];

                var combs = new List<List<string>>();

                foreach (var ands in ors)
                {
                    var innerCombs = new List<List<string>>();

                    foreach (var and in ands)
                    {
                        if (and == rule)
                        {
                            depth++;
                            if (depth > maxLoops)
                            {
                                break;
                            }
                        }
                        
                        innerCombs.Add(GetPossibilities(and, depth));
                    }

                    if (depth > maxLoops)
                    {
                        return new List<string>();
                    }

                    combs.Add(GetAllPossibleCombos(innerCombs));
                }

                List<string> GetAllPossibleCombos(List<List<string>> strings)
                {
                    IEnumerable<string> combos = new[] { "" };

                    var j = 0;

                    foreach (var s in strings)
                    {
                        j++;
                        combos = combos.SelectMany(c => s, (c, i) => {
                            if ((c+i).Length > maxWord)
                            {
                                return string.Empty;
                            }
                            if (j == strings.Count)
                            {
                                if (!subMatches.ContainsKey(c + i))
                                {
                                    subMatches[c + i] = words.Any(x => x.Contains(c + i));
                                }

                                if (!subMatches[c + i])
                                {
                                    return string.Empty;
                                }
                            }

                            return c + i; }).Where(a => !string.IsNullOrWhiteSpace(a));
                    }

                    return combos.ToList();
                }

                referenceRules[rule] = combs.SelectMany(a => a).ToList();

                return referenceRules[rule];
            }
        }
    }
}