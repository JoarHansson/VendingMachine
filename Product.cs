namespace VendingMachine;

public class Product
{
    public string Name { get; set; }
    public int Price { get; set; }
    public int ItemsInStock { get; set; }

    public Product(string name, int price, int itemsInStock)
    {
        Name = name;
        Price = price;
        ItemsInStock = itemsInStock;
    }
}