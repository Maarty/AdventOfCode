using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                ("3500", SolvePuzzle(await LoadTestInputAsync(1), false)),
                ("2", SolvePuzzle(await LoadTestInputAsync(2), false)),
                ("2", SolvePuzzle(await LoadTestInputAsync(3), false)),
                ("2", SolvePuzzle(await LoadTestInputAsync(4), false)),
                ("30", SolvePuzzle(await LoadTestInputAsync(5), false)),
                //("51316", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, true);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            for (int i = 0; i < 100; i++)
            {
                var instructions = input.Split(',').Select(int.Parse).ToArray();
                for (int j = 0; j < 100; j++)
                {
                    instructions[1] = i;
                    instructions[2] = j;

                    try
                    {
                        var result = RunProgram(instructions);

                        if (result == "19690720")
                        {
                            return (100 * i + j).ToString();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return string.Empty;
        }

        private string SolvePuzzle(string input, bool runProgramAlarm)
        {
            var instructions = input.Split(',').Select(int.Parse).ToArray();
            
            if (runProgramAlarm)
            {
                instructions[1] = 12;
                instructions[2] = 2;
            }

            return RunProgram(instructions);
        }

        private static string RunProgram(int[] instructions)
        {

            for (int i = 0; i < instructions.Length; i++)
            {
                var terminated = false;
                switch (instructions[i])
                {
                    case 1:
                        instructions[instructions[i + 3]] =
                            instructions[instructions[i + 1]] + instructions[instructions[i + 2]];
                        i += 3;
                        break;
                    case 2:
                        instructions[instructions[i + 3]] =
                            instructions[instructions[i + 1]] * instructions[instructions[i + 2]];
                        i += 3;
                        break;
                    case 99:
                        terminated = true;
                        break;
                }

                if (terminated)
                {
                    break;
                }
            }

            return instructions[0].ToString();
        }
    }
}