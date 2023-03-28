namespace Narumikazuchi.Generators.TaggedUnions;

/// <summary>
/// Generates a tagged union of the specified types.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
public sealed class UnionOfAttribute : Attribute
{
    /// <summary>
    /// Generates a tagged union of the specified types.
    /// </summary>
    public UnionOfAttribute(Type first,
                            Type second,
                            params Type[] additional)
    { }

    /// <summary>
    /// The name to use for the generated type.
    /// </summary>
    public String? Typename
    {
        get;
        set;
    }
}