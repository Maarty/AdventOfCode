namespace AdventOfCode2024.DayXX
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return Solve(input);
        }

        private string Solve(string input)
        {
            long result = 0;

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("3749", SolvePuzzle(await LoadTestInputAsync(1))),
                //("11387", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            ];
        }
    }
}