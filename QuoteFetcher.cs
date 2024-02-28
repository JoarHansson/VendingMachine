using Newtonsoft.Json.Linq;

namespace VendingMachine;

public class QuoteFetcher
{
    public async void GetData()
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
                            
                            Console.WriteLine(dataParsed);
                            
                            Quote quote = new Quote(dataParsed["content"].ToString(), dataParsed["author"].ToString());
                            
                            Console.WriteLine($"Quote: {quote.Content}");
                            Console.WriteLine($"Author: {quote.Author}");
                        }
                        else
                        {
                            Console.WriteLine("Data is null!");
                        }
                    }
                }
            }
        } 
        catch(Exception exception) 
        {
            Console.WriteLine(exception);
        }
        
    }

}