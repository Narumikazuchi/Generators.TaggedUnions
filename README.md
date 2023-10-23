![Logo](../release/logo.png)
  
![Tests](https://img.shields.io/github/actions/workflow/status/Narumikazuchi/Generators.TaggedUnions/linux-tests.yml?label=linux-tests) ![Tests](https://img.shields.io/github/actions/workflow/status/Narumikazuchi/Generators.TaggedUnions/windows-tests.yml?label=windows-tests) [![NuGet](https://img.shields.io/nuget/v/Narumikazuchi.Generators.TaggedUnions.svg)](https://www.nuget.org/packages/Narumikazuchi.Generators.TaggedUnions)  

# Introduction
Sometimes we want to handle more than one type at the same time. Maybe we want to return either a ```String``` or an ```Int32``` from a method depending on the input. However unlike Typescript where Union-Types are a feature of the language, in C# we don't have such a feature.
This library aims to fix that. But instead of using generics, like the known ```OneOf``` library does, this library generates types tailored to the use-case for each user.

# How to use
Using the library is really simple. You just need to decorate you assemlby with the ```UnionOf``` Attribute. This can be don in any file you want, as long as it precedes the namespace declaration of that file.  
Here an example:  
```csharp
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generated;
using System;

[assembly: UnionOf(typeof(Half), typeof(float), typeof(double), Typename = "FloatingPointNumber")]

namespace ExampleProject
{
	static public class Program
	{
		static internal void Main(string[] args)
		{
			FloatingPointNumber floatingPointNumber = 42d;
			FloatingPointNumber floatingPointNumber2 = 69f;
			FloatingPointNumber floatingPointNumber3 = (System.Half)420d;
		}
	}
}
```  
All you need to declare a tagged union is the ```[assembly: UnionOf()]``` attribute with at least 2 types passed to the constructor. The code generator will generate a name for you if you don't specify one (i.e. UnionOf_String_Int).
The generated types will reside in the ```Narumikazuchi.Generated``` namespace together with generated code from other generators of the ```Narumikazuchi.Generators``` family.
  
## Operators
As you can see in the example above you can implicitly assign any of the union types to the generated type. The generator will apart from that also generate overloads for the binary-or operator (```|```). So in case you are not sure whether or not the tagged union type that has
been passed to your method was actually initialized you can assign i.e. default values like so:  
```csharp
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generated;
using System;

[assembly: UnionOf(typeof(Half), typeof(float), typeof(double), Typename = "FloatingPointNumber")]

namespace ExampleProject
{
	static public class Program
	{
		static internal void Main(string[] args)
		{
			FloatingPointNumber floatingPointNumber = default;
			DoStuff(floatingPointNumber);
		}
		
		static internal void DoStuff(FloatingPointNumber floatingPointNumber)
		{
			floatingPointNumber |= 42d;
			...
		}
	}
}
```  
Be aware however that this operator will only assign a value (since it's a ```readonly struct``` it will actually create a new instance) if the current instance in uninitialized.  
  
## Is-Method
The code generator will also generate n-overloads for the ```Is``` method, where n is the number of types the tagged union type is composed out of. This method will help you identify the actual value of the union type when required. It looks as close to pattern matching as
possible with just the method signature. Unfortunately checking with the method is not exhaustive, meaning even if you checked all but one type you will still need to call the method to get the actual value instead of just writing ```else```.
```csharp
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generated;
using System;

[assembly: UnionOf(typeof(string), typeof(int), Typename = "StringOrInt")]

namespace ExampleProject
{
	static public class Program
	{
		static internal void Main(string[] args)
		{
			StringOrInt stringOrInt = "Hello, World!";
			Greet(stringOrInt);
		}
		
		static internal void Greet(StringOrInt stringOrInt)
		{
			if (stringOrInt.Is(out string? text))
			{
				Console.WriteLine(text);
			}
			else if (stringOrInt.Is(out int? value)
			{
				Console.WriteLine($"Greetings! It's {value} o'clock.");
			}
		}
	}
}
```  
## HasValue-Property
You can check whether the tagged union was initialized with the ```HasValue``` property.  
  
## Common methods
The code generator will find all methods and properties that all types that make up the tagged union have in common and will add those to the generated tagged union type.
Because of this you can call the ```Equals(object)``` method on the tagged union type and it will use the method of the actual type it's currently holding.
This will be done for every common method and property found on all types.  
```csharp
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generated;
using System;

[assembly: UnionOf(typeof(string), typeof(int), Typename = "StringOrInt")]

namespace ExampleProject
{
	static public class Program
	{
		static internal void Main(string[] args)
		{
			StringOrInt stringOrInt = "Hello, World!";
			if (stringOrInt.Equals("Hello, World!")) // Will equate to true
			{
				Console.WriteLine(stringOrInt.ToString()); // Will write "Hello, World!" to the console
			}
		}
	}
}
```  

# Example of generated code
Finally for the ones who are interested here is an example of what the generated file could look like.  
```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Narumikazuchi.Generated
{
    /// <summary>
    /// A type that can be assigned either of these types: <code><see cref="string"/> | <see cref="int"/></code>
    /// </summary>
    [CompilerGenerated]
    public readonly struct StringOrInt
    {
        static public implicit operator StringOrInt(String value)
        {
            return new StringOrInt(value);
        }

        static public StringOrInt operator |(StringOrInt first, String second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new StringOrInt(second);
            }
            else
            {
                return first;
            }
        }

        static public explicit operator String?(StringOrInt source)
        {
            if (source.Is(out String? result))
            {
                return result;
            }
            else
            {
                return default;
            }
        }

        static public implicit operator StringOrInt(Int32 value)
        {
            return new StringOrInt(value);
        }

        static public StringOrInt operator |(StringOrInt first, Int32 second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new StringOrInt(second);
            }
            else
            {
                return first;
            }
        }

        static public explicit operator Int32?(StringOrInt source)
        {
            if (source.Is(out Int32? result))
            {
                return result;
            }
            else
            {
                return default;
            }
        }

        public Int32 CompareTo(Object? value)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.CompareTo(value);
                case Tag.Tag_2:
                    return m_2.CompareTo(value);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override Boolean Equals(Object? obj)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.Equals(obj);
                case Tag.Tag_2:
                    return m_2.Equals(obj);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override Int32 GetHashCode()
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.GetHashCode();
                case Tag.Tag_2:
                    return m_2.GetHashCode();
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public String ToString(IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(provider);
                case Tag.Tag_2:
                    return m_2.ToString(provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override String ToString()
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString();
                case Tag.Tag_2:
                    return m_2.ToString();
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public bool Is([NotNullWhen(true)] out String? result)
        {
            if (m_Tag == Tag.Tag_1)
            {
                result = m_1;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public bool Is([NotNullWhen(true)] out Int32? result)
        {
            if (m_Tag == Tag.Tag_2)
            {
                result = m_2;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public bool HasValue
        {
            get
            {
                return m_Tag != Tag.Uninitialized;
            }
        }

        private StringOrInt(String value) : this()
        {
            m_1 = value;
            m_Tag = Tag.Tag_1;
        }

        private StringOrInt(Int32 value) : this()
        {
            m_2 = value;
            m_Tag = Tag.Tag_2;
        }

        private enum Tag
        {
            Uninitialized,
            Tag_1,
            Tag_2,
        }

        private readonly String m_1;
        private readonly Int32 m_2;
        private readonly Tag m_Tag;
    }
}
```
