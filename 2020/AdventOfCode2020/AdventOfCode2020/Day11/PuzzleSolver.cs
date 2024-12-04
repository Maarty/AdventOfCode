using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day11
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        private readonly int[][] _seatDirections;

        public PuzzleSolver()
        {
            _seatDirections = Enumerable.Range(-1, 3).SelectMany(x => Enumerable.Range(-1, 3), (x, y) => new[] { x, y })
                .Where(a => !(a[0] == 0 && a[1] == 0))
                .Select(a => a.ToArray()).Distinct().ToArray();
        }

        public override string Day => "11";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("37", SolvePuzzle(await LoadTestInputAsync(1))),
                ("26", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 4, OccupiedAdjacent);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 5, GetOccupiedSeats);
        }

        private string SolvePuzzle(
            string input,
            int maxOccupiedSeats,
            Func<char[][], int, int, int> occupiedAdjacentSeatsFunc)
        {
            var lines = GetLinesInput(input);

            var map = lines.Select(a => a.ToCharArray()).ToArray();

            var changed = true;

            while (changed)
            {
                changed = ChangeMap(map, occupiedAdjacentSeatsFunc, maxOccupiedSeats, out var adjustedMap);
                map = adjustedMap;
            }

            return map.SelectMany(a => a).Count(a => a == '#').ToString();
        }

        private static bool ChangeMap(char[][] map, Func<char[][], int, int, int> occupiedAdjacentSeatsFunc, int maxOccupied, out char[][] adjustedMap)
        {
            adjustedMap = map.Select(a => a.ToArray()).ToArray();
            var changed = false;

            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    var adjacent = occupiedAdjacentSeatsFunc(map, i, j);

                    switch (map[i][j])
                    {
                        case 'L' when adjacent == 0:
                            adjustedMap[i][j] = '#';
                            changed = true;
                            break;
                        case '#' when adjacent >= maxOccupied:
                            adjustedMap[i][j] = 'L';
                            changed = true;
                            break;
                    }
                }
            }

            return changed;
        }

        private static int OccupiedAdjacent(char[][] arr, int row, int column)
        {
            var occupied = 0;

            for (var j = row - 1; j <= row + 1; j++)
            {
                for (var i = column - 1; i <= column + 1; i++)
                {
                    if (i >= 0 && j >= 0 && i < arr[0].Length && j < arr.Length && !(j == row && i == column) && arr[j][i] == '#')
                        occupied++;
                }
            }

            return occupied;
        }

        private int GetOccupiedSeats(char[][] arr, int row, int column)
        {
            return _seatDirections.Select(a => GetFirstVisibleSeat(arr, row, column, a[0], a[1])).Count(a => a == '#');
        }

        private static char GetFirstVisibleSeat(char[][] arr, int row, int col, int rowDir, int colDir)
        {
            var next = '.';

            while (next != 'X')
            {
                if (row + rowDir > arr.Length - 1 || row + rowDir < 0 || col + colDir > arr[0].Length - 1 || col + colDir < 0)
                {
                    next = 'X';
                }
                else
                {
                    row += rowDir;
                    col += colDir;
                    next = arr[row][col];

                    if (next != '.')
                    {
                        return next;
                    }
                }
            }

            return 'X';
        }
    }
}
