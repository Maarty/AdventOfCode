using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day07
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "07";

        protected override string SolvePuzzle(string input)
        {
            var root = GetRootDirectory(input);

            var allDirectories = Directory.GetAllDirectories(root);

            return allDirectories.Where(a => a.GetSize() <= 100000).Sum(a => a.GetSize()).ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            var root = GetRootDirectory(input);

            var allDirectories = Directory.GetAllDirectories(root);

            var unusedSize = 70000000 - root.GetSize();
            var limitSize = 30000000 - unusedSize;

            return allDirectories.Where(a => a.GetSize() > limitSize).OrderBy(a => a.GetSize()).First().GetSize().ToString();
        }

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("95437", SolvePuzzle(await LoadTestInputAsync(1))),
                ("24933642", SolvePuzzleExtended(await LoadTestInputAsync(1)))
            };
        }


        private Directory GetRootDirectory(string input)
        {
            var lines = GetLinesInput(input);

            var root = new Directory("/", null);
            var actualDirectory = root;

            foreach (var line in lines)
            {
                var command = GetCommand(line);

                if (command != null)
                {
                    if (command.Name == "cd")
                    {
                        switch (command.Value)
                        {
                            case "/":
                                continue;
                            case "..":
                                actualDirectory = actualDirectory._parent;
                                continue;
                            default:
                                actualDirectory = actualDirectory.GetChild(command.Value);
                                break;
                        }
                    }
                }
                else
                {
                    var split = line.Split(" ");
                    if (split[0].Equals("dir"))
                    {
                        actualDirectory.AddChild(new Directory(split[1], actualDirectory));
                    }
                    else
                    {
                        actualDirectory.AddFile(new File(split[1], int.Parse(split[0])));
                    }
                }
            }

            return root;
        }

        private static Command GetCommand(string input)
        {
            if (!input.StartsWith("$")) return null;

            var split = input.Split(" ");

            return new Command(split[1], split.Length > 2 ? split[2] : string.Empty);
        }

        private record Command(string Name, string Value);

        private sealed class Directory
        {
            private readonly string _name;
            private readonly List<File> _files;
            private readonly LinkedList<Directory> _children;
            public readonly Directory _parent;

            public Directory(string name, Directory parent)
            {
                _name = name;
                _parent = parent;
                _files = new List<File>();
                _children = new LinkedList<Directory>();
            }

            public void AddChild(Directory directory)
            {
                _children.AddFirst(directory);
            }

            public void AddFile(File file)
            {
                _files.Add(file);
            }

            public long GetSize()
            {
                return _children.Sum(a => a.GetSize()) + _files.Sum(a => a.Size);
            }

            public Directory GetChild(string name)
            {
                return _children.First(a => a._name == name);
            }

            public static HashSet<Directory> GetAllDirectories(Directory node)
            {
                var result = new HashSet<Directory> { node };
                foreach (var dirs in node._children.Select(GetAllDirectories))
                {
                    result.UnionWith(dirs);
                }

                return result;
            }
        }

        private record File(string Name, long Size);
    }
}