using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day13
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "13";

        protected override string SolvePuzzle(string input)
        {
            var pairs = ParsePairs(input);
            var i = 0;
            var correctPairs = new List<int>();

            foreach (var pair in pairs)
            {
                i++;
                var correct = ComparePairs(pair);

                if (correct)
                {
                    correctPairs.Add(i);
                }
            }

            return correctPairs.Sum().ToString();
        }

        private static bool ComparePairs((Item, Item) pair)
        {
            var currentLeft = pair.Item1;
            var currentRight = pair.Item2;

            while (currentLeft != null)
            {
                currentLeft.Initialize();
                currentRight.Initialize();
                if (currentLeft.Value.HasValue && currentRight.Value.HasValue)
                {
                    if (currentLeft.Value < currentRight.Value)
                    {
                        return true;
                    }

                    if (currentLeft.Value > currentRight.Value)
                    {
                        return false;
                    }

                    currentLeft = currentLeft.Parent;
                    currentRight = currentRight.Parent;
                    currentLeft.CurrentInspectedItem++;
                    currentRight.CurrentInspectedItem++;
                    continue;
                }

                if (currentLeft.Value.HasValue)
                {
                    currentLeft = WrapItem(currentLeft);
                    continue;
                }

                if (currentRight.Value.HasValue)
                {
                    currentRight = WrapItem(currentRight);
                    continue;
                }

                if (currentLeft.CurrentInspectedItem == currentLeft.Items.Length ||
                    currentRight.CurrentInspectedItem == currentRight.Items.Length)
                {
                    if (currentLeft.Items.Length > currentRight.Items.Length)
                    {
                        return false;
                    }

                    if (currentLeft.Items.Length < currentRight.Items.Length)
                    {
                        return true;
                    }

                    currentLeft = currentLeft.Parent;
                    currentRight = currentRight.Parent;
                    currentLeft.CurrentInspectedItem++;
                    currentRight.CurrentInspectedItem++;

                    if (currentLeft.Items.Length == currentLeft.CurrentInspectedItem)
                    {
                        continue;
                    }

                    if (currentRight.CurrentInspectedItem == currentRight.Items.Length)
                    {
                        return false;
                    }
                }

                currentLeft = currentLeft.Items[currentLeft.CurrentInspectedItem];
                currentRight = currentRight.Items[currentRight.CurrentInspectedItem];
            }

            return true;
        }

        private static Item WrapItem(Item currentItem)
        {
            var k = new Item(currentItem.Parent);
            currentItem.Parent = k;
            k.ItemsList = new List<Item> { currentItem };
            currentItem = k;
            return currentItem;
        }

        protected override string SolvePuzzleExtended(string input)
        {
            input = "[[2]]\r\n[[6]]\r\n\r\n" + input;
             var items = ParsePairs(input).SelectMany(x => new List<Item>{ x.Item1, x.Item2}).ToList();

             BubbleSortLikeABoss(items);

             var keys = items.Where(a => a.IsKey);

             return ((items.IndexOf(keys.First()) + 1) * (items.IndexOf(keys.Last()) + 1)).ToString();
        }

        private void BubbleSortLikeABoss(List<Item> list)
        {
            for (var i = 1; i < list.Count; i++)
            {
                for (var j = 0; j < list.Count - i; j++)
                {
                    if (!ComparePairs((list[j], list[j+1])))
                    {
                        (list[j], list[j+1]) = (list[j+1], list[j]);
                    }
                }
            }
        }

        private List<(Item, Item)> ParsePairs(string input)
        {
            var result = new List<(Item, Item)>();
            var pairsString = input.Split(
                Environment.NewLine + Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var pairString in pairsString)
            {
                var pairs = pairString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                result.Add((ParsePair(pairs[0]), ParsePair(pairs[1])));
            }

            return result;
        }

        private Item ParsePair(string input)
        {
            var item = new Item(null);
            var currentItem = item;
            if (input == "[[2]]" || input == "[[6]]")
            {
                item.IsKey = true;
            }
            for (var i = 1; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '[':
                        var newItem = new Item(currentItem);
                        currentItem.ItemsList.Add(newItem);
                        currentItem = newItem;
                        continue;
                    case ']':
                        currentItem = currentItem.Parent;
                        continue;
                    case ',':
                        continue;
                }

                var valueItem = new Item(currentItem)
                {
                    Value = int.Parse(char.IsDigit(input[i+1]) ? input[i..(i + 2)] : input[i].ToString())
                };

                if (char.IsDigit(input[i+1]))
                {
                    i++;
                }

                currentItem.ItemsList.Add(valueItem);
            }

            return item;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("13", SolvePuzzle(await LoadTestInputAsync(1))),
                ("140", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        public class Item
        {
            public Item(Item parent)
            {
                Parent = parent;
            }

            public bool IsKey { get; set; }
            public Item Parent { get; set; }
            public Item[] Items { get; set; }
            public List<Item> ItemsList { get; set; } = new List<Item>();

            public int? Value { get; set; }
            public int CurrentInspectedItem { get; set; }

            public void Initialize()
            {
                Items ??= ItemsList.ToArray();
            }
        }
    }
}