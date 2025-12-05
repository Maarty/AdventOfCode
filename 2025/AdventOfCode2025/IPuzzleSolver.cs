namespace AdventOfCode2025;

public interface IPuzzleSolver
{
    string Day { get; }
    ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync();
    Task<PuzzleResult> SolvePuzzleAsync();
    Task<PuzzleResult> SolvePuzzleExtendedAsync();
}