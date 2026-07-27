namespace PCMS_Backend.Models;

//inheriting from ICompareble to enable sorting of products
public class Product : IComparable<Product>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; } 
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime ModifiedAt { get; set; } = DateTime.Now;

    public int CategoryId { get; set; }

    // Implement IComparable for custom sorting
    public int CompareTo(Product? other)
    {
        if (other is null) return 1;
        return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }
}

