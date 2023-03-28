namespace Narumikazuchi.Generators.TaggedUnions;

internal readonly struct __MethodSignature : __ISignature, IEquatable<__ISignature>
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

    public ImmutableArray<IParameterSymbol> Parameters { get; }

    public Boolean ReturnsVoid { get; }

    public Boolean IsVirtual { get; }

    internal __MethodSignature(IMethodSymbol method)
    {
        m_Call = new(() => GenerateCallString(method));
        m_Signature = new(() => GenerateSignatureString(method));
        m_SignatureWithNames = new(() => GenerateSignatureString(method, true));
        this.Parameters = method.Parameters;
        this.ReturnsVoid = method.ReturnsVoid;
        if (method.Name.StartsWith(nameof(Equals)) &&
            method.Parameters.Length == 1 &&
            method.Parameters[0].Type.ToDisplayString() is "object?")
        {
            this.IsVirtual = true;
        }
        else if (method.Name.StartsWith(nameof(GetHashCode)) &&
                 method.Parameters.Length == 0)
        {
            this.IsVirtual = true;
        }
        else if (method.Name.StartsWith(nameof(ToString)) &&
                 method.Parameters.Length == 0)
        {
            this.IsVirtual = true;
        }
        else
        {
            this.IsVirtual = false;
        }
    }

    static private String GenerateCallString(IMethodSymbol method)
    {
        String parameters = String.Join(", ", method.Parameters.Select(TransformParameterForCall));
        return $"{method.Name}({parameters})".Replace("?", "");
    }

    static private String GenerateSignatureString(IMethodSymbol method,
                                                  Boolean includeNames = default)
    {
        String parameters;
        if (includeNames)
        {
            parameters = String.Join(", ", method.Parameters.Select(parameter => parameter.ToDisplayString()));
        }
        else
        {
            parameters = String.Join(", ", method.Parameters.Select(parameter => parameter.Type.ToDisplayString()));
        }

        String generics = String.Empty;
        String constraint = String.Empty;
        if (method.IsGenericMethod)
        {
            generics = $"<{String.Join(", ", method.TypeParameters.Select(x => x.ToDisplayString()))}>";
            foreach (ITypeParameterSymbol argument in method.TypeArguments.OfType<ITypeParameterSymbol>())
            {
                Boolean anySpecial = false;
                Boolean any = false;
                if (argument.HasNotNullConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : notnull";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasReferenceTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : class";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasUnmanagedTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : unmanaged";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasValueTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : struct";
                    anySpecial = true;
                    any = true;
                }

                if (!argument.ConstraintTypes.IsDefaultOrEmpty)
                {
                    if (anySpecial)
                    {
                        constraint += ", ";
                    }
                    else
                    {
                        constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : ";
                    }

                    constraint += $"{String.Join(", ", argument.ConstraintTypes.Select(x => x.ToDisplayString()))}";
                    any = true;
                }

                if (argument.HasConstructorConstraint)
                {
                    if (any)
                    {
                        constraint += ", new()";
                    }
                    else
                    {
                        constraint += $"\r\n\t\twhere {argument.ToDisplayString()} : new()";
                    }
                }
            }
        }

        return $"{(method.ReturnsVoid ? "void" : method.ReturnType.ToDisplayString())} {method.Name}{generics}({parameters}){constraint}".Replace("?", "");
    }

    static private String TransformParameterForCall(IParameterSymbol parameter)
    {
        StringBuilder builder = new();
        if (parameter.RefKind is RefKind.Out)
        {
            builder.Append("out ");
        }
        else if (parameter.RefKind is RefKind.Ref)
        {
            builder.Append("ref ");
        }

        builder.Append(parameter.Name);
        return builder.ToString();
    }

    private readonly Lazy<String> m_Call;
    private readonly Lazy<String> m_Signature;
    private readonly Lazy<String> m_SignatureWithNames;
}