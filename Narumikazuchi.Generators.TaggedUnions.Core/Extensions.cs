using Microsoft.CodeAnalysis;

namespace Narumikazuchi.Generators.TaggedUnions;

static public class Extensions
{
    static public String ToFrameworkString(this ITypeSymbol type)
    {
        String result = type.ToDisplayString()
                            .Replace("*", "");

        foreach (KeyValuePair<String, String> kv in s_BuiltInTypes)
        {
            result = result.Replace(kv.Key, kv.Value);
        }

        if (type.ContainingNamespace is not null &&
            type.ContainingNamespace.Name is "System")
        {
            result = result.Replace("System.", "");
        }

        return result;
    }

    static public Boolean IsValidCSharpTypename(this String value)
    {
        UnicodeCategory category = Char.GetUnicodeCategory(value[0]);
        if (value[0] is not '_' &&
            category is not UnicodeCategory.UppercaseLetter
                     and not UnicodeCategory.LowercaseLetter
                     and not UnicodeCategory.TitlecaseLetter
                     and not UnicodeCategory.ModifierLetter
                     and not UnicodeCategory.OtherLetter)
        {
            return false;
        }

        foreach (Char character in value.AsSpan().Slice(1))
        {
            category = Char.GetUnicodeCategory(character);
            if (category is not UnicodeCategory.UppercaseLetter
                         and not UnicodeCategory.LowercaseLetter
                         and not UnicodeCategory.TitlecaseLetter
                         and not UnicodeCategory.ModifierLetter
                         and not UnicodeCategory.OtherLetter
                         and not UnicodeCategory.LetterNumber
                         and not UnicodeCategory.OtherNumber
                         and not UnicodeCategory.NonSpacingMark
                         and not UnicodeCategory.SpacingCombiningMark
                         and not UnicodeCategory.ConnectorPunctuation
                         and not UnicodeCategory.Format)
            {
                return false;
            }
        }

        return true;
    }

    static private readonly Dictionary<String, String> s_BuiltInTypes = new()
    {
        { "decimal", "Decimal" },
        { "double", "Double" },
        { "ushort", "UInt16" },
        { "object", "Object" },
        { "string", "String" },
        { "float", "Single" },
        { "sbyte", "SByte" },
        { "nuint", "UIntPtr" },
        { "ulong", "UInt64" },
        { "short", "Int16" },
        { "bool", "Boolean" },
        { "long", "Int64" },
        { "nint", "IntPtr" },
        { "uint", "UInt32" },
        { "byte", "Byte" },
        { "char", "Char" },
        { "int", "Int32" },
    };
}