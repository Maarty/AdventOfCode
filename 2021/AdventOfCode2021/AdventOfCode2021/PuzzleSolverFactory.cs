using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2021
{
    public class PuzzleSolverFactory
    {
        public IPuzzleSolver CreatePuzzleSolver(string dayNumber)
        {
            var solvers = LoadPuzzleSolvers();

            return solvers.First(a => a.Day == dayNumber);
        }

        private static List<IPuzzleSolver> LoadPuzzleSolvers()
        {
            var plugins = new List<IPuzzleSolver>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetExportedTypes())
            {
                if (!typeof(IPuzzleSolver).IsAssignableFrom(type) || type.IsInterface || type.IsAbstract) continue;

                var solver = (IPuzzleSolver)Activator.CreateInstance(type);
                plugins.Add(solver);
            }
            return plugins;
        }
    }
}
