using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day15
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "15";

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);
            var y = lines.Length > 14 ? 2000000 : 10;
            return SolvePuzzle(input, false, y, 50);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var lines = GetLinesInput(input);
            var y = lines.Length > 14 ? 2000000 : 10;
            return SolvePuzzle(input, true, y, y == 10 ? 20 : 4000000);
        }

        private string SolvePuzzle(string input, bool extended, int yResult, int maxRange)
        {
            var sensors = ParseSensors(input);

            var noBeaconPositions = new Dictionary<int, List<NoBeaconRange>>();

            foreach (var sensor in sensors)
            {
                var lowRange = sensor.Point.Y - sensor.NoBeaconDistance;
                lowRange = lowRange < 0 ? 0 : lowRange;
                var highRange = sensor.Point.Y + sensor.NoBeaconDistance;
                highRange = highRange > maxRange ? maxRange : highRange;

                var ysToExamine = !extended ? new []{yResult} : Enumerable.Range(lowRange, highRange-lowRange+1);
                if (!extended && (yResult < sensor.Point.Y - sensor.NoBeaconDistance || yResult > sensor.Point.Y + sensor.NoBeaconDistance)) continue;

                foreach (var y in ysToExamine)
                {
                    var currentDistance = Math.Abs(sensor.Point.Y - y);

                    var xLow = sensor.Point.X - sensor.NoBeaconDistance + currentDistance;
                    var xHigh = sensor.Point.X + sensor.NoBeaconDistance - currentDistance;

                    var noBeaconRange = new NoBeaconRange(xLow, xHigh);
                    if (sensor.ClosestBeacon.Y == y)
                    {
                        noBeaconRange.Except.Add(sensor.ClosestBeacon.X);
                    }

                    if (noBeaconPositions.TryGetValue(y, out var xs))
                    {
                        var overlapped = false;
                        var toOverlap = noBeaconRange;

                        while (toOverlap != null)
                        {
                            var overlapping = xs.FirstOrDefault(
                                a => (((a.From+1 >= noBeaconRange.From && a.From-1 <= noBeaconRange.To) ||
                                     (a.To+1 >= noBeaconRange.From && a.To-1 <= noBeaconRange.To)) ||
                                ((noBeaconRange.From+1 >= a.From && noBeaconRange.From-1 <= a.To) ||
                                 (noBeaconRange.To+1 >= a.From && noBeaconRange.To-1 <= a.To))) && a != toOverlap
                            );

                            if (overlapping != null)
                            {
                                overlapped = true;
                                overlapping.Merge(toOverlap);
                                xs.Remove(toOverlap);
                                toOverlap = overlapping;
                            }
                            else
                            {
                                toOverlap = null;
                            }
                        }

                        if (!overlapped)
                        {
                            xs.Add(noBeaconRange);
                        }
                    }
                    else
                    {
                        noBeaconPositions[y] = new List<NoBeaconRange> { noBeaconRange };
                    }
                }
            }

            string result;
            if (!extended)
            {
                result = noBeaconPositions[yResult].Sum(a => a.NoBeaconPositions()).ToString();
            }
            else
            {
                var beacons = noBeaconPositions.First(a => a.Value.Count > 1);
                var smaller = beacons.Value.Min(a => a.To);
                result = ((((long)smaller + 1) * 4000000) + beacons.Key).ToString();
            }

            return result;
        }

        private List<Sensor> ParseSensors(string input)
        {
            var lines = GetLinesInput(input);
            var sensors = new List<Sensor>();

            foreach (var line in lines)
            {
                var points = line.Split(
                    new[] { ": closest beacon is at x=", "Sensor at x=" },
                    StringSplitOptions.RemoveEmptyEntries);

                var sensorPointSplit = points[0].Split(", y=", StringSplitOptions.RemoveEmptyEntries);
                var sensorPoint = new Point(int.Parse(sensorPointSplit[0]), int.Parse(sensorPointSplit[1]));

                var beaconPointSplit = points[1].Split(", y=", StringSplitOptions.RemoveEmptyEntries);
                var beaconPoint = new Point(int.Parse(beaconPointSplit[0]), int.Parse(beaconPointSplit[1]));

                sensors.Add(new Sensor(sensorPoint, beaconPoint));
            }

            return sensors;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("26", SolvePuzzle(await LoadTestInputAsync(1))),
                ("56000011", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        public class NoBeaconRange
        {
            public NoBeaconRange(int from, int to)
            {
                From = from;
                To = to;
                Except = new HashSet<int>();
            }

            public int From { get; set; }
            public int To { get; set; }
            public HashSet<int> Except { get; set; }

            public void Merge(NoBeaconRange other)
            {
                From = Math.Min(From, other.From);
                To = Math.Max(To, other.To);
                Except.UnionWith(other.Except);
            }

            public int NoBeaconPositions()
            {
                return Math.Abs(From - To) + 1 - Except.Count;
            }
        }

        public class Sensor
        {
            public Sensor(Point point, Point closestBeacon)
            {
                Point = point;
                ClosestBeacon = closestBeacon;
            }

            public Point Point { get; set; }
            public Point ClosestBeacon { get; set; }
            public int NoBeaconDistance => Math.Abs(Point.X - ClosestBeacon.X) + Math.Abs(Point.Y - ClosestBeacon.Y);
        }

        public long GetDistance(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }
    }
}
