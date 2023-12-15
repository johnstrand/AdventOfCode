using System.Text.RegularExpressions;

namespace AoC.Common;
public static class Render
{
    private static readonly Dictionary<string, string> _ansiForegroundColors = new()
    {
        ["black"] = "\u001b[30m",
        ["red"] = "\u001b[31m",
        ["green"] = "\u001b[32m",
        ["yellow"] = "\u001b[33m",
        ["blue"] = "\u001b[34m",
        ["magenta"] = "\u001b[35m",
        ["cyan"] = "\u001b[36m",
        ["white"] = "\u001b[37m",
        ["brightblack"] = "\u001b[90m",
        ["brightred"] = "\u001b[91m",
        ["brightgreen"] = "\u001b[92m",
        ["brightyellow"] = "\u001b[93m",
        ["brightblue"] = "\u001b[94m",
        ["brightmagenta"] = "\u001b[95m",
        ["brightcyan"] = "\u001b[96m",
        ["brightwhite"] = "\u001b[97m",
    };

    private static readonly Dictionary<string, string> _ansiBackgroundColors = new()
    {
        ["black"] = "\u001b[40m",
        ["red"] = "\u001b[41m",
        ["green"] = "\u001b[42m",
        ["yellow"] = "\u001b[43m",
        ["blue"] = "\u001b[44m",
        ["magenta"] = "\u001b[45m",
        ["cyan"] = "\u001b[46m",
        ["white"] = "\u001b[47m",
        ["brightblack"] = "\u001b[100m",
        ["brightred"] = "\u001b[101m",
        ["brightgreen"] = "\u001b[102m",
        ["brightyellow"] = "\u001b[103m",
        ["brightblue"] = "\u001b[104m",
        ["brightmagenta"] = "\u001b[105m",
        ["brightcyan"] = "\u001b[106m",
        ["brightwhite"] = "\u001b[107m",
    };

    private const string _ansiReset = "\u001b[0m";

    private static int _indent;
    private static bool _shouldIndent = true;

    public static string Format(string text)
    {
        return Regex.Replace(text.Replace("[/]", _ansiReset), @"\[(?<color>[a-z]+)\]", m =>
        {
            var key = m.Groups["color"].Value;
            return _ansiForegroundColors.TryGetValue(key, out var color) ? color : $"[{key}]";
        });
    }

    public static void Progress(string text, int done, int total)
    {
        var pct = done * 100 / total;
        Write($"\r{text}: {pct} %");
        if (pct == 100)
        {
            WriteLine("");
        }
    }

    public static void ProgressBar(string text, int done, int total, int width, string marker = ">")
    {
        var pct = done * 100 / total;
        var filled = width * done / total;

        var fill = string.Concat(Enumerable.Repeat(marker, filled));
        var empty = new string(' ', width - filled);
        Write($"\r{text}: [{fill}{empty}] {pct} %");
        if (pct == 100)
        {
            WriteLine("");
        }
    }

    public static void Result(string label, long result)
    {
        WriteLine($"{label}: [green]{result,20}[/]");
    }

    public static void Write(string text)
    {
        if (_shouldIndent || text.StartsWith('\r'))
        {
            Console.Write(new string(' ', _indent * 2));
        }

        Console.Write(Format(text));
        _shouldIndent = text.EndsWith('\r');
    }

    public static void WriteLine(string text)
    {
        if (_shouldIndent)
        {
            Console.Write(new string(' ', _indent * 2));
        }

        Console.WriteLine(Format(text));
        _shouldIndent = true;
    }

    public static void Indent()
    {
        _indent++;
    }

    public static void Unindent()
    {
        _indent--;
    }

    public static IDisposable Scope()
    {
        return new IndentHelper();
    }
}

public sealed class IndentHelper : IDisposable
{
    public IndentHelper()
    {
        Render.Indent();
    }

    public void Dispose()
    {
        Render.Unindent();
    }
}
