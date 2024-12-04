namespace AdventOfCode2024.Day01
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "01";

        protected override string SolvePuzzle(string input)
        {
            var numbers = GetNumbers(input);
            var queues = (new Queue<int>(numbers.Item1.OrderBy(a => a)), new Queue<int>(numbers.Item2.OrderBy(a => a)));

            var result = 0;
            var length = queues.Item1.Count;

            for (var i = 0; i < length; i++)
            {
                result += Math.Abs(queues.Item1.Dequeue() - queues.Item2.Dequeue());
            }

            return result.ToString();
        }

        private (List<int>, List<int>) GetNumbers(string input)
        {
            var first = new List<int>();
            var second = new List<int>();

            foreach (var line in GetLinesInput(input))
            {
                var numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                first.Add(numbers.First());
                second.Add(numbers.Last());
            }

            return (first, second);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var numbers = GetNumbers(input);

            var result = 0;
            var g = numbers.Item2.GroupBy( i => i );

            for (int i = 0; i < numbers.Item1.Count; i++)
            {
                var occurences = g.FirstOrDefault(a => a.Key == numbers.Item1[i])?.Count() ?? 0;
                result += numbers.Item1[i] * occurences;
            }

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("11", SolvePuzzle(await LoadTestInputAsync(1))),
                ("31", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private static IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }
    }
}