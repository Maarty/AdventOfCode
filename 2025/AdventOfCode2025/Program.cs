using AdventOfCode2025;

const string dayNumber = "04";

var puzzleSolverFactory = new PuzzleSolverFactory();

var solver = puzzleSolverFactory.CreatePuzzleSolver(dayNumber);

var tests = await solver.SolveTestsAsync();
var i = 1;

foreach (var (expected, actual) in tests)
{
    Console.WriteLine($"Test {i} result: {actual} . Expected: {expected}");
    i++;
}

Console.WriteLine();

var result = await solver.SolvePuzzleAsync();

Console.WriteLine($"Puzzle result: {result.Result} . Time elapsed: {result.TimeElapsed}");

var resultExtended = await solver.SolvePuzzleExtendedAsync();

Console.WriteLine($"Puzzle extended result: {resultExtended.Result} . Time elapsed: {resultExtended.TimeElapsed}");