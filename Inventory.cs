namespace VendingMachine;

public class Inventory
{
    public Dictionary<Product, int> Products { get; } = new Dictionary<Product, int>();
    
    public void AddItem(Product product, int numberInStock)
    {
        Products.Add(product, numberInStock);
    }
}
