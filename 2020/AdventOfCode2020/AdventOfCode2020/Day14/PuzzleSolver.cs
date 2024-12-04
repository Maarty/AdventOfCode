using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq.Extensions;

namespace AdventOfCode2020.Day14
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "14";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("165", SolvePuzzle(await LoadTestInputAsync(1))),
                ("208", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);
            var mem = new Dictionary<int, long>();
            var mask = string.Empty;

            foreach (var instruction in lines)
            {
                if (instruction.StartsWith("mask"))
                {
                    mask = instruction[7..];
                    continue;
                }

                var split = instruction.Replace("mem[","").Replace("]","").Replace(" ","").Split('=');

                mem[int.Parse(split[0])] = ApplyMask(int.Parse(split[1]), mask);
            }

            return mem.Select(a => a.Value).Sum().ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var lines = GetLinesInput(input);
            var mem = new Dictionary<long, long>();
            var mask = string.Empty;

            foreach (var instruction in lines)
            {
                if (instruction.StartsWith("mask"))
                {
                    mask = instruction[7..];
                    continue;
                }

                var split = instruction.Replace("mem[", "").Replace("]", "").Replace(" ", "").Split('=');

                var addresses = GetPossibleAddresses(int.Parse(split[0]), mask);

                var val = int.Parse(split[1]);

                foreach (var address in addresses)
                {
                    mem[address] = val;
                }

            }

            return mem.Select(a => a.Value).Sum().ToString();
        }

        private long ApplyMask(int number, string mask)
        {
            var binary = new StringBuilder(Convert.ToString(number, 2).PadLeft(36, '0'));

            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] != 'X')
                {
                    binary[i] = mask[i];
                }
            }

            return Convert.ToInt64(binary.ToString(), 2);
        }

        private long[] GetPossibleAddresses(int number, string mask)
        {
            var binary = new StringBuilder(Convert.ToString(number, 2).PadLeft(36, '0'));

            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] != '0')
                {
                    binary[i] = mask[i];
                }
            }

            var bin = binary.ToString();

            var numberOfFloating = bin.Count(a => a == 'X');

            var chars = new List<char>();

            for (var i = 0; i < numberOfFloating; i++)
            {
                chars.Add('0');
                chars.Add('1');
            }

            var combos = chars.Subsets(numberOfFloating).Distinct();

            var addresses = new List<long>();

            foreach (var combo in combos)
            {
                var x = 0;
                var numba = new StringBuilder(binary.ToString());
                for (var i = 0; i < bin.Length; i++)
                {
                    if (bin[i] == 'X')
                    {
                        numba[i] = combo[x];
                        x++;
                    }
                }

                addresses.Add(Convert.ToInt64(numba.ToString(), 2));
            }

            return addresses.ToArray();
        }
    }
}
