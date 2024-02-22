namespace VendingMachine;

public class Inventory
{
    public List<Product> Products { get; }
    
    public Inventory()
    {
        Products = new List<Product>();
    }
    
    public void AddItem(Product product)
    {
        Products.Add(product);
    }
}
