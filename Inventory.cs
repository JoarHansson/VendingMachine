using VendingMachine.Products;

namespace VendingMachine;

public class Inventory
{
    public List<Product> Products { get; } = new List<Product>();

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public List<Quote> Quotes { get; } = new List<Quote>();

    public void AddQuote(Quote quote)
    {
        Quotes.Add(quote);
    }

    public List<CatFact> CatFacts { get; } = new List<CatFact>();

    public void AddCatFact(CatFact catFact)
    {
        CatFacts.Add(catFact);
    }

    public List<DogFact> DogFacts { get; } = new List<DogFact>();

    public void AddDogFact(DogFact dogFact)
    {
        DogFacts.Add(dogFact);
    }
}