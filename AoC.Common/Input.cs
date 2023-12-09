namespace AoC.Common;
public static class Input
{
    public static List<string> ReadActual()
    {
        return Read("input.txt");
    }

    public static List<string> ReadTest()
    {
        return Read("input-test.txt");
    }

    public static List<string> Read(string path)
    {
        return File.ReadLines(path).ToList();
    }
}
