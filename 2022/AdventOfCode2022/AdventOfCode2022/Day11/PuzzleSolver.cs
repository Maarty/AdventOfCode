using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Day11
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "11";

        protected override string SolvePuzzle(string input)
        {
            var monkeys = GetMonkeys(input);

            for (var i = 0; i < 20; i++)
            {
                DoRound();
            }

            var bestMonkeys = monkeys.Values.OrderByDescending(a => a.TotalInspections).Take(2).ToList();

            return (bestMonkeys.First().TotalInspections * bestMonkeys.Last().TotalInspections).ToString();

            void DoRound()
            {
                foreach (var monkey in monkeys.Values)
                {
                    while (monkey.Items.TryDequeue(out var item))
                    {
                        monkey.TotalInspections++;
                        item = monkey.Operation(item);
                        item = item / 3;
                        if (monkey.Test(item))
                        {
                            monkeys[monkey.TrueMonkey].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkey].Items.Enqueue(item);
                        }
                    }
                }
            }
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var monkeys = GetMonkeys(input);

            var lcm = FindLCM(monkeys.Values.Select(a => a.Divisor).ToArray(), monkeys.Count);

            for (var i = 0; i < 10000; i++)
            {
                DoRound();
            }

            var bestMonkeys = monkeys.Values.OrderByDescending(a => a.TotalInspections).Take(2).ToList();

            return (bestMonkeys.First().TotalInspections * bestMonkeys.Last().TotalInspections).ToString();

            void DoRound()
            {
                foreach (var monkey in monkeys.Values)
                {
                    while (monkey.Items.TryDequeue(out var item))
                    {
                        monkey.TotalInspections++;
                        item = monkey.Operation(item);
                        item %= lcm;
                        if (monkey.Test(item))
                        {
                            monkeys[monkey.TrueMonkey].Items.Enqueue(item);
                        }
                        else
                        {
                            monkeys[monkey.FalseMonkey].Items.Enqueue(item);
                        }
                    }
                }
            }
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("10605", SolvePuzzle(await LoadTestInputAsync(1))),
                ("2713310158", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        private Dictionary<int, Monkey> GetMonkeys(string input)
        {
            var monkeys = new Dictionary<int, Monkey>();
            var monkeysInput = input.Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var monkeyInput in monkeysInput)
            {
                var lines = GetLinesInput(monkeyInput);
                var monkey = new Monkey
                {
                    Id = int.Parse(lines[0][7].ToString())
                };
                var startingItems = lines[1].Replace("  Starting items: ", "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
                foreach (var startingItem in startingItems)
                {
                    monkey.Items.Enqueue(startingItem);
                }

                var divider = int.Parse(lines[3].Replace("  Test: divisible by ", ""));

                monkey.Test = i => i % divider == 0;
                monkey.Divisor = divider;

                var operation = lines[2].Replace("  Operation: new = ", "").Split(" ",StringSplitOptions.RemoveEmptyEntries);
                monkey.Operation = i =>
                {
                    var left = operation[0] == "old" ? i : int.Parse(operation[0]);
                    var right = operation[2] == "old" ? i : int.Parse(operation[2]);
                    return operation[1] == "+" ? left + right : left * right;
                };


                monkey.TrueMonkey = int.Parse(lines[4][29].ToString());
                monkey.FalseMonkey = int.Parse(lines[5][30].ToString());

                monkeys[monkey.Id] = monkey;
            }

            return monkeys;
        }

        private static int FindLCM(int[] arr, int n)
        {
            var result = arr[0];
            for (var i = 1; i < n; i++)
            {
                result = GetLCM(arr[i], result);
            }
            return result;
        }

        private static int GetLCM(int a, int b)
        {
            return (a * b) / GetGCD(a, b);
        }

        private static int GetGCD(int a, int b)
        {
            return a == 0 ? b : GetGCD(b % a, a);
        }
    }

    public class Monkey
    {
        public Monkey()
        {
            Items = new Queue<long>();
        }
        public int Id { get; set; }
        public Queue<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public Func<long, bool> Test { get; set; }
        public Func<long, long> GetRemainderItem { get; set; }
        public int Divisor { get; set; }
        public int TrueMonkey { get; set; }
        public int FalseMonkey { get; set; }
        public long TotalInspections { get; set; }
    }
}