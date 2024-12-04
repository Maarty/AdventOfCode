using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day09
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "09";

        protected override string SolvePuzzle(string input)
        {
            var visited = new HashSet<Point> { new(0, 0) };
            var tailPosition = new Point(0, 0);
            var headPosition = new Point(0, 0);
            var up = new Point(0, -1);
            var down = new Point(0, 1);
            var left = new Point(-1, 0);
            var right = new Point(1, 0);

            var movements = GetLinesInput(input);

            foreach (var movement in movements)
            {
                var split = movement.Split(" ");
                DoMovement(split[0], int.Parse(split[1]));
            }

            void DoMovement(string direction, int steps)
            {
                for (var i = 0; i < steps; i++)
                {
                    switch (direction)
                    {
                        case "R":
                            headPosition += right;
                            break;
                        case "L":
                            headPosition += left;
                            break;
                        case "U":
                            headPosition += up;
                            break;
                        case "D":
                            headPosition += down;
                            break;
                    }

                    AdjustTail();
                }
            }

            void AdjustTail()
            {
                if (Math.Abs(headPosition.X - tailPosition.X) > 1)
                {
                    if (headPosition.X > tailPosition.X)
                    {
                        tailPosition += right;
                        tailPosition = new Point(tailPosition.X, headPosition.Y);
                    }
                    else
                    {
                        tailPosition += left;
                        tailPosition = new Point(tailPosition.X, headPosition.Y);
                    }
                    visited.Add(tailPosition);
                }

                if (Math.Abs(headPosition.Y - tailPosition.Y) > 1)
                {
                    if (headPosition.Y > tailPosition.Y)
                    {
                        tailPosition += down;
                        tailPosition = new Point(headPosition.X, tailPosition.Y);
                    }
                    else
                    {
                        tailPosition += up;
                        tailPosition = new Point(headPosition.X, tailPosition.Y);
                    }
                    visited.Add(tailPosition);
                }

            }

            return visited.Count.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var visited = new HashSet<Point> { new(0, 0) };
            //var tailPosition = new Point(0, 0);
            //var headPosition = new Point(0, 0);
            var up = new Point(0, -1);
            var down = new Point(0, 1);
            var left = new Point(-1, 0);
            var right = new Point(1, 0);
            var rope = new Point[10];
            for (var i = 0; i < 10; i++)
            {
                rope[i] = new Point(0, 0);
            }

            var movements = GetLinesInput(input);

            foreach (var movement in movements)
            {
                var split = movement.Split(" ");
                DoMovement(split[0], int.Parse(split[1]));
            }

            void DoMovement(string direction, int steps)
            {
                for (var i = 0; i < steps; i++)
                {
                    switch (direction)
                    {
                        case "R":
                            rope[0] += right;
                            break;
                        case "L":
                            rope[0] += left;
                            break;
                        case "U":
                            rope[0] += up;
                            break;
                        case "D":
                            rope[0] += down;
                            break;
                    }

                    for (var j = 0; j < 10-1; j++)
                    {
                        AdjustKnots(j, j+1);
                    }

                    visited.Add(rope[9]);
                }
            }

            void AdjustKnots(int head, int tail)
            {
                if (Math.Abs(rope[head].X - rope[tail].X) > 1)
                {
                    if (rope[head].X > rope[tail].X)
                    {
                        rope[tail] += right;
                    }
                    else
                    {
                        rope[tail] += left;
                    }

                    if (rope[head].Y > rope[tail].Y)
                    {
                        rope[tail] += down;
                    }
                    else if (rope[head].Y < rope[tail].Y)
                    {
                        rope[tail] += up;
                    }
                }

                if (Math.Abs(rope[head].Y - rope[tail].Y) > 1)
                {
                    if (rope[head].Y > rope[tail].Y)
                    {
                        rope[tail] += down;
                    }
                    else
                    {
                        rope[tail] += up;
                    }

                    if (rope[head].X > rope[tail].X)
                    {
                        rope[tail] += right;
                    }
                    else if (rope[head].X < rope[tail].X)
                    {
                        rope[tail] += left;
                    }
                }
            }

            return visited.Count.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("13", SolvePuzzle(await LoadTestInputAsync(1))),
                ("1", SolvePuzzleExtended(await LoadTestInputAsync(1))),
                ("36", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }
    }
}