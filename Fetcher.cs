using Newtonsoft.Json.Linq;

namespace VendingMachine;

public class Fetcher
{
    public static async Task<Quote> GetRandomQuote()
    {
        string baseUrl = "https://api.quotable.io/quotes/random?maxLength=75";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        if (data != null)
                        {
                            var dataParsed = JArray.Parse(data)[0];

                            Quote quote = new Quote(dataParsed["content"].ToString(), dataParsed["author"].ToString());
                            return quote;
                        }

                        // (if null:)
                        Quote altQuote = new Quote("The quote API seems to be broken...", "Joar Hansson");
                        return altQuote;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return null;
    }

    public static async Task<CatFact> GetRandomCatFact()
    {
        string baseUrl = "https://meowfacts.herokuapp.com";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        if (data != null)
                        {
                            var dataParsed = JObject.Parse(data);

                            CatFact catFact = new CatFact(dataParsed["data"][0].ToString());
                            return catFact;
                        }

                        // (if null:)
                        CatFact altCatFact = new CatFact("The cat API seems to be broken...");
                        return altCatFact;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return null;
    }

    public static async Task<DogFact> GetRandomDogFact()
    {
        string baseUrl = "https://dogapi.dog/api/facts";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();

                        if (data != null)
                        {
                            var dataParsed = JObject.Parse(data);

                            DogFact dogFact = new DogFact(dataParsed["facts"][0].ToString());
                            return dogFact;
                        }

                        // (if null:)
                        DogFact altDogFact = new DogFact("The dog API seems to be broken...");
                        return altDogFact;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return null;
    }
}