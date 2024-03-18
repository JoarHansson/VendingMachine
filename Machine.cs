using ConsoleTables;

namespace VendingMachine;

public class Machine
{
    public Dictionary<string, string> Commands { get; set; } = new Dictionary<string, string>()
    {
        { "help", "List all available commands." },
        { "money", "Show your account balance." },
        { "list", "List all products, their price and inventory status." },
        { "buy", "Make a purchase (select items in next view)." },
        { "my stuff", "Look at the stuff you bought" },
        { "quit", "Quit the application." }
    };

    public Inventory MachineInventory { get; set; } = new Inventory();
    public User User { get; set; } = new User();
    public Random Random { get; set; } = new Random();

    public void Run()
    {
        // add some products to the inventory
        MachineInventory.AddProduct(new Product("Tomato", 12, 67));
        MachineInventory.AddProduct(new Product("Avocado", 34, 89));
        MachineInventory.AddProduct(new Product("Onion", 5, 10));
        MachineInventory.AddProduct(new Product("Quote", 3, 8));

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
            else if (command == "my stuff")
            {
                RunCommandShowUsersProducts();
            }
        } while (command != "quit");

        Console.WriteLine();
        ColorController.WritePurpleLine("Bye!");
        Thread.Sleep(1000);
    }

    public string GetCommand()
    {
        while (true)
        {
            Console.WriteLine();
            ColorController.WriteYellowAndBlue(["Input a command / get", " help", ": "]);

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
        ColorController.WritePurpleLine($"You have {User.AccountBalance} credits in your account.");
    }

    public void RunCommandListProducts()
    {
        var table = new ConsoleTable("Product", "Price", "Items in stock");

        foreach (var product in MachineInventory.Products)
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
            ColorController.WriteYellowAndBlue([
                "Choose a product /", " list ", "all products / go", " home", ": "
            ]);

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

            foreach (var product in MachineInventory.Products)
            {
                if (input.ToLower() == product.Name.ToLower())
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
            ColorController.WriteYellow(
                $"How many would you like? Enter a number between 0 and {chosenProduct.ItemsInStock}: ");

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

            GoToCheckout(chosenProduct, chosenNumberOfProductsInt);
            return;
        }
    }

    public void GoToCheckout(Product chosenProduct, int chosenNumberOfProductsInt)
    {
        while (true)
        {
            Console.WriteLine();
            ColorController.WriteYellowAndBlue([
                "You have ", User.AccountBalance.ToString(),
                " credits in your account. Do you wish to continue? (y/n): "
            ]);

            var answer = Console.ReadLine();

            if (answer.ToLower() != "y" && answer.ToLower() != "n")
            {
                ColorController.WriteRedLine("Try again.");
                continue;
            }

            if (answer.ToLower() == "n")
            {
                ColorController.WriteGreenLine("OK, Purchase cancelled.");
                return;
            }

            // answer.ToLower() == "y": 
            User.AccountBalance -= (chosenProduct.Price * chosenNumberOfProductsInt);
            chosenProduct.ItemsInStock -= chosenNumberOfProductsInt;


            // check if product exists by checking its unique name, if so, increase the count. otherwise create new.
            var createNewProduct = true;

            foreach (var product in User.Inventory.Products)
            {
                if (chosenProduct.Name == product.Name)
                {
                    product.ItemsInStock += chosenNumberOfProductsInt;
                    createNewProduct = false;
                }
            }

            if (createNewProduct)
            {
                User.Inventory.AddProduct(new Product(chosenProduct.Name, chosenProduct.Price,
                    chosenNumberOfProductsInt));
            }

            ColorController.WriteGreenLine("Purchase successful. Visit \"my stuff\" to see what you bought.");

            if (chosenProduct.Name == "Quote")
            {
                for (int i = 0; i < chosenNumberOfProductsInt; i++)
                {
                    var quote = QuoteFetcher.GetData().GetAwaiter().GetResult();

                    User.Inventory.AddQuote(quote);
                }
            }

            return;
        }
    }

    public void RunCommandShowUsersProducts()
    {
        if (User.Inventory.Products.Count == 0)
        {
            Console.WriteLine();
            ColorController.WritePurpleLine("You haven't bought anything yet. Go buy something.");
            return;
        }

        var table = new ConsoleTable("Product", "Price", "Items owned");

        foreach (var product in User.Inventory.Products)
        {
            table.AddRow(product.Name, product.Price, product.ItemsInStock);
        }

        Console.WriteLine();
        ColorController.WritePurpleTable(table);

        InspectUsersProducts();
    }

    public void InspectUsersProducts()
    {
        while (true)
        {
            ColorController.WriteYellow("Would you like to inspect any items? (y/n): ");

            var answerInspectItems = Console.ReadLine();

            if (answerInspectItems.ToLower() == "n")
            {
                ColorController.WriteGreenLine("OK, going back home.");
                return;
            }

            if (answerInspectItems.ToLower() != "y")
            {
                ColorController.WriteRedLine("Try again.");
                continue;
            }

            Console.WriteLine();
            ColorController.WriteYellow("Enter the name of the product you wish to inspect: ");
            // only works with qoute so far

            var answerWhichItem = Console.ReadLine();

            if (answerWhichItem.ToLower() == "quote")
            {
                Console.WriteLine();

                foreach (var quote in User.Inventory.Quotes)
                {
                    ColorController.WritePurpleLine(quote.Content);
                    ColorController.WritePurpleLine($"/ {quote.Author}");
                    Console.WriteLine();
                }
            }
        }
    }
}