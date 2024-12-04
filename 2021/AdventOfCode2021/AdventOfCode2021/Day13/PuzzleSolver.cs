using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2021.Helpers;

namespace AdventOfCode2021.Day13
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "13";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("17", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("5", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool allFolds)
        {
            var lines = GetLinesInput(input).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            var points = lines
                .Where(a => !a.StartsWith("f"))
                .Select(x => x.Split(","))
                .Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
                .ToList();

            var folds = lines
                .Where(a => a.StartsWith("f"))
                .Select(x => x.Replace("fold along ", string.Empty))
                .Select(a => a.Split("="))
                .Select(p => new Fold(p[0], int.Parse(p[1])))
                .ToList();

            var foldsQueue = allFolds ? new Queue<Fold>(folds) : new Queue<Fold>(new List<Fold> { folds.First() });

            var matrix = new char[points.Select(a => a.X).Max() + 1, points.Select(a => a.Y).Max() + 1];

            IterateMatrix(matrix, (x, y) => matrix[x,y] = points.Any(a => a.X == x && a.Y == y) ? '#' : '.');

            string totalDots;

            Fold(matrix, foldsQueue);

            void Fold(char[,] matrixToFold, Queue<Fold> remainingFolds)
            {
                var fold = remainingFolds.Dequeue();
                var maxX = fold.Axis == "y" ? matrixToFold.GetLength(0) : fold.Number;
                var maxY = fold.Axis == "y" ? fold.Number : matrixToFold.GetLength(1);
                var newMatrix = new char[maxX, maxY];

                IterateMatrix(
                    newMatrix,
                    (x, y) =>
                    {
                        var newX = fold.Axis == "y" ? x : fold.Number + (fold.Number - x);
                        var newY = fold.Axis == "y" ? fold.Number + (fold.Number - y) : y;
                        if (matrixToFold[x, y] == '#' || (newX < matrixToFold.GetLength(0) && newY < matrixToFold.GetLength(1) && matrixToFold[newX, newY] == '#'))
                        {
                            newMatrix[x, y] = '#';
                        }
                        else
                        {
                            newMatrix[x, y] = '.';
                        }
                    });

                if (remainingFolds.Count > 0)
                {
                    Fold(newMatrix, remainingFolds);
                }
                else
                {
                    totalDots = newMatrix.Cast<char>().Count(a => a == '#').ToString();

                    if (allFolds)
                    {
                        PrintMatrix(newMatrix);
                    }
                }
            }

            return totalDots;
        }

        private record Fold(string Axis, int Number);
    }
}