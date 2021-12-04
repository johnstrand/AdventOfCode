using System.Security.Cryptography;
using System.Text;

var md5 = MD5.Create();
var input = "bgvyzdsv";
var prefixLength = 6;
var prefix = string.Concat(Enumerable.Repeat("0", prefixLength));
var pos = 0;

while (true)
{
    var hashString = md5.ComputeHash($"{input}{++pos}");
    if (hashString.StartsWith(prefix))
    {
        Console.WriteLine(pos);
        break;
    }
}

Console.ReadLine();

internal static class Extension
{
    public static string ComputeHash(this MD5 crypto, string input)
    {
        return string.Concat(crypto.ComputeHash(Encoding.UTF8.GetBytes(input)).Select(b => b.ToString("x2")));
    }
}
