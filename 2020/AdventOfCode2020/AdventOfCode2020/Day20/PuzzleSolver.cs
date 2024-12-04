using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day20
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "20";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                 ("20899048083289", SolvePuzzle(await LoadTestInputAsync(1))),
                 ("273", SolvePuzzleExtended(await LoadTestInputAsync(1))),
             };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, null);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var seaMonster = @"                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ";
            return SolvePuzzle(input, seaMonster);
        }

        record Tile(long Number, string[] Sides, bool[][] Content);
        record PutTile(Tile Tile, bool [][] RotatedContent);

        private string SolvePuzzle(string input, string seaMonster)
        {
            var tiles = ParseTiles(input);

            var sides = new Dictionary<string, List<Tile>>();

            var squareSize = (int)Math.Sqrt(tiles.Count);
            var tileSize = tiles.First().Sides.First().Length;

            GetEdgeTiles(tiles, sides, out var borderTiles, out var cornerTiles);

            if (string.IsNullOrWhiteSpace(seaMonster))
            {
                return cornerTiles.Select(a => a.Number).Aggregate((a, b) => a * b).ToString();
            }
            
            var image = new PutTile[squareSize, squareSize];
            
            var availableTiles = new List<Tile>(tiles);

            SolveImage(null, 0, 0);

            var rendered = RenderFinalImage();

            //PrintImage(rendered);

            var monster = ParseSeaMonster(seaMonster);

            for (var k = 0; k < 4; k++)
            {
                var rotated = RotateMatrix(rendered, k);
                var m = FindMonsters(rotated);

                if (m > 0)
                {
                    return GetWaterRoughness(rendered, monster, m).ToString();
                }
                
                m = FindMonsters(FlipMatrix(rotated));

                if (m > 0)
                {
                    return GetWaterRoughness(rendered, monster, m).ToString();
                }

            }

            int FindMonsters(bool[][] img)
            {
                var monsterWidth = monster[0].GetLength(0);
                var monsterHeight = monster.Length;
                var found = 0;
                for (var i = 0; i < squareSize * (tileSize - 2) - monster.Length; i++)
                {
                    for (var j = 0; j < squareSize * (tileSize - 2) - monster[0].GetLength(0); j++)
                    {
                        var search = true;
                        for (var k = 0; k < monsterHeight; k++)
                        {
                            for (var l = 0; l < monsterWidth; l++)
                            {
                                if (!monster[k][l]) continue;
                                if (!img[i + k][j + l]) search = false;
                            }
                        }

                        if (search)
                        {
                            found++;
                        }
                    }
                }

                return found;
            }

            bool[][] RenderFinalImage()
            {
                var img = new bool[squareSize * (tileSize - 2)][];
                for (var i = 0; i < squareSize; i++)
                {
                    for (var j = 0; j < squareSize; j++)
                    {
                        for (var k = 1; k < tileSize - 1; k++)
                        {
                            for (var l = 1; l < tileSize - 1; l++)
                            {
                                var x = i * (tileSize - 2) + k - 1;
                                var y = j * (tileSize - 2) + l - 1;
                                img[x] ??= new bool[squareSize * (tileSize - 2)];
                                img[x][y] = image[i, j].RotatedContent[k][l];
                            }
                        }
                    }
                }

                return img;
            }

            bool SolveImage(Tile previousTile, int row, int column)
            {
                if (image[squareSize - 1, squareSize - 1] != null) return true;

                if (!availableTiles.Any())
                {
                    return false;
                }

                if (previousTile != null)
                {
                    availableTiles.Remove(previousTile);
                }

                var possibleTiles = GetTileCandidates(cornerTiles, borderTiles, previousTile, row, column, squareSize, availableTiles);

                foreach (var possibleTile in possibleTiles)
                {
                    var fits = PossibleTileFits(possibleTile, row, column);
                    if (!fits.Any())
                    {
                        continue;
                    }

                    foreach (var fit in fits)
                    {
                        image[row, column] = new PutTile(possibleTile, fit);
                        var nextRow = column == squareSize - 1 ? row + 1 : row;
                        var nextColumn = column == squareSize - 1 ? 0 : column + 1;
                        previousTile = possibleTile;

                        if (SolveImage(previousTile, nextRow, nextColumn)) return true;

                        availableTiles.Add(possibleTile);
                        image[row, column] = null;
                    }
                }
                
                return false;
            }

            List<bool[][]> PossibleTileFits(Tile tile, int i, int j)
            {
                var top = i > 0 ? image[i - 1, j] : null; 
                var left = j > 0 ? image[i, j - 1] : null;

                var possibleRotations = new List<bool[][]>();

                for (var k = 0; k < 4; k++)
                {
                    var rotated = RotateMatrix(tile.Content, k);
                    if (CompareSide(left?.RotatedContent, rotated, true) && CompareSide(top?.RotatedContent, rotated, false))
                    {
                        possibleRotations.Add(rotated);
                    }
                    var flipped = FlipMatrix(rotated);
                    if (CompareSide(left?.RotatedContent, flipped, true) && CompareSide(top?.RotatedContent, flipped, false))
                    {
                        possibleRotations.Add(flipped);
                    }
                }

                return possibleRotations;
            }

            return string.Empty;
        }

        private static List<Tile> GetTileCandidates(
            List<Tile> cornerTiles,
            List<Tile> borderTiles,
            Tile previousTile,
            int row,
            int column,
            int squareSize,
            List<Tile> availableTiles)
        {
            List<Tile> possibleTiles;
            var corners = cornerTiles.Select(a => a.Number).ToList();
            var borders = borderTiles.Select(a => a.Number).ToList();

            if (previousTile == null || row == 0 && column == squareSize - 1 || row == squareSize - 1 && column == 0 ||
                row == squareSize - 1 && column == squareSize - 1)
            {
                possibleTiles = availableTiles.Where(a => corners.Contains(a.Number)).ToList();
            }
            else if (row == 0 || column == 0 || row == squareSize - 1 || column == squareSize - 1)
            {
                possibleTiles = availableTiles.Where(a => borders.Contains(a.Number)).ToList();
            }
            else
            {
                possibleTiles = availableTiles.Where(a => !borders.Contains(a.Number) && !corners.Contains(a.Number)).ToList();
            }

            return possibleTiles;
        }

        private bool CompareSide(bool[][] placed, bool[][] candidate, bool right)
        {
            if (placed == null)
            {
                return true;
            }

            var matrixSides = GetMatrixSides(placed);
            var newSides = GetMatrixSides(candidate);

            if (right)
            {
                return matrixSides[1] == newSides[3];
            }

            return matrixSides[2] == newSides[0];
        }

        private static void GetEdgeTiles(List<Tile> tiles, Dictionary<string, List<Tile>> sides, out List<Tile> borderTiles, out List<Tile> cornerTiles)
        {
            foreach (var tile in tiles)
            {
                foreach (var tileSide in tile.Sides)
                {
                    void AddSide(string s, Tile tile1)
                    {
                        if (sides.ContainsKey(s))
                        {
                            if (!sides[s].Contains(tile1))
                            {
                                sides[s].Add(tile1);
                            }
                        }
                        else
                        {
                            sides[s] = new List<Tile> { tile1 };
                        }
                    }

                    var flipped = new string(tileSide.Reverse().ToArray());

                    AddSide(tileSide, tile);
                    AddSide(flipped, tile);
                }
            }

            borderTiles = GetTilesByBorderSides(1);
            cornerTiles = GetTilesByBorderSides(2);

            List<Tile> GetTilesByBorderSides(int edgesCount)
            {
                var result = new List<Tile>();

                foreach (var tile in tiles)
                {
                    if (tile.Sides.Count(
                        a =>
                        {
                            if (sides[a].Count == 1)
                            {
                                var flipped = new string(a.Reverse().ToArray());

                                return sides[flipped].Count == 1;
                            }

                            return false;
                        }) == edgesCount)
                    {
                        result.Add((tile));
                    }
                }

                return result;
            }
        }

        private static int GetWaterRoughness(bool[][] rendered, bool[][] monster, int monsters)
        {
            var allX = rendered.SelectMany(a => a).Count(a => a);
            var monsterX = monster.SelectMany(a => a).Count(a => a);

            return (allX - monsterX * monsters);
        }

        private string[] GetMatrixSides(bool[][] matrix)
        {
            var top = string.Join("", matrix[0].Select(a => a ? '#' : '.'));
            var bottom = string.Join("", matrix[^1].Select(a => a ? '#' : '.'));
            var left = string.Empty;
            var right = string.Empty;

            for (int i = 0; i < matrix.Length; i++)
            {
                left += matrix[i][0] ? '#' : '.';
                right += matrix[i][^1] ? '#' : '.';
            }

            return new[] { top, right, bottom, left };
        }

        private bool[][] RotateMatrix(bool[][] matrix, int times)
        {
            if (times == 0)
            {
                return matrix;
            }
            
            var n = matrix.GetLength(0);

            var reference = (bool[][])matrix.Clone();

            var ret = new bool[n][];

            for (var t = 0; t < times; t++)
            {
                for (var i = 0; i < n; ++i)
                {
                    ret[i] = new bool[n];
                    for (var j = 0; j < n; ++j)
                    {
                        ret[i][j] = reference[n - j - 1][i];
                    }
                }

                reference = (bool[][])ret.Clone();
            }

            return ret;
        }

        private bool[][] FlipMatrix(bool[][] matrix)
        {
            var n = matrix.GetLength(0);

            var ret = new bool[n][];

            for (var i = 0; i < n; ++i)
            {
                ret[i] = new bool[n];
                for (var j = 0; j < n; ++j)
                {
                    ret[i][j] = matrix[n - 1 - i][j];
                }
            }

            return ret;
        }

        private List<Tile> ParseTiles(string input)
        {
            return input.Split(Environment.NewLine + Environment.NewLine).Select(
                x =>
                {
                    var lines = GetLinesInput(x);
                    var number = int.Parse(lines[0][5..(lines[0].Length - 1)]);
                    var top = lines[1];
                    var bottom =lines[^1];
                    var left = string.Empty;
                    var right = string.Empty;

                    for (int i = 1; i < lines.Length; i++)
                    {
                        left += lines[i][0];
                        right += lines[i][^1];
                    }

                    var content = new bool[lines.Length - 1][];

                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        content[i] = new bool[lines[i].Length];
                        for (int j = 0; j < lines[i].Length; j++)
                        {
                            content[i][j] = lines[i+ 1][j] == '#';
                        }
                    }

                    return new Tile(number, new[] { top, right, bottom, left }, content);
                }).ToList();
        }

        private bool[][] ParseSeaMonster(string seaMonster)
        {
            return seaMonster.Split(Environment.NewLine).Select(
                x =>
                {
                    var monster = new bool[x.Length];
                        for (int j = 0; j < x.Length; j++)
                        {
                            monster[j] = x[j] == '#';
                        }

                    return monster;
                }).ToArray();
        }

        private void PrintImage(bool[][] img)
        {
            var size = img.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"{(img[i][j] ? "#" : ".")}");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}