using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day12
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "12";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("25", SolvePuzzle(await LoadTestInputAsync(1))),
                ("286", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var instructions = GetLinesInput(input);
            var position = new[] { 0, 0 };
            var directionWE = 1;
            var directionSN = 0;

            foreach (var instr in instructions)
            {
                var ins = instr[0];
                var val = int.Parse(instr[1..]);

                switch (ins)
                {
                    case 'F':
                        position[0] += directionSN * val;
                        position[1] += directionWE * val;
                        break;
                    case 'N':
                        position[0] += -val;
                        break;
                    case 'S':
                        position[0] += val;
                        break;
                    case 'W':
                        position[1] += -val;
                        break;
                    case 'E':
                        position[1] += val;
                        break;
                    case 'R':
                    case 'L':
                        for (var i = 1; i <= val / 90; i++)
                        {
                            var originalWe = directionWE;
                            directionWE = (ins == 'R' ? -1 : 1) * directionSN;
                            directionSN = (ins == 'R' ? 1 : -1) * originalWe;
                        }

                        break;
                }
            }

            return (Math.Abs(position[0]) + Math.Abs(position[1])).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var instructions = GetLinesInput(input);
            var position = new[] { -1, 10 };
            var shipPosition = new[] { 0, 0 };
            
            foreach (var instr in instructions)
            {
                var ins = instr[0];
                var val = int.Parse(instr[1..]);

                switch (ins)
                {
                    case 'F':
                        var moveSn = (position[0]-shipPosition[0]) * val;
                        var moveEw = (position[1]-shipPosition[1]) * val;
                        shipPosition[0] += moveSn;
                        shipPosition[1] += moveEw;
                        position[0] += moveSn;
                        position[1] += moveEw;
                        break;
                    case 'N':
                        position[0] += -val;
                        break;
                    case 'S':
                        position[0] += val;
                        break;
                    case 'W':
                        position[1] += -val;
                        break;
                    case 'E':
                        position[1] += val;
                        break;
                    case 'R':
                    case 'L':
                        for (var i = 1; i <= val / 90; i++)
                        {
                            var we = position[1];
                            position[1] = (ins == 'R' ? -1 : 1)*(position[0] - shipPosition[0]) + shipPosition[1];
                            position[0] = (ins == 'R' ? 1 : -1) * (we - shipPosition[1]) + shipPosition[0];
                        }

                        break;
                }
            }

            return (Math.Abs(shipPosition[0]) + Math.Abs(shipPosition[1])).ToString();
        }
    }
}
