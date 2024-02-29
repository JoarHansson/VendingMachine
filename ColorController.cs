using ConsoleTables;

namespace VendingMachine;

public class ColorController
{
    public static void WriteRedLine(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    
    public static void WriteGreenLine(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    
    public static void WriteYellow(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(input);
        Console.ResetColor();
    }
    
    public static void WriteYellowAndBlue(List<string> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            if (i % 2 == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(input[i]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(input[i]);
            }
        }
        
        Console.ResetColor();
    }
    
    public static void WritePurpleTable(ConsoleTable table)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        table.Write(Format.Minimal);
        Console.ResetColor();
    }
    

}