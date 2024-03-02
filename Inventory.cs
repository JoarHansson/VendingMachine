namespace VendingMachine;

public class Inventory
{
    public List<Product> Products { get; } = new List<Product>();
    
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
    
    public List<Quote> Quotes { get; } = new List<Quote>();
    
    public void AddQuote(Quote quote)
    {
        Quotes.Add(quote);
    }
}
