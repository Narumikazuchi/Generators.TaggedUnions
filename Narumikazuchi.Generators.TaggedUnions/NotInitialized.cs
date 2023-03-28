namespace Narumikazuchi.Generators.TaggedUnions;

/// <summary>
/// The exception that is thrown when trying to use the value of an uninitialized union.
/// </summary>
public sealed class NotInitialized : Exception
{
    /// <summary>
    /// The exception that is thrown when trying to use the value of an uninitialized union.
    /// </summary>
    public NotInitialized()
        : base("The type has not been initialized.")
    { }

    /// <summary>
    /// The exception that is thrown when trying to use the value of an uninitialized union.
    /// </summary>
    public NotInitialized(String message)
        : base(message)
    { }
}