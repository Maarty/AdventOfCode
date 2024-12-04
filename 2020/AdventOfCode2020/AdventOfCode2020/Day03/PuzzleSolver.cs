using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day03
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "03";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("7", SolvePuzzle(await LoadTestInputAsync(1), new List<(int Right, int Down)> { (3, 1) })),
                ("336",
                    SolvePuzzle(
                        await LoadTestInputAsync(1),
                        new List<(int Right, int Down)> { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, new List<(int Right, int Down)> { (3, 1) });
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, new List<(int Right, int Down)> { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) });
        }

        private string SolvePuzzle(string input, IReadOnlyCollection<(int Right, int Down)> slopes)
        {
            var lines = GetLinesInput(input);
            var mapWidth = (lines.Length - 1) * (slopes.Select(a => a.Right).Max()) + 1;
            var map = new char[lines.Length, mapWidth];
            var i = 0;

            foreach (var line in lines)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    var coordinate = j % line.Length;
                    map[i, j] = line[coordinate];
                }

                i++;
            }
            
            long result = 1;

            foreach (var slope in slopes)
            {
                var counter = 0;

                var right = slope.Right;

                for (int j = slope.Down; j < lines.Length; j+= slope.Down)
                {
                    if (map[j, right] == '#')
                    {
                        counter++;
                    }

                    right += slope.Right;
                }

                result *= counter;
            }

            return result.ToString();
        }

        private void PrintMap(char[,] arr)
        {
            var rowCount = arr.GetLength(0);
            var colCount = arr.GetLength(1);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                    Console.Write(arr[row, col]);
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
