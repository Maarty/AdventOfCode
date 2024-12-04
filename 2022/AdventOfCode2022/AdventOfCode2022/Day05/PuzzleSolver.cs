using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2022.Day05
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "05";

        protected override string SolvePuzzle(string input)
        {
            var (crates, instructions) = GetCratesAndInstructions(input);

            foreach (var instruction in instructions)
            {
                for (var i = 0; i < instruction.Move; i++)
                {
                    var toMove = crates[instruction.From-1].Pop();
                    crates[instruction.To-1].Push(toMove);
                }
            }

            var result = string.Empty;

            for (int i = 0; i < crates.Count; i++)
            {
                result += crates[i].Peek();
            }

            return result;
        }

        protected override string SolvePuzzleExtended(string input)
        {

            var (crates, instructions) = GetCratesAndInstructions(input);

            foreach (var instruction in instructions)
            {
                var temporary = new Stack<string>();

                for (var i = 0; i < instruction.Move; i++)
                {
                    var toMove = crates[instruction.From-1].Pop();
                    temporary.Push(toMove);
                }

                for (var i = 0; i < instruction.Move; i++)
                {
                    var toMove = temporary.Pop();
                    crates[instruction.To-1].Push(toMove);
                }
            }

            var result = string.Empty;

            for (var i = 0; i < crates.Count; i++)
            {
                result += crates[i].Peek();
            }

            return result;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("CMZ", SolvePuzzle(await LoadTestInputAsync(1))),
                ("MCD", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private (Dictionary<int, Stack<string>>, List<Instruction>) GetCratesAndInstructions(string input)
        {
            var s = input.Split(Environment.NewLine + Environment.NewLine);
            var crates = new Dictionary<int, Stack<string>>();
            var cratesLines = GetLinesInput(s[0]);
            var nOfC = int.Parse(cratesLines.Last().Split().Last());
            for (var i = nOfC - 1; i >= 0; i--)
            {
                crates[i] = new Stack<string>();
                for (var j = cratesLines.Length - 1; j >=0 ; j--)
                {
                    if (cratesLines[j].Length > i*4+1 && !string.IsNullOrWhiteSpace(cratesLines[j][i*4+1].ToString()))
                    {
                        crates[i].Push(cratesLines[j][i*4+1].ToString());
                    }
                }
            }

            var instructions = GetInstructions(s[1]);

            return (crates, instructions);
        }

        private List<Instruction> GetInstructions(string input)
        {
            var result = new List<Instruction>();
            var instructions = GetLinesInput(input).Select(a => a[5..]);

            foreach (var instruction in instructions)
            {
                var trimmed = instruction[5..];
                var x = trimmed.Split(" from ");
                var move = int.Parse(x[0]);
                var y = x[1].Split(" to ");
                var from = int.Parse(y[0]);
                var to = int.Parse(y[1]);

                result.Add(new Instruction(move, from, to));
            }

            return result;
        }

        public record Instruction(int Move, int From, int To);
    }
}