using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace AdventOfCode2021.Day18
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "18";

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
                ("3488", SolvePuzzle(await LoadTestInputAsync(1))),
                ("4140", SolvePuzzle(await LoadTestInputAsync(2))),
                ("3993", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }

        private string SolvePuzzle(string input, bool highestTwoMagnitude)
        {
            var lines = GetLinesInput(input);

            if (!highestTwoMagnitude)
            {
                var fish = new SnailFish(lines[0]);
                for (var i = 1; i < lines.Length; i++)
                {
                    fish += new SnailFish(lines[i]);
                }

                return fish.GetMagnitude().ToString();
            }

            return
                Enumerable.Range(0, lines.Length - 1)
                    .Subsets(2)
                    .Select(a => new List<IEnumerable<int>>{a, a.Reverse()})
                    .SelectMany(a => a)
                    .Select(permutation => new SnailFish(lines[permutation.First()]) + new SnailFish(lines[permutation.Last()]))
                    .Select(snailFish => snailFish.GetMagnitude())
                    .Max()
                    .ToString();
        }

        public class SnailFish
        {
            private SnailFish _parent;
            private SnailFish _left;
            private SnailFish _right;
            private int _value;

            private bool IsLeaf => _left == null && _right == null;

            private SnailFish(SnailFish left, SnailFish right)
            {
                _left = left;
                _right = right;
                _left._parent = this;
                _right._parent = this;
                ApplyReductions();
            }

            private SnailFish(int value, SnailFish parent)
            {
                _parent = parent;
                _value = value;
            }

            public SnailFish(string input, SnailFish parent = null)
            {
                _parent = parent;
                var trimmed = input[1..^1];

                _left = trimmed[0] != '[' ? new SnailFish(int.Parse(trimmed.Split(",")[0]), this) : new SnailFish(trimmed[..FindPairEnd(trimmed, true)], this);
                _right = trimmed[^1] != ']' ? new SnailFish(int.Parse(trimmed.Split(",")[^1]), this) : new SnailFish(trimmed[FindPairEnd(trimmed, false)..], this);
            }

            public long GetMagnitude()
            {
                if (IsLeaf)
                {
                    return _value;
                }

                return _left.GetMagnitude() * 3 + _right.GetMagnitude() * 2;
            }

            public override string ToString()
            {
                return IsLeaf ? _value.ToString() : $"[{_left},{_right}]";
            }

            private void ApplyReductions()
            {
                var level = GetMaxLevel(this);
                var toReduce = GetFishToReduce(level > 4, this);

                while (toReduce != null)
                {
                    if (toReduce.IsLeaf)
                    {
                        toReduce.Split();
                    }
                    else
                    {
                        SnailFish currentFoundFish = null;
                        var leftFish = FindLeftFishToAdd(toReduce._left, this, ref currentFoundFish);
                        var fishFound = false;
                        var rightFish = FindRightFishToAdd(toReduce._right, this, ref fishFound);
                        if (leftFish != null)
                        {
                            leftFish._value += toReduce._left._value;
                        }

                        if (rightFish != null)
                        {
                            rightFish._value += toReduce._right._value;
                        }

                        toReduce._left = null;
                        toReduce._right = null;
                        toReduce._value = 0;
                    }

                    level = GetMaxLevel(this);

                    toReduce = GetFishToReduce(level > 4, this);
                }
            }

            private int GetLevel(SnailFish root)
            {
                return GetLevel(root, this, 0);
            }

            private SnailFish GetFishToReduce(bool checkLevel, SnailFish fish = null)
            {
                if (fish == null)
                {
                    return null;
                }

                switch (checkLevel)
                {
                    case true when !fish.IsLeaf && fish.GetLevel(this) == 4:
                        return fish;
                    case false when fish.IsLeaf && fish._value > 9:
                        return fish;
                    default:
                    {
                        var left = GetFishToReduce(checkLevel, fish._left);
                        return left ?? GetFishToReduce(checkLevel, fish._right);
                    }
                }
            }

            private int GetLevel(SnailFish actualFish, SnailFish fishToSearch, int level)
            {
                if (actualFish == null) return 0;
                if (actualFish == fishToSearch) return level;

                var foundLevel = GetLevel(actualFish._left, fishToSearch, level + 1);
                return foundLevel != 0 ? foundLevel : GetLevel(actualFish._right, fishToSearch, level + 1);
            }

            private int GetMaxLevel(SnailFish parent = null)
            {
                return parent == null ? -1 : Math.Max(GetMaxLevel(parent._left), GetMaxLevel(parent._right)) + 1;
            }

            private SnailFish FindLeftFishToAdd(SnailFish fishToAdd, SnailFish actualFish, ref SnailFish currentFoundFish)
            {
                if (actualFish == null) return null;
                if (fishToAdd == actualFish) return currentFoundFish;
                if (actualFish.IsLeaf) currentFoundFish = actualFish;

                var left = FindLeftFishToAdd(fishToAdd, actualFish._left, ref currentFoundFish);
                return left ?? FindLeftFishToAdd(fishToAdd, actualFish._right, ref currentFoundFish);
            }

            private SnailFish FindRightFishToAdd(SnailFish fishToAdd, SnailFish actualFish, ref bool fishFound)
            {
                if (actualFish == null) return null;
                if (fishToAdd == actualFish)
                {
                    fishFound = true;
                    return null;
                }

                if (fishFound && actualFish.IsLeaf) return actualFish;

                var left = FindRightFishToAdd(fishToAdd, actualFish._left, ref fishFound);
                return left ?? FindRightFishToAdd(fishToAdd, actualFish._right, ref fishFound);
            }

           private void Split()
            {
                _left = new SnailFish(_value / 2, this);
                _right = new SnailFish((_value / 2) + (_value % 2), this);
            }

            private static int FindPairEnd(string input, bool left)
            {
                var position = left ? -1 : input.Length;
                var opening = 0;
                var closing = 0;
                do
                {
                    position += left ? 1 : -1;
                    var actualChar = input[position];
                    switch (actualChar)
                    {
                        case '[':
                            opening++;
                            break;
                        case ']':
                            closing++;
                            break;
                    }
                } while (opening != closing);

                return left ? position + 1 : position;
            }

            public static SnailFish operator +(SnailFish a, SnailFish b)
            {
                return new SnailFish(a, b);
            }
        }
    }
}