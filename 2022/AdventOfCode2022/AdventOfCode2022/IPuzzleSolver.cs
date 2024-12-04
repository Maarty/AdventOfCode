using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public interface IPuzzleSolver
    {
        string Day { get; }
        ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync();
        Task<PuzzleResult> SolvePuzzleAsync();
        Task<PuzzleResult> SolvePuzzleExtendedAsync();
    }
}