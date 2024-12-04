using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day16
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "16";

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
                ("6", SolvePuzzle(await LoadTestInputAsync(1))),
                ("16", SolvePuzzle(await LoadTestInputAsync(2))),
                ("12", SolvePuzzle(await LoadTestInputAsync(3))),
                ("23", SolvePuzzle(await LoadTestInputAsync(4))),
                ("31", SolvePuzzle(await LoadTestInputAsync(5))),
                ("3", SolvePuzzleExtended(await LoadTestInputAsync(6))),
                ("54", SolvePuzzleExtended(await LoadTestInputAsync(7))),
                ("1", SolvePuzzleExtended(await LoadTestInputAsync(8)))
            };
        }

        private string SolvePuzzle(string input, bool totalValue)
        {
            var binary = string.Join(string.Empty,
                input.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            var actualPosition = 0;
            var packets = new List<Packet>();

            var totalVersions = 0;

            packets.AddRange(ReadPackets(true, binary.Length - 1));

            List<Packet> ReadPackets(bool length, int to)
            {
                var subPackets = new List<Packet>();
                var times = 0;

                while (length  ? actualPosition < to : times < to)
                {
                    if (length && binary[actualPosition..to].All(x => x == '0'))
                    {
                        return subPackets;
                    }

                    times++;
                    var packet = new Packet
                    {
                        Version = Convert.ToInt32(binary[actualPosition..(actualPosition + 3)], 2),
                        PacketType = Convert.ToInt32(binary[(actualPosition + 3)..(actualPosition + 6)], 2)
                    };

                    if (packet.PacketType == 4)
                    {
                        var packetEnd = false;
                        var position = actualPosition + 6;
                        var resultValue = string.Empty;
                        while (!packetEnd)
                        {
                            if (binary[position] == '0')
                            {
                                packetEnd = true;
                                if (length ? position + 5 > to : position + 5 > binary.Length)
                                {
                                    resultValue += binary[(position + 1)..(to + 1)];
                                    position = to;
                                    continue;
                                }
                            }

                            resultValue += binary[(position + 1)..(position + 5)];
                            position += 5;
                        }

                        packet.Value = Convert.ToInt64(resultValue, 2);
                        actualPosition = position;
                    }
                    else
                    {
                        var shouldCheckLength = binary[actualPosition + 6] == '0';
                        var newPosition = shouldCheckLength ? actualPosition + 22 : actualPosition + 18;
                        var bitValue = Convert.ToInt32(binary[(actualPosition + 7)..(newPosition)], 2);
                        actualPosition = newPosition;
                        packet.SubPackets = ReadPackets(shouldCheckLength, shouldCheckLength ? actualPosition + bitValue : bitValue);
                    }

                    subPackets.Add(packet);
                    totalVersions += packet.Version;
                }

                return subPackets;
            }


            return totalValue ? packets.First().GetValue().ToString() : totalVersions.ToString();
        }

        private class Packet
        {
            public long Value { get; set; }
            public int Version { get; init; }
            public int PacketType { get; init; }
            public List<Packet> SubPackets { get; set; }

            public long GetValue()
            {
                return PacketType switch
                {
                    4 => Value,
                    0 => SubPackets.Sum(a => a.GetValue()),
                    1 => SubPackets.Select(a => a.GetValue()).Aggregate((a, b) => a * b),
                    2 => SubPackets.Min(a => a.GetValue()),
                    3 => SubPackets.Max(a => a.GetValue()),
                    5 => SubPackets.First().GetValue() > SubPackets.Last().GetValue() ? 1 : 0,
                    6 => SubPackets.First().GetValue() < SubPackets.Last().GetValue() ? 1 : 0,
                    7 => SubPackets.First().GetValue() == SubPackets.Last().GetValue() ? 1 : 0,
                    _ => throw new ArgumentException("Okay Morty, I may have fucked up here.")
                };
            }
        }
    }
}