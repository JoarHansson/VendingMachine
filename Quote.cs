namespace VendingMachine;

public class Quote
{
    public string Content { get; set; }
    public string Author { get; set; }

    public Quote(string content, string author)
    {
        Content = content;
        Author = author;
    }
}