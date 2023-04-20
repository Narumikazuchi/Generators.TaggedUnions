namespace Narumikazuchi.Generators.TaggedUnions.Generators;

internal interface __ISignature
{
    public String ToCallString();

    public String ToSignatureString(Boolean includeParameterNames = default);

    public String Name { get; }
}