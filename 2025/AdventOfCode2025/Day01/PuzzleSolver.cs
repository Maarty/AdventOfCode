namespace AdventOfCode2025.Day01;

public class PuzzleSolver : PuzzleSolverBase
{
    public override string Day => "01";

    protected override string SolvePuzzle(string input)
    {
        return Solve(input);
    }

    protected override string SolvePuzzleExtended(string input)
    {
        return Solve(input, true);
    }

    private string Solve(string input, bool allZeros = false)
    {
        var currentPosition = 50;
        var numberOfZerosHit = 0;
        var numberOfZerosStopped = 0;
        
        var lines = GetLinesInput(input).ToList();

        foreach (var line in lines)
        {
            var direction = line[0];
            var distance = int.Parse(line[1..]);
            var normalizedDistance = distance % 100;
            numberOfZerosHit += distance / 100;
            
            switch (direction)
            {
                case 'L':
                    currentPosition -= normalizedDistance;
                    if (currentPosition < 0)
                    {
                        if (currentPosition + normalizedDistance != 0)
                        {
                            numberOfZerosHit++;
                        }
                        currentPosition = (currentPosition + 100) % 100;
                    }
                    if (currentPosition == 0)
                        numberOfZerosHit++;
                    break;
                case 'R':
                    currentPosition += normalizedDistance;
                    if (currentPosition > 100)
                    {
                        numberOfZerosHit++;
                        currentPosition %= 100;
                    }
                    if (currentPosition == 100)
                    {
                        currentPosition = 0;
                        numberOfZerosHit++;
                    }
                    break;
            }

            if (currentPosition == 0)
            {
                numberOfZerosStopped++;
            }
        }
        

        return allZeros ? numberOfZerosHit.ToString() : numberOfZerosStopped.ToString();
    }

    public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
    {
        return
        [
            ("3", SolvePuzzle(await LoadTestInputAsync(1))),
            ("6", SolvePuzzleExtended(await LoadTestInputAsync(1)))
        ];
    }
}