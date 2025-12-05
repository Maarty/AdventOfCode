namespace AdventOfCode2025.Day02;

public class PuzzleSolver : PuzzleSolverBase
{
    public override string Day => "02";

    protected override string SolvePuzzle(string input)
    {
        return Solve(input);
    }

    protected override string SolvePuzzleExtended(string input)
    {
        return SolveExtended(input);
    }

    private string Solve(string input)
    {
        long result = 0;
        
        var ranges = input.Split(',');

        foreach (var range in ranges)
        {
            var bounds = range.Split('-');
            var start = long.Parse(bounds[0]);
            var end = long.Parse(bounds[1]);
            
            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
                if (id.Length % 2 == 0 && id[0..(id.Length/2)] == id[(id.Length/2)..])
                {
                    result += i;
                }
            }
        }

        return result.ToString();
    }

    private string SolveExtended(string input)
    {
        long result = 0;
        
        var ranges = input.Split(',');

        foreach (var range in ranges)
        {
            var bounds = range.Split('-');
            var start = long.Parse(bounds[0]);
            var end = long.Parse(bounds[1]);
            
            for (var i = start; i <= end; i++)
            {
                var id = i.ToString();
    
                for (var repetitionSize = 1; repetitionSize <= id.Length / 2; repetitionSize++)
                {
                    if (id.Length % repetitionSize != 0) continue;
                    
                    var repetition = id.Substring(0, repetitionSize);
                    var finalNumber = string.Concat(Enumerable.Repeat(repetition, id.Length / repetitionSize));

                    if (id == finalNumber)
                    {
                        result += i;
                        break;
                    }
                }
            }
        }

        return result.ToString();
    }

    public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
    {
        return
        [
            ("1227775554", SolvePuzzle(await LoadTestInputAsync(1))),
            ("4174379265", SolvePuzzleExtended(await LoadTestInputAsync(1)))
        ];
    }
}