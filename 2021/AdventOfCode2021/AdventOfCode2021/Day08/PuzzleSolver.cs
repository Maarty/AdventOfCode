using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day08
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "08";

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);

            var outputs = lines.Select(a => a.Split("|")[1]);

            var total = 0;

            var nums = new[] { 2, 3, 7, 4 };

            foreach (var output in outputs)
            {
                var split = output.Split(" ", StringSplitOptions.RemoveEmptyEntries).Where(a => nums.Contains(a.Length));

                total += split.Count();
            }

            return total.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("26", SolvePuzzle(await LoadTestInputAsync(1))),
                ("61229", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("5353", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        private string SolvePuzzle(string input, bool sth)
        {
            var lines = GetLinesInput(input);

            long total = 0;

            foreach (var line in lines)
            {
                var split = line.Split("|");
                var outputs = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var numbas = line.Split(new []{ ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);

                var all = new Dictionary<string, char[]>();

                var eight = numbas.First(a => a.Length == 7); //8
                all["8"] = eight.OrderBy(x => x).ToArray();

                var one = numbas.First(a => a.Length == 2);
                all["1"] = one.OrderBy(x => x).ToArray();

                var n6 = numbas.Where(a => a.Length == 6).ToList(); //0, 6, 9

                var cf = one.OrderBy(x => x).ToArray();

                var six = n6.First(a => !cf.All(a.Contains) && cf.Any(a.Contains));
                all["6"] = six.OrderBy(x => x).ToArray();

                var c = cf.First(x => !six.Contains(x));
                var f = cf.First(x => six.Contains(x));

                var n5 = numbas.Where(a => a.Length == 5).ToList(); // 2, 3, 5

                var five = n5.First(x => x.Contains(f) && !x.Contains(c));
                all["5"] = five.OrderBy(x => x).ToArray();

                var e = eight.ToArray().First(x => !five.Contains(x) && x != c);

                var two = n5.First(x => x.Contains(e) && x.Contains(c));
                all["2"] = two.OrderBy(x => x).ToArray();

                var b = eight.ToArray().First(x => !two.Contains(x) && x != f);

                var zero = n6.First(a => a.Contains(c) && a.Contains(f) && a.Contains(b) && a.Contains(e));
                all["0"] = zero.OrderBy(x => x).ToArray();

                var d = eight.ToArray().First(x => !zero.Contains(x));

                var seven = numbas.First(a => a.Length == 3);
                all["7"] = seven.OrderBy(x => x).ToArray();

                var a = seven.First(x => x != c && x != f);
                var g = eight.First(x => x != a && x != b && x != c && x != d && x != e && x != f);

                var three = n5.First(
                    x => x.Contains(a) && x.Contains(c) && x.Contains(d) && x.Contains(f) && x.Contains(g));
                all["3"] = three.OrderBy(x => x).ToArray();

                var four = numbas.First(x => x.Length == 4);
                all["4"] = four.OrderBy(x => x).ToArray();

                var nine = n6.First(x => !x.Contains(e));
                all["9"] = nine.OrderBy(x => x).ToArray();

                var digits = string.Empty;
                var allList = all.ToList();

                foreach (var output in outputs)
                {
                    var tr = new string(output.OrderBy(p => p).ToArray());
                    var hit = allList.First(x => new string(x.Value).Equals(tr));

                    digits += hit.Key;
                }

                total += long.Parse(digits);
            }


            return total.ToString();
        }
    }
}