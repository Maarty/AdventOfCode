using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day08
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "08";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("5", SolvePuzzle(await LoadTestInputAsync(1))),
                ("8", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return RunProgram(GetLinesInput(input).Select(ParseInstruction).ToArray()).Acc.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var instructions = GetLinesInput(input).Select(ParseInstruction).ToArray();

            var firstRun = RunProgram(instructions).instructionsOrder.ToList();
            var lastIndex = firstRun.Count + 1;
            (string Instr, int Position) lastChange;
            UpdateLastChange();

            while (true)
            {
                instructions = GetLinesInput(input).Select(ParseInstruction).ToArray();
                instructions[lastChange.Position].Instr = instructions[lastChange.Position].Instr == "nop" ? "jmp" : "nop";

                var (_, lastInstr, acc) = RunProgram(instructions);

                if (lastInstr != instructions.Length)
                {
                    UpdateLastChange();
                }
                else
                {
                    return acc.ToString();
                }
            }

            void UpdateLastChange()
            {
                lastChange = firstRun.Last(a => (a.Instr == "jmp" || a.Instr == "nop") && firstRun.IndexOf(a) < lastIndex);
                lastIndex = firstRun.IndexOf(lastChange);
            }
        }

        private static ((string Instr, int Position)[] instructionsOrder, int LastInstr, int Acc) RunProgram(
            (string Instr, int Value, bool Visited)[] instructions)
        {
            var acc = 0;
            var i = 0;
            var instructionsOrder = new List<(string, int)>();

            while (true)
            {
                if (i == instructions.Length) break;

                instructionsOrder.Add((instructions[i].Instr, i));

                if (instructions[i].Visited) return (instructionsOrder.ToArray(), i, acc);

                instructions[i].Visited = true;

                switch (instructions[i].Instr)
                {
                    case "acc":
                        acc += instructions[i].Value;
                        break;
                    case "jmp":
                        i += instructions[i].Value;
                        continue;
                }

                i++;
            }

            return (instructionsOrder.ToArray(), i, acc);
        }

        private static (string Instr, int Value, bool Visited) ParseInstruction(string line)
        {
            var split = line.Split(" ");
            return (split[0], int.Parse(split[1]), false);
        }
    }
}
