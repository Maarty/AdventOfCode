using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023.Day02
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "02";

        protected override string SolvePuzzle(string input)
        {
            var games = GetLinesInput(input).Select(ParseGame);

            return games.Where(a => a.Rounds.All(r => r.Red <= 12 && r.Green <= 13 && r.Blue <= 14))
                .Sum(a => a.Id).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var games = GetLinesInput(input).Select(ParseGame);


            return games.Select(a => a.Rounds.Max(x => x.Red) * a.Rounds.Max(x => x.Green) * a.Rounds.Max(x => x.Blue))
                .Sum().ToString();
        }

        private Game ParseGame(string line)
        {
            var split = line.Split(":");

            var game = new Game{ Id = int.Parse(split[0].Replace("Game ", string.Empty)) };

            var rounds = split[1].Split("; ");

            foreach (var round in rounds)
            {
                var r = new Round();
                var colors = round.Split(", ");
                foreach (var color in colors)
                {
                    var number = int.Parse(Regex.Match(color, @"\d+").Value);
                    if (color.Contains("blue"))
                    {
                        r.Blue = number;
                    }
                    if (color.Contains("red"))
                    {
                        r.Red = number;
                    }
                    if (color.Contains("green"))
                    {
                        r.Green = number;
                    }
                }

                game.Rounds.Add(r);
            }

            return game;
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("8", SolvePuzzle(await LoadTestInputAsync(1))),
                ("2286", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        public class Game
        {
            public int Id { get; set; }
            public List<Round> Rounds { get; } = new();
        }

        public class Round
        {
            public int Blue { get; set; }
            public int Red { get; set; }
            public int Green { get; set; }
        }
    }
}