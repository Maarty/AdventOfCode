namespace AdventOfCode2024.Day09
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "09";

        protected override string SolvePuzzle(string input)
        {
            return Solve(input);
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return SolveExt(input);
        }

        private string Solve(string input)
        {
           ulong result = 0;

            var disk = LoadDisk(input).ToArray();

            var endIndex = disk.Length;

            for (var i = 0; i < endIndex; i++)
            {
                if (disk[i] >= 0) continue;

                while (true)
                {
                    if (endIndex <= i)
                    {
                        break;
                    }
                    endIndex--;
                    if (disk[endIndex] <= 0) continue;

                    disk[i] = disk[endIndex];
                    disk[endIndex] = -1;
                    break;
                }
            }

            var index = 0;

            //PrintDisk(disk);

            foreach (var block in disk)
            {
                if (block == -1)
                {
                    break;
                }
                result += (ulong)(block * index);
                index++;
            }

            return result.ToString();
        }

        private string SolveExt(string input)
        {
            ulong result = 0;

            var disk = LoadFragmentedDisk(input);

            var toFit = disk.Last;
            var lastSpace = disk.First.Next;

            while (true)
            {
                //PrintFragmentedDisk(disk);
                if (toFit.Value.Id < 0)
                {
                    toFit = toFit.Previous;
                    continue;
                }

                var move = true;

                var currentSpace = lastSpace;

                while (true)
                {
                    if (currentSpace == null || toFit == currentSpace || currentSpace.Value.Index >= toFit.Value.Index)
                    {
                        break;
                    }
                    if (currentSpace.Value.Id >= 0)
                    {
                        currentSpace = currentSpace.Next;
                        continue;
                    }
                    if (toFit.Value.Size <= currentSpace.Value.Size)
                    {
                        disk.AddBefore(currentSpace, new Fragment(toFit.Value.Id, currentSpace.Value.Index, toFit.Value.Size));
                        currentSpace.Value.Index += toFit.Value.Size;
                        if (toFit.Value.Size == currentSpace.Value.Size)
                        {
                            if (lastSpace == currentSpace)
                            {
                                lastSpace = currentSpace.Next;
                            }
                            disk.Remove(currentSpace);
                        }
                        else
                        {
                            currentSpace.Value.Size -= toFit.Value.Size;
                        }
                        var newToFit = toFit.Previous;
                        toFit.Value.Id = -1;
                        disk.Remove(toFit);
                        toFit = newToFit;
                        move = false;
                        break;
                    }

                    currentSpace = currentSpace.Next;
                }

                if (toFit.Previous == null)
                {
                    break;
                }

                if (move)
                {
                    toFit = toFit.Previous;
                }
            }

            //PrintFragmentedDisk(disk);

            foreach (var block in disk)
            {
                if (block.Id == -1)
                {
                    continue;
                }

                for (var i = 0; i < block.Size; i++)
                {
                    result += (ulong)(block.Id * (block.Index + i));
                }
            }

            return result.ToString();
        }

        private static List<int> LoadDisk(string input)
        {
            var blockId = 0;
            var disk = new List<int>();
            var isBlock = true;
            foreach (var blockCount in input.Select(block => int.Parse(block.ToString())))
            {
                if (isBlock)
                {
                    AddBlock(blockCount, blockId);
                    blockId++;
                }
                else
                {
                    AddBlock(blockCount, -1);
                }

                isBlock = !isBlock;
            }

            return disk;

            void AddBlock(int blockCount, int blockId1)
            {
                for (var i = 0; i < blockCount; i++)
                {
                    disk.Add(blockId1);
                }
            }
        }

        private static LinkedList<Fragment> LoadFragmentedDisk(string input)
        {
            var blockId = 0;
            var disk = new LinkedList<Fragment>();
            var isBlock = true;
            var index = 0;
            foreach (var blockCount in input.Select(block => int.Parse(block.ToString())))
            {
                if (isBlock)
                {
                    disk.AddLast(new Fragment(blockId, index, blockCount));
                    blockId++;
                }
                else
                {
                    var empty = new Fragment(-1, index, blockCount);
                    disk.AddLast(empty);
                }

                index += blockCount;
                isBlock = !isBlock;
            }

            return disk;
        }

        private static void PrintDisk(int[] disk)
        {
            foreach (var l in disk)
            {
                Console.Write(l >= 0 ? l.ToString() : ".");
            }
        }

        private static void PrintFragmentedDisk(LinkedList<Fragment> disk)
        {
            foreach (var l in disk)
            {
                for (var i = 0; i < l.Size; i++)
                {
                    Console.Write(l.Id >= 0 ? l.Id.ToString() : ".");
                }
            }
            Console.WriteLine();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return
            [
                ("1928", SolvePuzzle(await LoadTestInputAsync(1))),
                ("2858", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            ];
        }
    }

    public class Fragment(int id, int index, int size)
    {
        public int Id { get; set; } = id;
        public int Index { get; set; } = index;
        public int Size { get; set; } = size;
    }
}