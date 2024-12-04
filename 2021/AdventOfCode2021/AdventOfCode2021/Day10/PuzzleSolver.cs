using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day10
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "10";

        private Dictionary<char, char> ChunkPairs = new Dictionary<char, char>
            { { '<', '>' }, { '(', ')' }, { '{', '}' }, { '[', ']' } };

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
                ("288957", SolvePuzzle(await LoadTestInputAsync(1))),
               // ("5", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }

        private string SolvePuzzle(string input, bool sth)
        {
            var lines = GetLinesInput(input);

            var result = 0;

            var scores = new List<long>();

            foreach (var line in lines)
            {
                var incompleteChars = new List<char>();
                for (int i = 0; i < line.Length; i++)
                {
                    var chunk = ReadChunk(line, i, incompleteChars);

                    if (chunk.Valid && chunk.End < line.Length)
                    {
                        i = chunk.End + 1;
                        continue;
                    }
                    else if (!chunk.Valid && chunk.End < line.Length)
                    {
                        switch (line[chunk.End])
                        {
                            case ')': result += 3;
                                break;
                            case ']': result += 57;
                                break;
                            case '}': result += 1197;
                                break;
                            case '>': result += 25137;
                                break;
                        }

                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                long total = 0;

                foreach (var incompleteChar in incompleteChars)
                {
                    total *= 5;
                    switch (incompleteChar)
                    {
                        case ')': total += 1;
                            break;
                        case ']': total += 2;
                            break;
                        case '}': total += 3;
                            break;
                        case '>': total += 4;
                            break;
                    }

                }

                if (total > 0)
                {
                    scores.Add(total);
                }

            }

            var res = 0;

            scores = scores.OrderBy(a => a).ToList();

            return scores.ElementAt(scores.Count / 2).ToString();
        }

        private (string Chunk, int End, bool Valid, bool IsComplete) ReadChunk(string line, int position, List<char> incompleteCharacters)
        {
            var opening = line[position];
            var chunk = "" + opening;


            while (true)
            {
                position += 1;
                if (position >= line.Length)
                {
                    incompleteCharacters.Add(ChunkPairs[opening]);
                    return (chunk, position, true, false);
                }
                var next = line[position];

                if (IsClosing(next))
                {
                    chunk += next;
                    if (ChunkPairs[opening] == next)
                    {
                        return (chunk, position, true, true);
                    }
                    else
                    {
                        return (chunk, position, false, true);
                    }
                }

                var innerChunk = ReadChunk(line, position, incompleteCharacters);

                if (!innerChunk.Valid)
                {
                    position = innerChunk.End;
                    return (chunk, position, false, true);
                }

                position = innerChunk.End;
                chunk += innerChunk.Chunk;
            }
        }

        private bool IsClosing(char character)
        {
            return ChunkPairs.Values.Any(a => a == character);
        }
    }
}