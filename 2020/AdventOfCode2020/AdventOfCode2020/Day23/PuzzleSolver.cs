using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day23
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "23";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new()
            {
                ("67384529", SolvePuzzle(await LoadTestInputAsync(1))),
                ("149245887792", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false, 100);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true, 10000000);
        }

        private string SolvePuzzle(string input, bool addToMillion, int numberOfMoves)
        {
            var cupBoard = new CubBoard();
            foreach (var number in input.Select(character => int.Parse(character.ToString())))
            {
                cupBoard.Add(new Cup { Label = number });
            }

            if (addToMillion)
            {
                for (var i = 10; i < 1000001; i++)
                {
                    cupBoard.Add(new Cup { Label = i });
                }
            }

            var currentCup = cupBoard.Head;
            var destinationCup = currentCup;

            for (var i = 0; i < numberOfMoves; i++)
            {
                var removed = new[] { currentCup.Next, currentCup.Next.Next, currentCup.Next.Next.Next };
                for (var j = currentCup.Label - 1; j > -1; j--)
                {
                    if (j == 0)
                    {
                        destinationCup = cupBoard.Cups.Where(
                                a => a.Key != currentCup.Label && removed.All(rem => rem.Label != a.Key))
                            .OrderByDescending(x => x.Key).First().Value;

                        break;
                    }

                    if (removed.All(a => a.Label != j))
                    {
                        destinationCup = cupBoard.Cups[j];
                        break;
                    }
                }

                currentCup.Next = currentCup.Next.Next.Next.Next;
                cupBoard.MoveCups(destinationCup.Label, removed);

                currentCup = currentCup.Next;
            }

            var current = cupBoard.Cups[1];

            if (!addToMillion)
            {
                var result = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    result += current.Next.Label;
                    current = current.Next;
                }

                return result;
            }

            long r = current.Next.Label * current.Next.Next.Label;
            return r.ToString();
        }

        public class CubBoard
        {
            public Cup Head { get; set; }
            private Cup _cursor;

            public readonly Dictionary<long, Cup> Cups = new();

            public void Add(Cup cup)
            {
                if (Head == null)
                {
                    Head = cup;
                    _cursor = cup;
                }
                else
                {
                    _cursor.Next = cup;
                    _cursor = cup;
                    _cursor.Next = Head;
                }

                Cups[cup.Label] = cup;
            }

            public void MoveCups(long afterLabel, Cup[] cups)
            {
                var originalSuccessor = Cups[afterLabel].Next;
                Cups[afterLabel].Next = cups[0];
                cups[2].Next = originalSuccessor;
            }
        }

        public class Cup
        {
            public long Label { get; set; }

            public Cup Next { get; set; }
        }
    }
}