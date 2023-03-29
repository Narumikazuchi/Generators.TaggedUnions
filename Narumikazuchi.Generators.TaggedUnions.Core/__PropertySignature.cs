namespace Narumikazuchi.Generators.TaggedUnions;

internal readonly struct __PropertySignature : __ISignature, IEquatable<__ISignature>
{
    public readonly override Boolean Equals(Object obj)
    {
        return obj is __ISignature other &&
               this.Equals(other);
    }

    public readonly override Int32 GetHashCode()
    {
        return this.ToSignatureString()
                   .GetHashCode();
    }

    public readonly Boolean Equals(__ISignature? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.ToSignatureString()
                   .Equals(value: other.ToSignatureString(),
                           comparisonType: StringComparison.InvariantCultureIgnoreCase);
    }

    public readonly String ToCallString()
    {
        return m_Call.Value;
    }

    public readonly String ToSignatureString(Boolean includeParameterNames = default)
    {
        if (includeParameterNames)
        {
            return m_SignatureWithNames.Value;
        }
        else
        {
            return m_Signature.Value;
        }
    }

    public String Name { get; }

    internal __PropertySignature(IPropertySymbol property)
    {
        m_Call = new(() => GenerateCallString(property));
        m_Signature = new(() => GenerateSignatureString(property));
        m_SignatureWithNames = new(() => GenerateSignatureString(property, true));
        this.CanRead = property.IsReadOnly || !property.IsWriteOnly;
        this.CanWrite = property.ContainingType.IsReferenceType &&
                        (property.IsWriteOnly || !property.IsReadOnly);
        this.Name = property.Name;
    }

    internal Boolean CanRead { get; }

    internal Boolean CanWrite { get; }

    static private String GenerateCallString(IPropertySymbol property)
    {
        StringBuilder builder = new();

        String parameters = String.Empty;
        if (!property.Parameters.IsDefaultOrEmpty)
        {
            parameters = $"[{String.Join(", ", property.Parameters.Select(x => x.Name))}]";
            builder.Append(parameters);
        }
        else
        {
            builder.Append($".{property.Name}");
        }

        return builder.ToString().Replace("?", "");
    }

    static private String GenerateSignatureString(IPropertySymbol property,
                                                  Boolean includeNames = default)
    {
        StringBuilder builder = new();

        String parameters = String.Empty;
        if (!property.Parameters.IsDefaultOrEmpty)
        {
            if (includeNames)
            {
                parameters = $"[{String.Join(", ", property.Parameters.Select(x => x.ToDisplayString()))}]";
            }
            else
            {
                parameters = $"[{String.Join(", ", property.Parameters.Select(x => x.Type.ToDisplayString()))}]";
            }
        }

        builder.Append($"{property.Type.ToDisplayString()} {property.Name.Replace("[", "").Replace("]", "")}{parameters}");
        if (!includeNames)
        {
            builder.Append(" { ");
            if (property.IsReadOnly)
            {
                builder.Append("get; }");
            }
            else if (property.IsWriteOnly)
            {
                builder.Append("set; }");
            }
            else
            {
                builder.Append("get; set; }");
            }
        }

        return builder.ToString().Replace("?", "");
    }

    private readonly Lazy<String> m_Call;
    private readonly Lazy<String> m_Signature;
    private readonly Lazy<String> m_SignatureWithNames;
}