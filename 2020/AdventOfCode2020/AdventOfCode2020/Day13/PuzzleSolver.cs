using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day13
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "13";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("295", SolvePuzzle(await LoadTestInputAsync(1))),
                //("29", SolvePuzzleExtended(await LoadTestInputAsync(2))),
                //("103", SolvePuzzleExtended(await LoadTestInputAsync(3))),
                ("1068781", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var lines = GetLinesInput(input);
            var earliest = int.Parse(lines[0]);
            var buses = lines[1].Split(',').Where(a => a != "x").Select(x => (int.Parse(x), ((earliest / int.Parse(x) + 1)) * int.Parse(x))).ToList();

            var (busId, offset) = buses.Where(a => a.Item2 >= earliest).OrderBy(a => a.Item2).First();

            return ((offset - earliest) * busId).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var lines = GetLinesInput(input);

            var schedule = lines[1].Split(',').ToArray();
            var busList = new List<(long BusId, long Offset)>();
            var offset = 0;

            foreach (var t in schedule)
            {
                if (t == "x")
                {
                    offset++;
                    continue;
                }

                var bus = int.Parse(t);

                busList.Add((bus, offset));
                offset++;
            }

            var buses = busList.ToArray();

           var result = CalculateSoonestTimestamp(buses);

           return result.ToString();
        }

        private long CalculateSoonestTimestamp((long BusId, long Offset)[] buses)
        {
            var maxOffset = buses.Select(a => a.Offset).Max();
            var lastModulo = buses[0].BusId;
            var lastRemainder = maxOffset - buses[0].Offset;

            for (var i = 1; i < buses.Length; i++)
            {
                var result = GetConguence(lastRemainder, lastModulo, maxOffset - buses[i].Offset, buses[i].BusId);
                lastRemainder = lastModulo * result + lastRemainder;
                lastModulo *= buses[i].BusId;
            }

            return lastRemainder - maxOffset;
        }

        private long GetConguence(long remainder1, long modulo1, long remainder2, long modulo2)
        {
            var rightSide = Modulo((remainder2 - remainder1), modulo2);
            
            var moduloInverse = GetInverseModulo(modulo1, modulo2);

            rightSide *= moduloInverse;

            return Modulo(rightSide, modulo2);
        }

        private long Modulo(long x, long m)
        {
            var r = x % m;
            return r < 0 ? r + m : r;
        }

        private long GetInverseModulo(long number, long modulo)
        {
            var t = 0l;
            var r = modulo;
            var newt = 1l;
            var newr = number;

            while (newr != 0)
            {
                var quotient = r / newr;
                var pomNewT = newt;
                newt = t - quotient * pomNewT;
                t = pomNewT;
                var pomNewR = newr;
                newr = r - quotient * pomNewR;
                r = pomNewR;
            }

            if (t < 0)
            {
                t = t + modulo;
            }

            return t;
        }
    }
}
