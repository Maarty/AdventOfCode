using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day21
{
    public class PuzzleSolver : PuzzleSolverBase
    {
        public override string Day => "21";

        public override async ValueTask<List<(string Expected, string Actual)>> SolveTestsAsync()
        {
            return new List<(string Expected, string Actual)>
            {
                ("5", SolvePuzzle(await LoadTestInputAsync(1))),
                ("mxmxvkd,sqjhc,fvjkl", SolvePuzzleExtended(await LoadTestInputAsync(1))),
            };
        }

        protected override string SolvePuzzle(string input)
        {
            var count = 0;

            var foods = DetermineAllergens(input);

            foreach (var ingredient in foods.AllIngredients)
            {
                if (!foods.Allergens.Contains(ingredient))
                {
                    count++;
                }
            }

            return count.ToString();
        }

        protected override string SolvePuzzleExtended(string input)
        {
            return string.Join(",", DetermineAllergens(input).Allergens);
        }

        private (List< string> Allergens, List<string> AllIngredients) DetermineAllergens(string input)
        {
            var lines = GetLinesInput(input);

            var allergens = new Dictionary<string, List<string>>();
            var allIngredients = new List<string>();

            foreach (var line in lines)
            {
                var split = line.Split(" (contains ");

                var ingredients = split[0].Split(" ").ToList();
                var containedAllergens = split[1].Replace(")", "").Split(", ").ToList();

                allIngredients.AddRange(ingredients);

                foreach (var allergen in containedAllergens)
                {
                    if (!allergens.ContainsKey(allergen))
                    {
                        allergens[allergen] = new List<string>(ingredients);
                    }
                    else
                    {
                        allergens[allergen].RemoveAll(x => !ingredients.Contains(x));
                    }
                }
            }

            var singleAllergens = allergens.Where(a => a.Value.Count == 1).ToList();

            while (singleAllergens.Count < allergens.Count)
            {
                foreach (var (allergenName, ingredients) in singleAllergens)
                {
                    foreach (var allergen in allergens.Where(a => a.Key != allergenName))
                    {
                        allergen.Value.RemoveAll(x => x == ingredients.First());
                    }
                }

                singleAllergens = allergens.Where(a => a.Value.Count == 1).ToList();
            }

            var allAllergens = allergens.OrderBy(a => a.Key).SelectMany(a => a.Value).ToList();

            return (allAllergens, allIngredients);
        }
    }
}