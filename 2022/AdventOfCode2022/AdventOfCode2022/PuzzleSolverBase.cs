using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022
{
    public abstract class PuzzleSolverBase : IPuzzleSolver
    {
        public abstract string Day { get; }

        public virtual ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new ValueTask<List<(string Expected, string Actual)>>();
        }

        public async Task<PuzzleResult> SolvePuzzleAsync()
        {
            return await SolvePuzzleAsync((SolvePuzzle));
        }

        public async Task<PuzzleResult> SolvePuzzleExtendedAsync()
        {
            return await SolvePuzzleAsync((SolvePuzzleExtended));
        }

        private async Task<PuzzleResult> SolvePuzzleAsync(Func<string, string> method)
        {
            var input = await LoadInputAsync();

            var stopwatch = Stopwatch.StartNew();
            var result = method(input);
            stopwatch.Stop();

            return new PuzzleResult(result, stopwatch.Elapsed);
        }

        protected virtual string SolvePuzzle(string input)
        {
            return "Not implemented yet";
        }

        protected virtual string SolvePuzzleExtended(string input)
        {
            return "Not implemented yet";
        }

        protected long[] GetNumbersInput(string input)
        {
            return input.Split(Environment.NewLine).Select(long.Parse).ToArray();
        }

        protected string[] GetLinesInput(string input)
        {
            return input.Split(Environment.NewLine).ToArray();
        }

        protected void IterateMatrix<T>(T[,] matrix, Action<int, int> xAction, Action<int> yAction = null)
        {
            var maxY = matrix.GetLength(1);
            var maxX = matrix.GetLength(0);

            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    xAction(x, y);
                }

                yAction?.Invoke(y);
            }
        }

        protected void PrintMatrix<T>(T[,] matrix)
        {
            IterateMatrix(matrix, (x, y) => Console.Write(matrix[x, y]), _ => Console.WriteLine());
            Console.WriteLine();
        }

        protected async Task<string> LoadInputAsync()
        {
            return await LoadFileInputAsync("puzzle");
        }

        protected async Task<string> LoadTestInputAsync(int index)
        {
            return await LoadFileInputAsync("test" + index);
        }
        private async Task<string> LoadFileInputAsync(string name)
        {
            var path = Path.Combine("Day" + Day, $"{name}.txt");
            return await File.ReadAllTextAsync(path);
        }
    }
}