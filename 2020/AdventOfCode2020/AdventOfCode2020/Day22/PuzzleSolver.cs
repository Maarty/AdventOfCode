using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day22
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "22";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("306", SolvePuzzle(await LoadTestInputAsync(1))),
                ("291", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        protected override string SolvePuzzle(string input)
        {
            return SolvePuzzle(input, false);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolvePuzzle(input, true);
        }

        private string SolvePuzzle(string input, bool recursiveCombat)
        {
            var decks = input.Split(Environment.NewLine + Environment.NewLine)
                .Select(a => new Queue<int>(a.Split(Environment.NewLine)[1..].Select(int.Parse)))
                .ToArray();

            PlayGame(decks);

            var res = 0;
            var winnerCrab = new Queue<int>(decks.First(a => a.Count > 0).Reverse());

            var i = 1;
            while (winnerCrab.Count > 0)
            {
                res += i * winnerCrab.Dequeue();
                i++;
            }

            return res.ToString();

            int PlayGame(Queue<int>[] newDecks)
            {
                var startingCombos = new HashSet<string>();

                while (newDecks.All(a => a.Count != 0))
                {
                    var starting = string.Join(",", newDecks[0]) + "+" + string.Join(",", newDecks[1]);

                    if (startingCombos.Contains(starting))
                    {
                        return 0;
                    }

                    startingCombos.Add(starting);

                    var first = newDecks[0].Dequeue();
                    var second = newDecks[1].Dequeue();

                    var winner = first > second ? 0 : 1;

                    if (recursiveCombat && newDecks[0].Count >= first && newDecks[1].Count >= second)
                    {
                        winner = PlayGame(new[] { new Queue<int>(newDecks[0].Take(first)), new Queue<int>(newDecks[1].Take(second)) });
                    }

                    var winning = winner == 0 ? first : second;
                    var losing = winner == 0 ? second : first;

                    newDecks[winner].Enqueue(winning);
                    newDecks[winner].Enqueue(losing);
                }

                return newDecks[0].Count == 0 ? 1 : 0;
            }
        }
    }
}