using VendingMachine;

var random = new Random();

var inventory = new Inventory();

inventory.AddItem(new Product("Tomato", 12));
inventory.AddItem(new Product("Avocado", 34));
inventory.AddItem(new Product("Onion", 5));


Console.WriteLine("Welcome to the vending machine!");

var user = new User();
user.AccountBalance = random.Next(100, 200);

while (true)
{
    Console.WriteLine("Enter your name:");
    user.Name = Console.ReadLine();
    
    if (!string.IsNullOrEmpty(user.Name))
    {
        Console.WriteLine($"\nWelcome, {user.Name}!");
        Console.WriteLine($"Your account balance is {user.AccountBalance} SEK\n");
        break;
    }
    
    Console.WriteLine("Please try again.");

}

Console.WriteLine("Products in the inventory:");
foreach (var product in inventory.Products)
{
    Console.WriteLine($"{product.Name}: {product.Price} SEK");
}

Console.ReadLine();