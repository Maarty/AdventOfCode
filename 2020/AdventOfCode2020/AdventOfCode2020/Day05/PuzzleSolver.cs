using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day05
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "05";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("567", SolvePuzzle(await LoadTestInputAsync(1))),
                ("820", SolvePuzzle(await LoadTestInputAsync(2)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return GetSeatIds(input).Max().ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var seatIds = GetSeatIds(input);

            return Enumerable.Range(seatIds.Min(), seatIds.Max()).Except(seatIds).First().ToString();
        }

        private List<int> GetSeatIds(string input)
        {
            return GetLinesInput(input).Select(ParseBoardingPass).Select(a => a.row * 8 + a.column).ToList();
        }

        private (int row, int column) ParseBoardingPass(string pass)
        {
            var row = pass.Substring(0, 7).Replace("F","0").Replace("B","1");
            var column = pass.Substring(7, 3).Replace("L","0").Replace("R","1");

            return (Convert.ToInt32(row, 2), Convert.ToInt32(column, 2));
        }
    }
}
