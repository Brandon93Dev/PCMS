using PCMS_Backend.Models;

namespace PCMS_Backend.Extensions
{
    public class SearchWeightConfig
    {
        public static readonly Dictionary<Func<Product, string?>, double> ProductWeights =
            new()
            {
                { p => p.Name, 0.6 },
                { p => p.Description, 0.3 },
                { p => p.SKU, 0.1 }
            };

        public static readonly Dictionary<Func<Category, string?>, double> CategoryWeights =
            new()
            {
                { c => c.Name, 0.7 },
                { c => c.Description, 0.3 }
            };
    }
}
