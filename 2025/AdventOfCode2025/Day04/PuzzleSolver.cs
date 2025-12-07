using AdventOfCode2025.Helpers;

namespace AdventOfCode2025.Day04;

public class PuzzleSolver : PuzzleSolverBase
{
    public override string Day => "04";

    protected override string SolvePuzzle(string input)
    {
        return Solve(input);
    }

    protected override string SolvePuzzleExtended(string input)
    {
        long result = 0;

        var matrix = LoadMatrix<char>(input);
        var removed = 0;

        do
        {
            removed = 0;
            IterateMatrix(
                matrix,
                (x, y) =>
                {
                    if ((matrix[x, y] == '@' || matrix[x, y] == 'x') && 
                        new Point(x, y).GetAdjacentPoints(PointHelpers.AllDirections, matrix).Count(a => matrix[a.X, a.Y] == '@' || matrix[a.X, a.Y] == 'x') < 4)
                    {
                        matrix[x, y] = 'x';
                    }
                });
            
            IterateMatrix(matrix,
                (x, y) =>
                {
                    if (matrix[x, y] == 'x')
                    {
                        matrix[x, y] = '.';
                        removed++;
                    }
                });
            
            result += removed;
            
        } while (removed > 0);


        return result.ToString();
    }

    private string Solve(string input)
    {
        long result = 0;

        var matrix = LoadMatrix<char>(input);

        IterateMatrix(
            matrix,
            (x, y) =>
            {
                result += (matrix[x, y] == '@' && new Point(x, y).GetAdjacentPoints(PointHelpers.AllDirections, matrix).Count(a => matrix[a.X, a.Y] == '@') < 4) ? 1 : 0;
            });

        return result.ToString();
    }

    public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
    {
        return
        [
            ("13", SolvePuzzle(await LoadTestInputAsync(1))),
            ("43", SolvePuzzleExtended(await LoadTestInputAsync(1)))
        ];
    }
}