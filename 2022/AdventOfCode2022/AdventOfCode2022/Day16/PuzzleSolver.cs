using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;
using MoreLinq;

namespace AdventOfCode2022.Day16
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "16";

        protected override string SolvePuzzle(string input)
        {
            return string.Empty;
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            //return string.Empty;
            return SolvePuzzle(input, true);
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                //("1651", SolvePuzzle(await LoadTestInputAsync(1))),
                ("1707", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        private string SolvePuzzle(string input, bool extended)
        {
            var valves = GetValves(input);

            var openValves = new HashSet<string>();

            var x = MaxRatePathExtended(new Boyyy(valves["AA"]), new Boyyy(valves["AA"]), ref openValves, valves, 0, 26, 0);

            return x.ToString();
        }

        private static int MaxRatePathExtended(
            Boyyy me,
            Boyyy freakingElephant,
            ref HashSet<string> openedValves,
            Dictionary<string, Valve> allValves,
            int minute,
            int maxMinutes,
            int totalFlowGained)
        {
            minute++;

            if (openedValves.Count == allValves.Count(a => a.Value.FlowRate > 0) || minute == maxMinutes) return me.FlowGained + freakingElephant.FlowGained;

            var opened = Enumerable.ToHashSet(openedValves);

            var shouldINotBother = IsTargetValveAlreadyOpen(me, opened);
            var shouldElephantNotBother = IsTargetValveAlreadyOpen(freakingElephant, opened);

            var meSkips = !shouldINotBother && (PerformDelay(me) || PerformMove(me));
            var elephantSkips = !shouldElephantNotBother && (PerformDelay(freakingElephant) || PerformMove(freakingElephant));


            if (!meSkips)
            {
                var valve = PerformOpen(me, opened);
                if (valve != null)
                {
                    openedValves.Add(valve.Name);
                }
            }

            if (!elephantSkips)
            {
                if (openedValves.Count == 5)
                {
                    var c = 1;
                }
                var valve = PerformOpen(freakingElephant, opened);
                if (valve != null)
                {
                    openedValves.Add(valve.Name);
                }
            }

            if (meSkips && elephantSkips)
            {
                return MaxRatePathExtended(
                    me,
                    freakingElephant,
                    ref openedValves,
                    allValves,
                    minute,
                    maxMinutes,
                    totalFlowGained);
            }

            opened = Enumerable.ToHashSet(openedValves);

            var closedValves = allValves.Where(a => !opened.Contains(a.Value.Name)).ToList();


            var maxPossibleFlow = totalFlowGained;
            var finalOpenedValves = Enumerable.ToHashSet(openedValves);
            var bestMe = 0;
            var bestElephant = 0;

            var availableValves = closedValves.Where(a => a.Value.FlowRate > 0).Select(a => a.Value).ToList();

            List<IList<Valve>> valveSubsets;

            if (meSkips || elephantSkips || availableValves.Count < 2)
            {
                valveSubsets = availableValves.Select(a => new List<Valve> { a, a } as IList<Valve>).ToList();
            }
            else
            {
                valveSubsets = availableValves.Subsets(2).ToList();

                valveSubsets = valveSubsets.Union(valveSubsets.Select(a => a.Reverse().ToList()))
                    .Where(a => a.First() != a.Last()).ToList();
            }

            foreach (var closedCombo in valveSubsets)
            {
                var hypotheticalMe = me.NextNode != null ? me : GetHypotheticalBoyyy(me, closedCombo.First()) ?? me;
                var hypotheticalElephant = freakingElephant.NextNode != null ? freakingElephant : GetHypotheticalBoyyy(freakingElephant, closedCombo.Last()) ?? freakingElephant;

                var pathOpenedValves = Enumerable.ToHashSet(openedValves);

                var pathTotalFlowGained = MaxRatePathExtended(
                    hypotheticalMe,
                    hypotheticalElephant,
                    ref pathOpenedValves,
                    allValves,
                    minute,
                    maxMinutes,
                    totalFlowGained);

                if (pathTotalFlowGained > maxPossibleFlow)
                {
                    maxPossibleFlow = pathTotalFlowGained;
                    finalOpenedValves = pathOpenedValves;
                    bestMe = hypotheticalMe.FlowGained;
                    bestElephant = hypotheticalElephant.FlowGained;
                }
            }

            me.FlowGained = bestMe;
            freakingElephant.FlowGained = bestElephant;

            openedValves = finalOpenedValves;

            return maxPossibleFlow;

            bool PerformMove(Boyyy boyyy)
            {
                if (boyyy.Start == boyyy.NextNode || boyyy.NextNode == null) return false;
                boyyy.Start = boyyy.PathToNextNode.Dequeue();
                return true;
            }

            Valve PerformOpen(Boyyy boyyy, HashSet<string> open)
            {
                if (minute == 1 || open.Contains(boyyy.Start.Name)) return null;
                boyyy.FlowGained += (maxMinutes - minute) * boyyy.Start.FlowRate;
                var temp = boyyy.NextNode;
                boyyy.NextNode = null;
                boyyy.IsOpening = true;

                return temp;
            }

            bool PerformDelay(Boyyy boyyy)
            {
                if (!boyyy.IsOpening) return false;
                boyyy.IsOpening = false;
                return true;
            }

            bool IsTargetValveAlreadyOpen(Boyyy boyyy, HashSet<string> open)
            {
                if (boyyy.NextNode == null || !open.Contains(boyyy.NextNode.Name)) return false;
                boyyy.NextNode = null;
                boyyy.PathToNextNode = new Queue<Valve>();
                return true;
            }

            Boyyy GetHypotheticalBoyyy(Boyyy boyyy, Valve valve)
            {
                var hypotheticalBoyyy = boyyy.CreateHypothetic();
                var shortestPath = ShortestPathExtended(
                    boyyy.Start,
                    valve,
                    new HashSet<string>(),
                    new Dictionary<string, int> { { boyyy.Start.Name, 0 } },
                    new Dictionary<string, Queue<Valve>>());

                var pathMinute = minute + shortestPath.Cost;
                if (pathMinute >= maxMinutes) { return null; }
                hypotheticalBoyyy.Start = shortestPath.Path.Dequeue();
                hypotheticalBoyyy.NextNode = valve;
                hypotheticalBoyyy.PathToNextNode = shortestPath.Path;

                return hypotheticalBoyyy;
            }
        }

        private static (int Cost, Queue<Valve> Path) ShortestPathExtended(Valve start, Valve end, HashSet<string> visitedValves, Dictionary<string, int> distances, Dictionary<string, Queue<Valve>> paths)
        {
            if (start == end)
            {
                var x = new Queue<Valve>();
                x.Enqueue(start);

                return (0, x);
            }

            var toExamine = new Queue<Valve>();
            toExamine.Enqueue(start);

            while (toExamine.TryDequeue(out var valve))
            {
                if (visitedValves.Contains(valve.Name)) continue;

                foreach (var adjacent in valve.Neighbours)
                {
                    // if (visitedValves.Contains(adjacent.Name))
                    // {
                    //     continue;
                    // }

                    distances[adjacent.Name] = distances[valve.Name] + 1;

                    if (!paths.ContainsKey(adjacent.Name))
                    {
                        paths[adjacent.Name] = paths.ContainsKey(valve.Name)
                            ? new Queue<Valve>(paths[valve.Name])
                            : new Queue<Valve>();
                    }

                    if (!visitedValves.Contains(adjacent.Name) && paths[adjacent.Name].Count < distances[adjacent.Name])
                    {
                        paths[adjacent.Name].Enqueue(valve);
                    }

                    toExamine.Enqueue(adjacent);
                }

                visitedValves.Add(valve.Name);
                if (valve != end) continue;
                paths[end.Name].Enqueue(end);
                if (paths[end.Name].Peek().Name == start.Name)
                {
                    paths[end.Name].Dequeue();
                }
                return (distances[end.Name], paths[end.Name]);
            }

            var path = new Queue<Valve>();
            path.Enqueue(start);

            return (0, path);
        }

        // private static int MaxRatePath(
        //     Valve startMe,
        //     Valve nextNode,
        //     Queue<Valve> pathToNextNode,
        //     ref HashSet<string> openedValves,
        //     Dictionary<string, Valve> allValves,
        //     int minute,
        //     int maxMinutes,
        //     int totalFlowGained)
        // {
        //     if (startMe != nextNode && nextNode != null)
        //     {
        //         minute++;
        //         return MaxRatePath(
        //             pathToNextNode.Dequeue(),
        //             nextNode,
        //             pathToNextNode,
        //             ref openedValves,
        //             allValves,
        //             minute,
        //             maxMinutes,
        //             totalFlowGained);
        //     }
        //     else if (startMe.Name != "AA" || minute != 1)
        //     {
        //         minute++;
        //         totalFlowGained += (maxMinutes - minute) * startMe.FlowRate;
        //         minute++;
        //     }
        //
        //     if (openedValves.Count == allValves.Count(a => a.Value.FlowRate > 0) || minute == maxMinutes) return totalFlowGained;
        //
        //     var opened = Enumerable.ToHashSet(openedValves);
        //
        //     var closedValves = allValves.Where(a => !opened.Contains(a.Value.Name)).ToList();
        //
        //     var maxPossibleFlow = totalFlowGained;
        //     var finalOpenedValves = Enumerable.ToHashSet(openedValves);
        //
        //     foreach (var closedValve in closedValves.Where(a => a.Value.FlowRate > 0))
        //     {
        //         var shortestPath = ShortestPath(
        //             startMe,
        //             closedValve.Value,
        //             new HashSet<string>(),
        //             new Dictionary<string, int> { { startMe.Name, 0 } },
        //             new Dictionary<string, Queue<Valve>>());
        //         var pathMinute = minute + shortestPath.Cost;
        //         if (pathMinute >= maxMinutes) { continue; }
        //         var pathOpenedValves = Enumerable.ToHashSet(openedValves);
        //         pathOpenedValves.Add(closedValve.Key);
        //         var next = shortestPath.Path.Dequeue();
        //         var pathTotalFlowGained = MaxRatePath(
        //             next,
        //             closedValve.Value,
        //             shortestPath.Path,
        //             ref pathOpenedValves,
        //             allValves,
        //             minute,
        //             maxMinutes,
        //             totalFlowGained);
        //
        //         if (pathTotalFlowGained > maxPossibleFlow)
        //         {
        //             maxPossibleFlow = pathTotalFlowGained;
        //             finalOpenedValves = pathOpenedValves;
        //         }
        //     }
        //
        //     openedValves = finalOpenedValves;
        //
        //     return maxPossibleFlow;
        // }
        //
        // private static (int Cost, Queue<Valve> Path) ShortestPath(Valve start, Valve end, HashSet<string> visitedValves, Dictionary<string, int> distances, Dictionary<string, Queue<Valve>> paths)
        // {
        //     if (start == end)
        //     {
        //         var x = new Queue<Valve>();
        //         x.Enqueue(start);
        //
        //         return (0, x);
        //     }
        //
        //     var toExamine = new Queue<Valve>();
        //     toExamine.Enqueue(start);
        //
        //     while (toExamine.TryDequeue(out var valve))
        //     {
        //         if (visitedValves.Contains(valve.Name)) continue;
        //
        //         foreach (var adjacent in valve.Neighbours)
        //         {
        //             // if (visitedValves.Contains(adjacent.Name))
        //             // {
        //             //     continue;
        //             // }
        //
        //             distances[adjacent.Name] = distances[valve.Name] + 1;
        //
        //             if (!paths.ContainsKey(adjacent.Name))
        //             {
        //                 paths[adjacent.Name] = paths.ContainsKey(valve.Name)
        //                     ? new Queue<Valve>(paths[valve.Name])
        //                     : new Queue<Valve>();
        //             }
        //
        //             if (!visitedValves.Contains(adjacent.Name) && paths[adjacent.Name].Count < distances[adjacent.Name])
        //             {
        //                 paths[adjacent.Name].Enqueue(valve);
        //             }
        //
        //             toExamine.Enqueue(adjacent);
        //         }
        //
        //         visitedValves.Add(valve.Name);
        //         if (valve != end) continue;
        //         paths[end.Name].Enqueue(end);
        //         if (paths[end.Name].Peek().Name == start.Name)
        //         {
        //             paths[end.Name].Dequeue();
        //         }
        //         return (distances[end.Name], paths[end.Name]);
        //     }
        //
        //     var path = new Queue<Valve>();
        //     path.Enqueue(start);
        //
        //     return (0, path);
        // }

        private Dictionary<string, Valve> GetValves(string input)
        {
            var valves = ParseValves(input).ToDictionary(a => a.Name, a => a);
            foreach (var valve in valves.Select(a => a.Value))
            {
                foreach (var valveTunnel in valve.TunnelLeadsTo)
                {
                    valve.Neighbours.Add(valves[valveTunnel]);
                }
            }

            return valves;
        }

        private List<Valve> ParseValves(string input)
        {
            var result = new List<Valve>();
            var lines = GetLinesInput(input);

            foreach (var line in lines)
            {
                var split = line.Split(
                    new[] { "Valve ", " has flow rate=", "; tunnels lead to valves ", "; tunnel leads to valve " },
                    StringSplitOptions.RemoveEmptyEntries);
                result.Add(new Valve(split[0], int.Parse(split[1]), split[2].Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList()));

            }

            return result;
        }

        public class Boyyy
        {
            public Boyyy(Valve start)
            {
                Start = start;
                PathToNextNode = new Queue<Valve>();
            }
            private Boyyy(Valve start, Valve nextNode, Queue<Valve> pathToNextNode, int flowGained, bool isOpening)
            {
                Start = start;
                NextNode = nextNode;
                PathToNextNode = pathToNextNode;
                FlowGained = flowGained;
                IsOpening = isOpening;
            }
            public Valve Start { get; set; }
            public Valve NextNode { get; set; }
            public Queue<Valve> PathToNextNode { get; set; }
            public int FlowGained { get; set; }
            public bool IsOpening { get; set; }

            public Boyyy CreateHypothetic()
            {
                return new Boyyy(Start, NextNode, new Queue<Valve>(PathToNextNode), FlowGained, IsOpening);
            }
        }

        public class Valve
        {
            public Valve(string name, int flowRate, List<string> tunnelLeadsTo)
            {
                Name = name;
                FlowRate = flowRate;
                TunnelLeadsTo = tunnelLeadsTo;
                Neighbours = new List<Valve>();
            }
            public string Name { get; set; }
            public int FlowRate { get; set; }
            public bool Open { get; set; }
            public List<string> TunnelLeadsTo { get; set; }
            public List<Valve> Neighbours { get; set; }
        }
    }
}
