using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day17
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "17";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("112", SolvePuzzle(await LoadTestInputAsync(1))),
                ("848", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }
        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, 3);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, 4);
        }
        
        private string SolvePuzzle(string input, int dimensions)
        {
            var lines = GetLinesInput(input).Select(a => a.ToCharArray()).ToArray();

            var depth = 12 + lines.Length + 1;
            if (depth % 2 == 1)
            {
                depth++;
            }

            var space = new Dictionary<string, bool>();
            var pivot = depth / 2;
            var width = (lines.Length % 2 == 1 ? lines.Length - 1 : lines.Length) / 2;
            const int dimWidth = 2;

            var stops = Stopwatch.StartNew();

            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    var index = new StringBuilder();
                    for (var i = 0; i < dimensions - 2; i++)
                    {
                        index.Append(pivot.ToString().PadLeft(dimWidth, '0'));
                    }

                    var yY = pivot - width + y;
                    index.Append(yY.ToString().PadLeft(dimWidth, '0'));
                    var xX = pivot - width + x;
                    index.Append(xX.ToString().PadLeft(dimWidth, '0'));

                    if (lines[y][x] == '#')
                    {
                        var t = 1;
                    }


                    space[index.ToString()] = lines[y][x] == '#';
                }
            }

            stops.Stop();

            Console.WriteLine($"Initializing took {stops.Elapsed}");

            stops.Restart();

            var neighbors = new List<string>();

            IterateNDimensionalSpace(
                -1,
                "",
                dimensions,
                0,
                2,
                0,
                dimWidth,
                false,
                neighbor =>
                {
                    neighbors.Add(neighbor);
                });

            stops.Stop();

            Console.WriteLine($"Neighbors took {stops.Elapsed}");

            for (var level = 1; level < 7; level++)
            {
                stops.Restart();

                var spaceXCopy = new Dictionary<string, bool>(space);

                IterateNDimensionalSpace(
                    -1,
                    "",
                    dimensions,
                    pivot - level - width,
                    pivot + level + width,
                    width,
                    dimWidth,
                    true,
                    cubeCoordinates =>
                    {
                        space.TryGetValue(cubeCoordinates, out var cube);

                        var activeNeighbors = 0;
                        var coordinates = new int[dimensions];

                        for (var i = 0; i < dimensions; i++)
                        {
                            coordinates[i] = int.Parse(cubeCoordinates.Substring(i * dimWidth, dimWidth));
                        }

                        foreach (var neighborShift in neighbors)
                        {
                            var adjacentCube = cubeCoordinates;
                            for (var i = 0; i < dimensions; i++)
                            {
                                var position = i * dimWidth;
                                var coordinateShift = int.Parse(neighborShift.Substring(position, dimWidth));

                                adjacentCube = adjacentCube.Remove(position, dimWidth)
                                    .Insert(position, (coordinates[i] + coordinateShift - 1)
                                        .ToString().PadLeft(dimWidth, '0'));
                            }

                            if (adjacentCube != cubeCoordinates && space.TryGetValue(adjacentCube, out var val) && val)
                            {
                                activeNeighbors++;
                            }
                        }


                        spaceXCopy[cubeCoordinates] = (cube && (activeNeighbors == 3 || activeNeighbors == 2)) || (!cube && activeNeighbors == 3);
                    });

                space = spaceXCopy;

                stops.Stop();

                Console.WriteLine($"Level {level} took {stops.Elapsed}");
            }

            return space.Count(a => a.Value).ToString();
        }


        private void IterateNDimensionalSpace(
            int actualDimension,
            string coordinates,
            int dimensions,
            int rangeFrom,
            int rangeTo,
            int width,
            int dimWidth,
            bool start2D,
            Action<string> pointAction)
        {
            if (++actualDimension == dimensions) return;

            var from = start2D && actualDimension + 2 < dimensions ? rangeFrom + width : rangeFrom;
            var to = start2D && actualDimension + 2 < dimensions ? rangeTo - width : rangeTo;

            for (var d = from; d < to + 1; d++)
            {
                var partialIndex = coordinates + d.ToString().PadLeft(dimWidth, '0');

                IterateNDimensionalSpace(
                    actualDimension,
                    partialIndex,
                    dimensions,
                    rangeFrom,
                    rangeTo,
                    width,
                    dimWidth,
                    start2D,
                    pointAction);
                if (actualDimension == dimensions - 1)
                {
                    pointAction(partialIndex);
                }
            }
        }
    }
}