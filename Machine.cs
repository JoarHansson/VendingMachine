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
            ColorController.WriteYellow("Enter your name: ");
            
            User.Name = Console.ReadLine();
    
            if (!string.IsNullOrEmpty(User.Name))
            {
                Console.WriteLine();
                ColorController.WriteGreenLine($"Welcome, {User.Name}!");
                break;
            }
    
            ColorController.WriteRedLine("Please try again.");
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

        Console.WriteLine();
        Console.WriteLine("Bye!");
        Thread.Sleep(1000);
    }
    
    public string GetCommand()
    {
        while (true)
        {
            Console.WriteLine();
            ColorController.WriteYellowAndBlue(["Input a command. Type", " help ", "to list all commands: "]);
                
            var input = Console.ReadLine()!;

            if (Commands.ContainsKey(input))
            {
                ColorController.WriteGreenLine("Command OK");
                return input;
            }
            
            ColorController.WriteRedLine("Not a valid command, try again.");
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
        
        Console.WriteLine();
        ColorController.WritePurpleTable(table);
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
        
        Console.WriteLine();
        ColorController.WritePurpleTable(table);
    }

    public void RunCommandBuyProducts()
    {
        while (true)
        {
            Console.WriteLine();
            ColorController.WriteYellowAndBlue(["Choose a product by typing its name (type", " list ", "to list the products, type", " home ", "to go home): "]);
            
            var input = Console.ReadLine();

            if (input == "list")
            {
                ColorController.WriteGreenLine("Command OK");
                RunCommandListProducts();
                continue;
            }

            if (input == "home")
            {
                ColorController.WriteGreenLine("Command OK");
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
                ColorController.WriteRedLine("Error. Are you sure you spelled correctly?");
                continue;
            }
            
            ColorController.WriteGreenLine($"{chosenProduct.Name} selected.");
            
            Console.WriteLine();
            ColorController.WriteYellow($"How many would you like? Enter a number between 0 and {chosenProduct.ItemsInStock}: ");
                        
            var chosenNumberOfProductsString = Console.ReadLine();
                
            if (int.TryParse(chosenNumberOfProductsString, out var chosenNumberOfProductsInt) == false)
            {
                ColorController.WriteRedLine("Not a valid number");
                continue;
            }

            if (chosenNumberOfProductsInt > chosenProduct.ItemsInStock)
            {
                ColorController.WriteRedLine("Not that many in stock.");
                continue;
            }

            if (User.AccountBalance < (chosenProduct.Price * chosenNumberOfProductsInt))
            {
                ColorController.WriteRedLine("Not enough money in your account.");
                continue;
            }
            
            if (chosenNumberOfProductsInt == 0)
            {
                ColorController.WriteGreenLine("OK, Purchase cancelled.");
                return;
            }
            
            // Order summary table
            var table = new ConsoleTable("Order summary", "");
            
            table.AddRow("Product", chosenProduct.Name);
            table.AddRow("Price per item", chosenProduct.Price);
            table.AddRow("Number of items", chosenNumberOfProductsInt);
            table.AddRow(" ", " ");
            table.AddRow("Total cost", chosenProduct.Price * chosenNumberOfProductsInt);
    
            Console.WriteLine();
            ColorController.WritePurpleTable(table);
            
            while (true)
            {
                Console.WriteLine();
                ColorController.WriteYellowAndBlue(["You have ", User.AccountBalance.ToString(), " credits in your account. Do you wish to continue? (y/n): "]);
                
                var answer = Console.ReadLine();
                
                if (answer.ToLower() == "y")
                {
                    User.AccountBalance -= (chosenProduct.Price * chosenNumberOfProductsInt);
                    chosenProduct.ItemsInStock -= chosenNumberOfProductsInt;

                    ColorController.WriteGreenLine("Purchase successful. Visit \"my products\" to see what you bought.");
                    return;
                }
                
                if (answer.ToLower() == "n")
                {
                    ColorController.WriteGreenLine("OK, Purchase cancelled.");
                    return;
                }
            }
        }
    }
}