using System;

namespace AdventOfCode2024
{
    public class PuzzleResult
    {
        public PuzzleResult(string result, TimeSpan timeElapsed)
        {
            Result = result;
            TimeElapsed = timeElapsed;
        }

        public string Result { get; set; }

        public TimeSpan TimeElapsed { get; set; }
    }
}
