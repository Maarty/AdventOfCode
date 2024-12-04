using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day17
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "17";

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
                ("45", SolvePuzzle(await LoadTestInputAsync(1))),
                ("112", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool countHits)
        {
            var sub = input.Replace("target area: x=", "").Replace("y=", "").Split(", ");
            var xSplit = sub[0].Split("..", StringSplitOptions.RemoveEmptyEntries);
            var ySplit = sub[1].Split("..", StringSplitOptions.RemoveEmptyEntries);

            var xMin = int.Parse(xSplit[0]);
            var xMax = int.Parse(xSplit[1]);
            var yMin = int.Parse(ySplit[0]);
            var yMax = int.Parse(ySplit[1]);

            var maxHeight = 0;
            var hits = 0;

            for (var x = 0; x <= xMax; x++)
            {
                for (var y = yMin; y <= -yMin; y++)
                {
                    var actualX = 0;
                    var actualY = 0;
                    var xVel = x;
                    var yVel = y;
                    var runMaxHeight = 0;

                    while (true)
                    {
                        if (actualX >= xMin && actualX <= xMax && actualY <= yMax && actualY >= yMin)
                        {
                            hits++;
                            if (runMaxHeight > maxHeight)
                            {
                                maxHeight = runMaxHeight;
                            }

                            break;
                        }
                        if (actualX > xMax || actualY < yMin)
                        {
                            break;
                        }

                        actualX += xVel;
                        actualY += yVel;
                        if (actualY > runMaxHeight) runMaxHeight = actualY;

                        if (xVel > 0) xVel -= 1;
                        yVel -= 1;
                    }
                }
            }

            return !countHits ? maxHeight.ToString() : hits.ToString();
        }
    }
}