using Newtonsoft.Json.Linq;

namespace VendingMachine;

public class QuoteFetcher
{
    public static async Task<Quote> GetData()
    {
        string baseUrl = "https://api.quotable.io/quotes/random?maxLength=50/";

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
        catch(Exception exception) 
        {
            Console.WriteLine(exception);
        }
        
        return null;
    }
}