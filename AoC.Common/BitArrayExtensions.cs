using System.Collections;

namespace AoC.Common;
public static class BitArrayExtensions
{
    public static bool True(this BitArray arr)
    {
        return arr.Length > 0 && Enumerable.Range(0, arr.Length).Any(arr.Get);
    }

    public static bool AllTrue(this BitArray arr)
    {
        return arr.Length > 0 && Enumerable.Range(0, arr.Length).All(arr.Get);
    }

    public static bool False(this BitArray arr)
    {
        return !arr.True();
    }

    public static BitArray One(int length = 8)
    {
        var one = new BitArray(length);
        one.Set(0, true);
        return one;
    }

    public static void Set(this BitArray arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = (value & (1 << i)) != 0;
        }
    }

    public static void Add(this BitArray a, BitArray b)
    {
        var bCopy = new BitArray(b);

        while (bCopy.True())
        {
            var carry = new BitArray(a).And(bCopy);
            a.Xor(bCopy);
            bCopy = carry.LeftShift(1);
        }
    }

    public static void Increment(this BitArray a)
    {
        a.Add(One(a.Length));
    }
}
