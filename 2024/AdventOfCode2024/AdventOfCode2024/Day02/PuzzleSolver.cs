namespace AdventOfCode2024.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        protected override string SolvePuzzle(string input)
        {
            var numberLines = GetMultipleNumbersInput(input);

            var result = 0;

            foreach (var numbers in numberLines)
            {
                var increasing = numbers[0] < numbers[1];
                var firstFailIndex = -1;
                var list = numbers.ToList();
                firstFailIndex = EvaluateSafe(list);
                var safe = firstFailIndex == -1;
                var origFail = firstFailIndex;
                var newList = new List<int>();

                if (!safe)
                {
                    safe = RemoveWrong(firstFailIndex);
                }

                if (!safe)
                {
                    safe = RemoveWrong(origFail + 1);
                }

                if (!safe)
                {
                    safe = RemoveWrong(0);
                }

                if (safe)
                {
                    result++;
                }

                int EvaluateSafe(List<int> numbas)
                {
                    for (int i = 0; i < numbas.Count - 1; i++)
                    {//5
                        var max = numbas[i] + (increasing ? 3 : -3);//8 || 2
                        var min = numbas[i] + (increasing ? 1 : -1);//6 || 4
                        var range = Enumerable.Range(Math.Min(max, min), 3);
                        if (!range.Contains(numbas[i + 1]))
                        {
                            firstFailIndex = i;
                            break;
                        }
                    }

                    return firstFailIndex;
                }

                bool RemoveWrong(int index)
                {
                    newList = list.ToList();
                    newList.RemoveAt(index);
                    increasing = newList[0] < newList[1];
                    firstFailIndex = -1;
                    EvaluateSafe(newList);
                    safe = firstFailIndex == -1;
                    return safe;
                }
            }

            return result.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var result = 0;

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("2", SolvePuzzle(await LoadTestInputAsync(1))),
                ("4", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
    }
}