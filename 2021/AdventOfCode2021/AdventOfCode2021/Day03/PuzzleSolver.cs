using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day03
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "03";

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);
            var bits = new int[lines.First().Length];

            foreach (var line in lines)
            {
                var i = 0;
                foreach (var bit in line)
                {
                    if (bit == '1')
                    {
                        bits[i] += 1;
                    }
                    else
                    {
                        bits[i] -= 1;
                    }
            
                    i++;
                }
            }
            
            var gamma = string.Empty;
            var delta = string.Empty;
            
            foreach (var bit in bits)
            {
                if (bit > 0)
                {
                    gamma += '1';
                    delta += '0';
                }
                else
                {
                    delta += '1';
                    gamma += '0';
                }
            }

            return (Convert.ToInt32(gamma, 2)*Convert.ToInt32(delta, 2)).ToString();;
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var co2Lines = GetLinesInput(input);
            var oxygenLines = GetLinesInput(input);

            var oxygen = GetBitsCalculation(oxygenLines, '1');
            var co2 = GetBitsCalculation(co2Lines, '0');

            return (Convert.ToInt32(oxygen, 2) * Convert.ToInt32(co2, 2)).ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("198", SolvePuzzle(await LoadTestInputAsync(1))),
                ("230", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string GetBitsCalculation(string[] lines, char searchedBit)
        {
            var length = lines.First().Length;
            for (var i = 0; i < length; i++)
            {
                var bit = GetDesiredBit(lines, i, searchedBit);
;
                lines = lines.Where(a => a[i] == bit).ToArray();

                if (lines.Length == 1)
                {
                    return lines.First();
                }
            }

            throw new Exception("Something went wrong");
        }

        private char GetDesiredBit(string[] lines, int bitNumber, char defaultBit)
        {
            var bitsByCount = lines.Select(a => a[bitNumber]).GroupBy(x => x).OrderBy(d => d.Count()).ToList();

            return bitsByCount.First().Count() == bitsByCount.Last().Count() ? defaultBit : bitsByCount.ElementAt(defaultBit == '0' ? 0 : 1).Key;
        }
    }
}