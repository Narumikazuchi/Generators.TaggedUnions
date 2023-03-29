namespace Narumikazuchi.Generators.TaggedUnions;

static public class Extensions
{
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
}