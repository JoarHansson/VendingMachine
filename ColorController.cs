using ConsoleTables;

namespace VendingMachine;

public abstract class ColorController
{
    // Red. For errors and similar.
    public static void WriteRedLine(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(input);
        Console.ResetColor();
    }

    // Green. For affirmations and similar.
    public static void WriteGreenLine(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine(input);
        Console.ResetColor();
    }

    // Yellow. For instructions and questions from the machine.
    public static void WriteYellow(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(input);
        Console.ResetColor();
    }

    // Yellow/Blue. For instructions and questions from the machine, where some words need to be highlighted.
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

    // Purple. For informative tables. 
    public static void WritePurpleTable(ConsoleTable table)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        table.Write(Format.Minimal);
        Console.ResetColor();
    }

    // Purple. For other information. 
    public static void WritePurpleLine(string input)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(input);
        Console.ResetColor();
    }
}