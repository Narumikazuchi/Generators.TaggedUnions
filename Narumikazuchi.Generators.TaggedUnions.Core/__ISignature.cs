namespace Narumikazuchi.Generators.TaggedUnions;

internal interface __ISignature
{
    public String ToCallString();

    public String ToSignatureString(Boolean includeParameterNames = default);

    public ImmutableArray<IParameterSymbol> Parameters { get; }
}