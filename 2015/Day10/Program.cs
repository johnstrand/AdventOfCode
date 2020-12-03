using System;
using System.Collections.Generic;
using System.Linq;

var input = new Queue<int>("1113122113".Select(c => c - '0'));
//var input = new Queue<int>(new[] { 1 });
for (var iter = 0; iter < 50; iter++)
{
    var next = new Queue<int>();
    while (input.Any())
    {
        var digit = input.Dequeue();
        var count = 1;
        while (input.Any() && input.Peek() == digit)
        {
            input.Dequeue();
            count++;
        }
        next.Enqueue(count);
        next.Enqueue(digit);
    }
    input = next;
    if (iter == 39 || iter == 49)
    {
        Console.WriteLine($"Iteration {iter + 1}: {next.Count}");
    }
}
