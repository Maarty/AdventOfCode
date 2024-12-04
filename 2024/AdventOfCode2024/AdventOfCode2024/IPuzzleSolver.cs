using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    public interface IPuzzleSolver
    {
        string Day { get; }
        ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync();
        Task<PuzzleResult> SolvePuzzleAsync();
        Task<PuzzleResult> SolvePuzzleExtendedAsync();
    }
}