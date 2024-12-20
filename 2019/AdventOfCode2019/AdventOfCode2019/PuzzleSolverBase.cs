﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019
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
