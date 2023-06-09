﻿namespace Tests.CodeGenerator;

#pragma warning disable IDE1006 // No need to add postfix 'Asynchronously' here
[TestClass]
public class PrimitiveTypes
{
    [TestMethod]
    public async Task UnionOf2Types()
    {
        String source = @"using Narumikazuchi.Generated;
using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Int64), Typename = ""Integer"")]";

        String generated = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Narumikazuchi.Generated
{
    /// <summary>
    /// A type that can be assigned either of these types: <code><see cref=""int""/> | <see cref=""long""/></code>
    /// </summary>
    [CompilerGenerated]
    public readonly struct Integer
    {
        static public implicit operator Integer(int value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, int second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        static public implicit operator Integer(long value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, long second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        public int CompareTo(object? value)
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

        public override bool Equals(object? obj)
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

        public override int GetHashCode()
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

        public string ToString(System.IFormatProvider? provider)
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

        public string ToString(string? format)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(format);
                case Tag.Tag_2:
                    return m_2.ToString(format);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public string ToString(string? format, System.IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(format, provider);
                case Tag.Tag_2:
                    return m_2.ToString(format, provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override string ToString()
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

        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.TryFormat(destination, out charsWritten, format, provider);
                case Tag.Tag_2:
                    return m_2.TryFormat(destination, out charsWritten, format, provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public bool Is([NotNullWhen(true)] out int? result)
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

        public bool Is([NotNullWhen(true)] out long? result)
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

        private Integer(int value) : this()
        {
            m_1 = value;
            m_Tag = Tag.Tag_1;
        }

        private Integer(long value) : this()
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

        private readonly int m_1;
        private readonly long m_2;
        private readonly Tag m_Tag;
    }
}";

        await GeneratorTest.VerifySourceGeneratorAsynchronously(source, ("Narumikazuchi.Generators.TaggedUnions.Core\\Narumikazuchi.Generators.TaggedUnions.Generators.TaggedUnionGenerator\\Integer.g.cs", SourceText.From(generated, Encoding.UTF8)));
    }

    [TestMethod]
    public async Task UnionOfManyTypes()
    {
        String source = @"using Narumikazuchi.Generated;
using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(SByte), typeof(Int16), typeof(Int32), typeof(Int64), Typename = ""Integer"")]";

        String generated = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Narumikazuchi.Generated
{
    /// <summary>
    /// A type that can be assigned either of these types: <code><see cref=""sbyte""/> | <see cref=""short""/> | <see cref=""int""/> | <see cref=""long""/></code>
    /// </summary>
    [CompilerGenerated]
    public readonly struct Integer
    {
        static public implicit operator Integer(sbyte value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, sbyte second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        static public implicit operator Integer(short value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, short second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        static public implicit operator Integer(int value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, int second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        static public implicit operator Integer(long value)
        {
            return new Integer(value);
        }

        static public Integer operator |(Integer first, long second)
        {
            if (first.m_Tag == Tag.Uninitialized)
            {
                return new Integer(second);
            }
            else
            {
                return first;
            }
        }

        public int CompareTo(object? obj)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.CompareTo(obj);
                case Tag.Tag_2:
                    return m_2.CompareTo(obj);
                case Tag.Tag_3:
                    return m_3.CompareTo(obj);
                case Tag.Tag_4:
                    return m_4.CompareTo(obj);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override bool Equals(object? obj)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.Equals(obj);
                case Tag.Tag_2:
                    return m_2.Equals(obj);
                case Tag.Tag_3:
                    return m_3.Equals(obj);
                case Tag.Tag_4:
                    return m_4.Equals(obj);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override int GetHashCode()
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.GetHashCode();
                case Tag.Tag_2:
                    return m_2.GetHashCode();
                case Tag.Tag_3:
                    return m_3.GetHashCode();
                case Tag.Tag_4:
                    return m_4.GetHashCode();
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public string ToString(System.IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(provider);
                case Tag.Tag_2:
                    return m_2.ToString(provider);
                case Tag.Tag_3:
                    return m_3.ToString(provider);
                case Tag.Tag_4:
                    return m_4.ToString(provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public string ToString(string? format)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(format);
                case Tag.Tag_2:
                    return m_2.ToString(format);
                case Tag.Tag_3:
                    return m_3.ToString(format);
                case Tag.Tag_4:
                    return m_4.ToString(format);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public string ToString(string? format, System.IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString(format, provider);
                case Tag.Tag_2:
                    return m_2.ToString(format, provider);
                case Tag.Tag_3:
                    return m_3.ToString(format, provider);
                case Tag.Tag_4:
                    return m_4.ToString(format, provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public override string ToString()
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.ToString();
                case Tag.Tag_2:
                    return m_2.ToString();
                case Tag.Tag_3:
                    return m_3.ToString();
                case Tag.Tag_4:
                    return m_4.ToString();
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public bool TryFormat(System.Span<char> destination, out int charsWritten, System.ReadOnlySpan<char> format, System.IFormatProvider? provider)
        {
            switch (m_Tag)
            {
                case Tag.Tag_1:
                    return m_1.TryFormat(destination, out charsWritten, format, provider);
                case Tag.Tag_2:
                    return m_2.TryFormat(destination, out charsWritten, format, provider);
                case Tag.Tag_3:
                    return m_3.TryFormat(destination, out charsWritten, format, provider);
                case Tag.Tag_4:
                    return m_4.TryFormat(destination, out charsWritten, format, provider);
                default:
                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();
            };
        }

        public bool Is([NotNullWhen(true)] out sbyte? result)
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

        public bool Is([NotNullWhen(true)] out short? result)
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

        public bool Is([NotNullWhen(true)] out int? result)
        {
            if (m_Tag == Tag.Tag_3)
            {
                result = m_3;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public bool Is([NotNullWhen(true)] out long? result)
        {
            if (m_Tag == Tag.Tag_4)
            {
                result = m_4;
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

        private Integer(sbyte value) : this()
        {
            m_1 = value;
            m_Tag = Tag.Tag_1;
        }

        private Integer(short value) : this()
        {
            m_2 = value;
            m_Tag = Tag.Tag_2;
        }

        private Integer(int value) : this()
        {
            m_3 = value;
            m_Tag = Tag.Tag_3;
        }

        private Integer(long value) : this()
        {
            m_4 = value;
            m_Tag = Tag.Tag_4;
        }

        private enum Tag
        {
            Uninitialized,
            Tag_1,
            Tag_2,
            Tag_3,
            Tag_4,
        }

        private readonly sbyte m_1;
        private readonly short m_2;
        private readonly int m_3;
        private readonly long m_4;
        private readonly Tag m_Tag;
    }
}";

        await GeneratorTest.VerifySourceGeneratorAsynchronously(source, ("Narumikazuchi.Generators.TaggedUnions.Core\\Narumikazuchi.Generators.TaggedUnions.Generators.TaggedUnionGenerator\\Integer.g.cs", SourceText.From(generated, Encoding.UTF8)));
    }
}