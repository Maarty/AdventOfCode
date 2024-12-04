namespace AdventOfCode2024.Day03
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "03";

        protected override string SolvePuzzle(string input)
        {
            var result = 0;
            var sequence = "mul(X,Y)";
            var instructionPosition = 0;
            var currentX = string.Empty;
            var currentY = string.Empty;

            foreach (var character in input)
            {
                switch (sequence[instructionPosition])
                {
                    case 'X':
                        if (char.IsDigit(character))
                        {
                            currentX += character;
                        }
                        else if (character == ',')
                        {
                            instructionPosition += 2;
                        }
                        else
                        {
                            Reset();
                        }
                        break;
                    case 'Y':
                        if (char.IsDigit(character))
                        {
                            currentY += character;
                        }
                        else if (character == ')')
                        {
                            result += int.Parse(currentY) * int.Parse(currentX);
                            Reset();
                        }
                        else
                        {
                            Reset();
                        }
                        break;
                    default:
                        if (character == sequence[instructionPosition])
                        {
                            instructionPosition++;
                        }
                        else
                        {
                            Reset();
                        }
                        break;

                }

                void Reset()
                {
                    instructionPosition = 0;
                    currentX = string.Empty;
                    currentY = string.Empty;
                }
            }

            return result.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            long result = 0;
            var sequence = "mul(X,Y)";
            var instructionPosition = 0;
            var currentX = string.Empty;
            var currentY = string.Empty;
            var active = true;

            for (int i = 0; i < input.Length; i++)
            {
                var character = input[i];
                if (character == 'd')
                {
                    if (input[i..(i+4)] == "do()")
                    {
                        active = true;
                        i += 3;
                        Reset();
                        continue;
                    }

                    if (input[i..(i+7)] == "don't()")
                    {
                        active = false;
                        i += 6;
                        Reset();
                        continue;
                    }
                }

                if (!active)
                {
                    Reset();
                    continue;
                }
                switch (sequence[instructionPosition])
                {
                    case 'X':
                        if (char.IsDigit(character))
                        {
                            currentX += character;
                        }
                        else if (character == ',')
                        {
                            instructionPosition += 2;
                        }
                        else
                        {
                            Reset();
                        }
                        break;
                    case 'Y':
                        if (char.IsDigit(character))
                        {
                            currentY += character;
                        }
                        else if (character == ')')
                        {
                            result += long.Parse(currentY) * long.Parse(currentX);
                            Reset();
                        }
                        else
                        {
                            Reset();
                        }
                        break;
                    default:
                        if (character == sequence[instructionPosition])
                        {
                            instructionPosition++;
                        }
                        else
                        {
                            Reset();
                        }
                        break;

                }

                void Reset()
                {
                    instructionPosition = 0;
                    currentX = string.Empty;
                    currentY = string.Empty;
                }
            }

            return result.ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("161", SolvePuzzle(await LoadTestInputAsync(1))),
                ("48", SolvePuzzleExtended(await LoadTestInputAsync(2)))
            };
        }
    }
}