using ConsoleTables;

namespace VendingMachine;

public class Machine
{
    public Dictionary<string, string> Commands { get; set; }= new Dictionary<string, string>()
    {
        { "help", "List all available commands." },
        { "money", "Show your account balance." },
        { "list", "List all products, their price and inventory status." },
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

        // give user some money
        User.AccountBalance = Random.Next(100, 200);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Welcome to the vending machine!");
        Console.WriteLine();
        Console.ResetColor();
        
        while (true)
        {
            Console.Write("Enter your name: ");
            User.Name = Console.ReadLine();
    
            if (!string.IsNullOrEmpty(User.Name))
            {
                Console.WriteLine();
                Console.WriteLine($"Welcome, {User.Name}!");
                break;
            }
    
            Console.WriteLine("Please try again.");
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
            Console.Write("Input a command. Input");
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
                Console.WriteLine();
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
        table.Write(Format.Minimal);
        Console.ResetColor();


    }

    public void RunCommandShowAccountBalance()
    {
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
        table.Write(Format.Minimal);
        Console.ResetColor();
        
    }
}