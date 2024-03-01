namespace VendingMachine;

public class User
{
    public string Name { get; set; }
    public int AccountBalance { get; set; }
    
    public Inventory Inventory { get; set; }  = new Inventory();
}