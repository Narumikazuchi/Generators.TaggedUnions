using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generated;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = "StringOrInt")]

namespace Debugging;

static public class Program
{
    static internal void Main(String[] args)
    {
        StringOrInt stringOrInt = "Hello, World!";
        if (stringOrInt.Is(out String? stringValue))
        {
            Console.WriteLine(stringValue);
        }
        else if (stringOrInt.Is(out Int32? intValue))
        {
            Console.WriteLine(intValue);
        }
    }
}
