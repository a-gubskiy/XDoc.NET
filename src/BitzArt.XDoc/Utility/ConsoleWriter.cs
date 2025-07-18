using System.Diagnostics;

namespace BitzArt.XDoc;

public class ConsoleWriter
{
    public static void WriteLine(string message, ConsoleColor foregroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(message);
        Console.ResetColor();
        
        Trace.WriteLine(message);
    }
}