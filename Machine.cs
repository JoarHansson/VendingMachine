using ConsoleTables;

namespace VendingMachine;

public class Machine
{
    public Dictionary<string, string> Commands { get; set; }= new Dictionary<string, string>()
    {
        { "help", "List all available commands." },
        { "money", "Show your account balance." },
        { "list", "List all products, their price and inventory status." },
        { "buy", "Make a purchase (select items in next view)." },
        { "quit", "Quit the application." }
    };
    
    public Inventory Inventory { get; set; } = new Inventory();
    public User User { get; set; } = new User();
    public Random Random { get; set; } = new Random();
    
    public void Run()
    {
        // add some products to the inventory
        Inventory.AddItem(new Product("Tomato", 12, 67));
        Inventory.AddItem(new Product("Avocado", 34, 89));
        Inventory.AddItem(new Product("Onion", 5,  10));
        Inventory.AddItem(new Product("Quote", 3,  8));

        // give user some money
        User.AccountBalance = Random.Next(100, 200);

        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(" --- Welcome to the vending machine! --- ");
        Console.ResetColor();
        Console.WriteLine();
        
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Enter your name: ");
            Console.ResetColor();
            
            User.Name = Console.ReadLine();
    
            if (!string.IsNullOrEmpty(User.Name))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"Welcome, {User.Name}!");
                Console.ResetColor();
                break;
            }
    
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Please try again.");
            Console.ResetColor();
        }
        
        
        string command;

        do
        {
            command = GetCommand();

            if (command == "help")
            {
                RunCommandHelp();
            }
            else if (command == "money")
            {
                RunCommandShowAccountBalance();
            }
            else if (command == "list")
            {
                RunCommandListProducts();
            }
            else if (command == "buy")
            {
                RunCommandBuyProducts();
            }
        } while (command != "quit");

        Console.WriteLine("Bye!");
        Thread.Sleep(1000);
    }
    
    public string GetCommand()
    {
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Input a command. Type");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" help ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("to list all commands: ");
            Console.ResetColor();

            var input = Console.ReadLine()!;

            if (Commands.ContainsKey(input))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Command OK");
                Console.ResetColor();

                return input;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Not a valid command, try again.");
            Console.ResetColor();
        }
    }
    
    public void RunCommandHelp()
    {
        var table = new ConsoleTable("Command", "Action");
            
        foreach (var command in Commands)
        {
            if (command.Key != "help")
            {
                table.AddRow(command.Key, command.Value); 
            }
        }
        
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine();
        table.Write(Format.Minimal);
        Console.ResetColor();
    }

    public void RunCommandShowAccountBalance()
    {
        Console.WriteLine();
        Console.WriteLine($"Your account balance is {User.AccountBalance} SEK");
    }

    public void RunCommandListProducts()
    {
        var table = new ConsoleTable("Product", "Price", "Items in stock");
        
        foreach (var product in Inventory.Products)
        { 
            table.AddRow(product.Name, product.Price, product.ItemsInStock);
        }
        
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine();
        table.Write(Format.Minimal);
        Console.ResetColor();
    }

    public void RunCommandBuyProducts()
    {
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Choose a product by typing its name (type");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" list ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("to list the products, type");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(" home ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("to go home): ");
            Console.ResetColor();

            var input = Console.ReadLine();

            if (input == "list")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Command OK");
                Console.ResetColor();
                
                RunCommandListProducts();
                continue;
            }

            if (input == "home")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Command OK");
                Console.ResetColor();
                
                return;
            }
            
            Product? chosenProduct = null;

            foreach (var product in Inventory.Products)
            {
                if (input == product.Name)
                {
                    chosenProduct = product;
                }
            }

            if (chosenProduct == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error. Are you sure you spelled correctly?");
                Console.ResetColor();
                continue;
            }
            
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"{chosenProduct.Name} selected.");
            Console.ResetColor();
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"How many would you like? Enter a number between 0 and {chosenProduct.ItemsInStock}: ");
            Console.ResetColor();
                        
            var chosenNumberOfProductsString = Console.ReadLine();
                
            if (int.TryParse(chosenNumberOfProductsString, out var chosenNumberOfProductsInt) == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Not a valid number");
                Console.ResetColor();
                continue;
            }

            if (chosenNumberOfProductsInt > chosenProduct.ItemsInStock)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Not that many in stock.");
                Console.ResetColor();
                continue;
            }

            if (User.AccountBalance < (chosenProduct.Price * chosenNumberOfProductsInt))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Not enough money in your account.");
                Console.ResetColor();
                continue;
            }
            
            if (chosenNumberOfProductsInt == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("OK, Purchase cancelled.");
                Console.ResetColor();
                return;
            }
            
            // Order summary table
            var table = new ConsoleTable( "Product", chosenProduct.Name);
            
            table.AddRow("Price per item", chosenProduct.Price);
            table.AddRow("Number of items", chosenNumberOfProductsInt);
            table.AddRow(" ", " ");
            table.AddRow("Total cost", chosenProduct.Price * chosenNumberOfProductsInt);
    
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine();
            table.Write(Format.Minimal);
            Console.ResetColor();
            
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"Your account balance: {User.AccountBalance}. Continue? (y/n): ");
                Console.ResetColor();
                
                var answer = Console.ReadLine();
                
                if (answer.ToLower() == "y")
                {
                    User.AccountBalance -= (chosenProduct.Price * chosenNumberOfProductsInt);
                    chosenProduct.ItemsInStock -= chosenNumberOfProductsInt;

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Purchase successful. Visit \"my products\" to see what you bought.");
                    Console.ResetColor();
                    return;
                }
                
                if (answer.ToLower() == "n")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("OK, Purchase cancelled.");
                    Console.ResetColor();
                    return;
                }
            }
        }
    }
}