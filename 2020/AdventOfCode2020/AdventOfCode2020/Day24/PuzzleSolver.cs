using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day24
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "24";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                ("10", SolvePuzzle(await LoadTestInputAsync(1))),
                ("2208", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        private string SolvePuzzle(string input, bool performFlipping)
        {
            var tilesIdentifications = GetLinesInput(input);

            var maxLength = tilesIdentifications.Select(a => a.Length - Regex.Matches(a, "se|sw|ne|nw").Count).Max();

            var grid = new NegativeArray(maxLength);

            foreach (var tilesIdentification in tilesIdentifications)
            {
                var steps = new StringBuilder(tilesIdentification);

                var x = 0;
                var y = 0;
                var z = 0;

                while (steps.Length > 0)
                {
                    var step = steps[0].ToString();
                    switch (step)
                    {
                        case "n":
                        case "s":
                            step = steps.ToString(0, 2);
                            break;
                    }

                    switch (step)
                    {
                        case "w":
                            x--;
                            y++;
                            break;
                        case "e":
                            x++;
                            y--;
                            break;
                        case "sw":
                            z++;
                            x--;
                            break;
                        case "se":
                            z++;
                            y--;
                            break;
                        case "nw":
                            y++;
                            z--;
                            break;
                        case "ne":
                            x++;
                            z--;
                            break;
                    }

                    steps.Remove(0, step.Length);
                }

                grid[x,y,z] = !grid[x,y,z];
            }

            if (performFlipping)
            {
                for (var i = 0; i < 100; i++)
                {
                    grid.ApplyDayFlipping();
                }
            }

            return grid.GetBlackTiles().Count.ToString();
        }

        public class NegativeArray : ICloneable
        {
            public bool[,,] Array;
            private readonly int _offset;

            private readonly int[][] _offsets = new int[][]
            {
                new[] { -1, 1, 0 }, new[] { -1, 0, 1 }, new[] { 0, -1, 1 }, new[] { 1, -1, 0 }, new[] { 1, 0, -1 },
                new[] { 0, 1, -1 }
            };

            public NegativeArray(int size)
            {
                _offset = size + 101;
                Array = new bool[_offset * 2, _offset * 2, _offset * 2];
            }

            public NegativeArray(bool[,,] initial, int offset)
            {
                Array = initial;
                _offset = offset;
            }

            public bool this[int x, int y, int z]
            {
                get => Array[GetCrd(x), GetCrd(y), GetCrd(z)];
                set => Array[GetCrd(x), GetCrd(y), GetCrd(z)] = value;
            }

            public List<int[]> GetBlackTiles()
            {
                var result = new List<int[]>();
                for (var x = 0; x < _offset*2; x++)
                {
                    for (var y = 0; y < _offset*2; y++)
                    {
                        for (var z = 0; z < _offset*2; z++)
                        {
                            if (Array[x,y,z])
                            {
                                result.Add(new[] { GetCrdNeg(x), GetCrdNeg(y), GetCrdNeg(z) });
                            }
                        }
                    }
                }

                return result;
            }

            public void ApplyDayFlipping()
            {
                var copy = (NegativeArray)Clone();

                var blackTiles = GetBlackTiles();

                var toExamine = new List<int[]>();
                var examineMap = new HashSet<string>();

                foreach (var blackTile in blackTiles)
                {
                    var neighbors = _offsets.Select(a => new[] { blackTile[0] + a[0], blackTile[1] + a[1], blackTile[2] + a[2] }).ToList();

                    toExamine.AddRange(from neighbor in neighbors let key = $"{neighbor[0]},{neighbor[1]}, {neighbor[2]}" where examineMap.Add(key) select neighbor);

                    var keyBlack = $"{blackTile[0]},{blackTile[1]}, {blackTile[2]}";
                    if (examineMap.Add(keyBlack))
                    {
                        toExamine.Add(blackTile);
                    }
                }

                foreach (var neighbor in toExamine)
                {
                    var adjacent = _offsets.Count(a => this[neighbor[0] + a[0], neighbor[1] + a[1], neighbor[2] + a[2]]);
                    if (this[neighbor[0], neighbor[1], neighbor[2]] && adjacent == 0 || adjacent > 2)
                    {
                        copy[neighbor[0], neighbor[1], neighbor[2]] = false;
                    }
                    if (!this[neighbor[0], neighbor[1], neighbor[2]] && adjacent == 2)
                    {
                        copy[neighbor[0], neighbor[1], neighbor[2]] = true;
                    }
                }

                Array = copy.Array;
            }

            private int GetCrd(int index)
            {
                return index >= 0 ? index : -index + _offset;
            }

            private int GetCrdNeg(int index)
            {
                return index >= _offset ? -(index-_offset) : index;
            }

            public object Clone()
            {
                return new NegativeArray((bool[,,])Array.Clone(), _offset);
            }
        }
    }
}