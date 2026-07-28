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
        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
            return 0;

        source = source.ToLower().Trim();
        target = target.ToLower().Trim();
        
        if (target.Contains(source) || source.Contains(target))
            return 1.0;

      

        if (source.Length >= 2 && IsSubsequence(source, target))
            return 0.75; 
       
        double WholeScore()
        {
            double distance = LevenshteinDistanceChecker(source, target);
            int maxLength = Math.Max(source.Length, target.Length);
            return maxLength > 0 ? 1.0 - distance / maxLength : 1.0;
        }

        double best = WholeScore();

        //provision for additional characters and spaces
        var tokens = target.Split(new[] { ' ', '\t', ',', '.', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var token in tokens)
        {
            if (string.IsNullOrWhiteSpace(token)) continue;

            if (token.Contains(source) || source.Contains(token))
            {
                best = Math.Max(best, 1.0);
                continue;
            }
           
            if (source.Length >= 2 && IsSubsequence(source, token))
            {
                best = Math.Max(best, 0.75);
                continue;
            }

            double d = LevenshteinDistanceChecker(source, token);
            int maxLength = Math.Max(source.Length, token.Length);
            double score = maxLength > 0 ? 1.0 - d / maxLength : 1.0;
            best = Math.Max(best, score);
        }

        return best;
    }

    // found that if all query characters match in the serch term to one of the items, it should guarantee a good match
    private static bool IsSubsequence(string s, string t)
    {
        int i = 0, j = 0;
        while (i < s.Length && j < t.Length)
        {
            if (s[i] == t[j]) i++;
            j++;
        }
        return i == s.Length;
    }

    //Weighted scoring
    public async Task<IEnumerable<(Product Item, double Score)>> SearchAsync(string term)
    {
        var items = await _repository.GetAllAsync();
        var results = new List<(Product, double Score)>();

        var normalizedTerm = term.ToLower().Trim();

        foreach (var item in items)
        {
            double totalModelScore = 0;

            foreach (var kvp in _weights)
            {
                string value = kvp.Key(item)?.ToLower().Trim() ?? string.Empty;

                if(string.IsNullOrWhiteSpace(value))
                    continue;

                double similarity = FuzzyMatch(normalizedTerm, value);
                totalModelScore += similarity * kvp.Value;
            }
            if (totalModelScore > 0)
                results.Add((item, totalModelScore));
        }

        return results.OrderByDescending(r => r.Score);
    }
       


}
