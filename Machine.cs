using ConsoleTables;

namespace VendingMachine;

public class Machine
{
    private Dictionary<string, string> Commands { get; set; } = new Dictionary<string, string>()
    {
        { "help", "List all available commands." },
        { "money", "Show your account balance." },
        { "list", "List all products, their price and inventory status." },
        { "buy", "Make a purchase (select items in next view)." },
        { "my stuff", "Look at the stuff you bought" },
        { "quit", "Quit the application." }
    };

    private Inventory MachineInventory { get; set; } = new Inventory();
    private User User { get; set; } = new User();
    private Random Random { get; set; } = new Random();

    public void Run()
    {
        // add some products to the inventory
        MachineInventory.AddProduct(new Product("Quote", 3, 9));
        MachineInventory.AddProduct(new Product("Dog fact", 5, 7));
        MachineInventory.AddProduct(new Product("Cat fact", 8, 4));

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
        ColorController.WritePurpleLine($"Bye {User.Name}!");
        Thread.Sleep(1000);
    }

    private string GetCommand()
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

    private void RunCommandHelp()
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

    private void RunCommandShowAccountBalance()
    {
        Console.WriteLine();
        ColorController.WritePurpleLine($"You have {User.AccountBalance} credits in your account.");
    }

    private void RunCommandListProducts()
    {
        var table = new ConsoleTable("Product", "Price", "Items in stock");

        foreach (var product in MachineInventory.Products)
        {
            table.AddRow(product.Name, product.Price, product.ItemsInStock);
        }

        Console.WriteLine();
        ColorController.WritePurpleTable(table);
    }

    private void RunCommandBuyProducts()
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

    private void GoToCheckout(Product chosenProduct, int chosenNumberOfProductsInt)
    {
        while (true)
        {
            Console.WriteLine();
            ColorController.WriteYellowAndBlue([
                "You have ", User.AccountBalance.ToString(),
                " credits in your account. Do you wish to continue?", " (y/n)", ": "
            ]);

            var answer = Console.ReadLine();

            if (answer.ToLower() == "n")
            {
                ColorController.WriteGreenLine("OK, Purchase cancelled.");
                return;
            }

            if (answer.ToLower() != "y")
            {
                ColorController.WriteRedLine("Try again.");
                continue;
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

            // make API requests depending on product choice and save the values in users inventory
            if (chosenProduct.Name == "Quote")
            {
                for (int i = 0; i < chosenNumberOfProductsInt; i++)
                {
                    var quote = Fetcher.GetRandomQuote().GetAwaiter().GetResult();

                    User.Inventory.AddQuote(quote);
                }
            }

            if (chosenProduct.Name == "Dog fact")
            {
                for (int i = 0; i < chosenNumberOfProductsInt; i++)
                {
                    var dogFact = Fetcher.GetRandomDogFact().GetAwaiter().GetResult();

                    User.Inventory.AddDogFact(dogFact);
                }
            }

            if (chosenProduct.Name == "Cat fact")
            {
                for (int i = 0; i < chosenNumberOfProductsInt; i++)
                {
                    var catFact = Fetcher.GetRandomCatFact().GetAwaiter().GetResult();

                    User.Inventory.AddCatFact(catFact);
                }
            }

            return;
        }
    }

    private void RunCommandShowUsersProducts()
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

    private void InspectUsersProducts()
    {
        ColorController.WriteYellowAndBlue(["Would you like to inspect any items?", " (y/n)", ": "]);

        while (true)
        {
            var answerInspectItems = Console.ReadLine();

            if (answerInspectItems.ToLower() == "n")
            {
                ColorController.WriteGreenLine("OK, going back home.");
                return;
            }

            if (answerInspectItems.ToLower() != "y")
            {
                ColorController.WriteRedLine("Try again.");
                Console.WriteLine();
                ColorController.WriteYellowAndBlue(["Would you like to inspect any items?", " (y/n)", ": "]);
                continue;
            }

            Console.WriteLine();
            ColorController.WriteYellow("Enter the name of the product you wish to inspect: ");

            var answerWhichItem = Console.ReadLine();

            var usersProducts = User.Inventory.Products
                .Select(product => product.Name.ToLower())
                .ToList();

            if (!usersProducts.Contains(answerWhichItem.ToLower()))
            {
                ColorController.WriteRedLine("No item by that name. Check your spelling.");
                Console.WriteLine();
                ColorController.WriteYellowAndBlue(["Would you like to try again?", " (y/n)", ": "]);
                continue;
            }

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

            if (answerWhichItem.ToLower() == "cat fact")
            {
                Console.WriteLine();

                foreach (var catFact in User.Inventory.CatFacts)
                {
                    ColorController.WritePurpleLine(catFact.Content);
                    Console.WriteLine();
                }
            }

            if (answerWhichItem.ToLower() == "dog fact")
            {
                Console.WriteLine();

                foreach (var dogFact in User.Inventory.DogFacts)
                {
                    ColorController.WritePurpleLine(dogFact.Content);
                    Console.WriteLine();
                }
            }

            ColorController.WriteYellowAndBlue(["Would you like to inspect any more items?", " (y/n)", ": "]);
        }
    }
}