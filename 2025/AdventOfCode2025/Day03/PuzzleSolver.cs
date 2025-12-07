namespace AdventOfCode2025.Day03;

public class PuzzleSolver : PuzzleSolverBase
{
    public override string Day => "03";

    protected override string SolvePuzzle(string input)
    {
        return Solve(input, 2);
    }

    protected override string SolvePuzzleExtended(string input)
    {
        return Solve(input, 12);
    }

    private string Solve(string input, int batteriesCount)
    {
        long result = 0;
        var lines = GetLinesInput(input);

        foreach (var line in lines)
        {
            var numbers = line.Select(a => int.Parse(a.ToString())).ToList();
            var max = numbers.Take(line.Length - batteriesCount + 1).Max();
            var batteries = new List<int> { max };
            var position = numbers.IndexOf(max) + 1;

            for (var i = batteriesCount - 2; i >= 0; i--)
            {
                max = numbers.Skip(position).Take(line.Length - position - i).Max();
                batteries.Add(max);
                position = numbers.Skip(position).ToList().IndexOf(batteries.Last()) + position + 1;
            }

            result += long.Parse(string.Join("", batteries));
        }

        return result.ToString();
    }

    public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
    {
        return
        [
            ("357", SolvePuzzle(await LoadTestInputAsync(1))),
            ("3121910778619", SolvePuzzleExtended(await LoadTestInputAsync(1)))
        ];
    }
}