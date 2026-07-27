using PCMS_Backend.Models;
using PCMS_Backend.Repositories;

namespace PCMS_Backend.Services;

//Implemented generic addition so that this can be used for other models or future models
public class ProductSearchEngine
{
    private readonly IRepository<Product> _repository;
    private readonly Dictionary<Func<Product, string>, double> _weights;

    public ProductSearchEngine(Dictionary<Func<Product, string>, double> weights, IRepository<Product> repository)
    {
        _repository = repository;
        _weights = weights;
    }


    //Checking distance from one strings transformation to antoher
    private static double LevenshteinDistanceChecker(string source, string target)
    {
        ///Checks lengths and if source is shorter than target swap the variables
        if (source.Length < target.Length)
        {
            string temp = source;
            source = target;
            target = temp;
        }

        if (target.Length == 0)
            return source.Length;

        int[] previousRow = new int[target.Length + 1];
        for (int i = 0; i <= target.Length; i++)          
            previousRow[i] = i;

        for (int i = 0; i < source.Length; i++)
        {
            int[] currentRow = new int[target.Length + 1];
            currentRow[0] = i + 1;

            for (int j = 0; j < target.Length; j++)
            {
                int cost = (source[i] == target[j]) ? 0 : 1;

                int insertion = previousRow[j + 1] + 1;
                int deletion = currentRow[j] + 1;
                int substitution = previousRow[j] + cost;

                currentRow[j + 1] = Math.Min(Math.Min(insertion, deletion), substitution);
            }
            previousRow = currentRow;
        }

        return previousRow[target.Length];
    }

    public static double FuzzyMatch(string source, string target)
    {
        //both strings empty
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
            return 0;

        double distance = LevenshteinDistanceChecker(source, target);
        int maxLength = Math.Max(source.Length, target.Length);
        
        return maxLength > 0 ? 1.0 - distance / maxLength : 1.0;
    }

    //Weighted scoring
    public async Task<IEnumerable<(Product Item, double Score)>> SearchAsync(string term)
    {
        var items = await _repository.GetAllAsync();
        var results = new List<(Product, double Score)>();

        foreach (var item in items)
        {
            double totalModelScore = 0;

            foreach (var kvp in _weights)
            {
                string value = kvp.Key(item) ?? string.Empty;

                if(string.IsNullOrWhiteSpace(value))
                    continue;

                double similarity = FuzzyMatch(term, value);
                totalModelScore += similarity * kvp.Value;
            }
            if (totalModelScore > 0)
                results.Add((item, totalModelScore));
        }

        return results.OrderByDescending(r => r.Score);
    }
       


}
