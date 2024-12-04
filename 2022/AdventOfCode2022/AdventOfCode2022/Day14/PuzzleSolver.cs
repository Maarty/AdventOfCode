using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day14
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "14";

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, true);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected string SolvePuzzle(string input, bool countAbyss)
        {
            var lines = GetLinesInput(input).Select(a => a.Split(" -> ", StringSplitOptions.RemoveEmptyEntries)).Select(a => a.Select(x => (int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1]))).ToList()).ToList();

            var down = new Point(0, 1);
            var diagonalLeft = new Point(-1, 1);
            var diagonalRight = new Point(1, 1);

            var lengthY = 600;
            var lengthX = 3000;

            var matrix = new CavePoint[lengthX, lengthY];

            IterateMatrix(matrix, (x, y) => matrix[x,y] = new CavePoint(new Point(x,y)));

            var maxRockX = 500;
            var maxRockY = 0;
            var minRockX = 500;
            var minRockY = 0;

            foreach (var line in lines)
            {
                for (var i = 1; i < line.Count; i++)
                {
                    var stableX = line.ElementAt(i - 1).Item1 == line.ElementAt(i).Item1;
                    var stableCoor = stableX ? line.ElementAt(i).Item1 : line.ElementAt(i).Item2;
                    var from =  !stableX ? line.ElementAt(i-1).Item1 : line.ElementAt(i-1).Item2;
                    var to =  !stableX ? line.ElementAt(i).Item1 : line.ElementAt(i).Item2;

                    if (from > to)
                    {
                        (from, to) = (to, from);
                    }

                    for (var j = from; j < to + 1; j++)
                    {
                        var point = stableX ? matrix[stableCoor, j] : matrix[j, stableCoor];
                        point.CaveType = CaveType.Rock;
                        SetBoundaries(point);
                    }
                }
            }

            var totalSandNeeded = -1;
            var abyss = false;

            while (!abyss)
            {
                totalSandNeeded++;
                var sand = new Point(500, 0);
                //Print();
                while (true)
                {
                    if (countAbyss && (sand.X < minRockX || sand.X > maxRockX || sand.Y > maxRockY))
                    {
                        abyss = true;
                        break;
                    }

                    if (CanSettle(sand + down)) continue;
                    if (CanSettle(sand + diagonalLeft)) continue;
                    if (CanSettle(sand + diagonalRight)) continue;

                    if (sand.X == 500 && sand.Y == 0)
                    {
                        abyss = true;
                        break;
                    }

                    matrix[sand.X, sand.Y].CaveType = CaveType.Sand;

                    break;

                    bool CanSettle(Point newSand)
                    {
                        if (matrix[newSand.X, newSand.Y].CaveType != CaveType.Air || ( !countAbyss && (newSand.Y > maxRockY + 1))) return false;
                        sand = newSand;

                        return true;
                    }
                }
            }

            return countAbyss ? totalSandNeeded.ToString() : (totalSandNeeded+1).ToString();

            void SetBoundaries(CavePoint point)
            {
                if (point.Point.X > maxRockX) { maxRockX = point.Point.X; }
                if (point.Point.X < minRockX) { minRockX = point.Point.X; }
                if (point.Point.Y > maxRockY) { maxRockY = point.Point.Y; }
                if (point.Point.Y < minRockY) { minRockY = point.Point.Y; }
            }

            void Print()
            {
                IterateMatrix(
                    matrix,
                    (x, y) =>
                    {
                        if (x > minRockX - 1 && y < maxRockY + 1 && x < maxRockX + 1 && y > minRockY - 1)
                        {
                            Console.Write(matrix[x, y]);
                        }
                    },
                    y =>
                    {
                        if (y < maxRockY + 1 && y > minRockY - 1)
                        {
                            Console.WriteLine();
                        }
                    });
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("24", SolvePuzzle(await LoadTestInputAsync(1))),
                ("93", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        public class CavePoint
        {
            public CavePoint(Point point)
            {
                Point = point;
                CaveType = CaveType.Air;
            }
            public Point Point { get; set; }
            public CaveType CaveType { get; set; }

            public override string ToString()
            {
                return CaveType switch
                {
                    CaveType.Air => ".",
                    CaveType.Rock => "#",
                    CaveType.Sand => "o"
                };
            }
        }

        public enum CaveType
        {
            Air,
            Rock,
            Sand
        }
    }
}
