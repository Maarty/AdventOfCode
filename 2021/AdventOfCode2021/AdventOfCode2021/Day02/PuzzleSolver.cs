using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);
            var depth = 0;
            var horiz = 0;
            var aim = 0;

            foreach (var line in lines)
            {
                var parsed = line.Split(' ');
                var value = int.Parse(parsed[1]);

                switch (parsed[0])
                {
                    case "forward":
                        horiz += value;
                        depth += value * aim;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }

            return (depth*horiz).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {

            return String.Empty;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("900", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("5", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle()
        {

            return string.Empty;
        }
    }
}