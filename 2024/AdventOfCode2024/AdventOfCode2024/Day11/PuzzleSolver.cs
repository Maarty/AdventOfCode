namespace AdventOfCode2024.Day11;

public class PuzzleSolver : PuzzleSolverBase
{
    public override string Day => "11";

    private static readonly Dictionary<(long, long), long> _cache = new();

    protected override string SolvePuzzle(string input)
    {
        return SolvePuzzle(input, 25);
    }

    protected override string SolvePuzzleExtended(string input)
    {
        return SolvePuzzle(input, 75);
    }

    private static string SolvePuzzle(string input, int blinks)
    {
        var stones = new List<long>(input.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));

        var result = CalculateStones(stones, blinks);

        return result.ToString();
    }

    private static long CalculateStones(List<long> stones, long blinksLeft)
    {
        return stones.Sum(stone => CalculateStonesForStone(stone, blinksLeft));
    }

    private static long CalculateStonesForStone(long stone, long blinksLeft)
    {
        if (blinksLeft == 0) return 1;

        if (_cache.ContainsKey((stone, blinksLeft)))
        {
            return _cache[(stone, blinksLeft)];
        }

        long newStoneCount;
        var stoneStr = stone.ToString();

        if (stone == 0)
        {
            newStoneCount = CalculateStonesForStone(1, blinksLeft - 1);
        }
        else if (stoneStr.Length % 2 == 0)
        {
            var halfLength = stoneStr.Length / 2;
            var left = long.Parse(stoneStr[..halfLength]);
            var right = long.Parse(stoneStr[halfLength..]);

            newStoneCount = CalculateStonesForStone(left, blinksLeft - 1) + CalculateStonesForStone(right, blinksLeft - 1);
        }
        else
        {
            newStoneCount = CalculateStonesForStone(stone * 2024, blinksLeft - 1);
        }

        _cache[(stone, blinksLeft)] = newStoneCount;

        return newStoneCount;
    }

    public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
    {
        return
        [
            ("55312", SolvePuzzle(await LoadTestInputAsync(1))),
            //("11387", SolvePuzzleExtended(await LoadTestInputAsync(1)))
        ];
    }
}