using Microsoft.CodeAnalysis;

namespace Narumikazuchi.Generators.TaggedUnions.Generators;

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

    public readonly override String ToString()
    {
        return m_Signature.Value;
    }

    public readonly Boolean Equals(__ISignature other)
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

    static public __MethodSignature ObjectEquals
    {
        get
        {
            return new(name: "Equals",
                       call: "Equals(obj)",
                       signature: "Boolean Equals(Object?)",
                       signatureWithNames: "Boolean Equals(Object? obj)",
                       returnsVoid: false);
        }
    }

    static public __MethodSignature ObjectGetHashCode
    {
        get
        {
            return new(name: "GetHashCode",
                       call: "GetHashCode()",
                       signature: "Int32 GetHashCode()",
                       signatureWithNames: "Int32 GetHashCode()",
                       returnsVoid: false);
        }
    }

    static public __MethodSignature ObjectToString
    {
        get
        {
            return new(name: "ToString",
                       call: "ToString()",
                       signature: "String ToString()",
                       signatureWithNames: "String ToString()",
                       returnsVoid: false);
        }
    }

    public String Name { get; }

    public Boolean ReturnsVoid { get; }

    public Boolean IsVirtual { get; }

    internal __MethodSignature(IMethodSymbol method)
    {
        m_Call = new(() => GenerateCallString(method));
        m_Signature = new(() => GenerateSignatureString(method));
        m_SignatureWithNames = new(() => GenerateSignatureString(method, true));
        this.ReturnsVoid = method.ReturnsVoid;
        this.IsVirtual = false;
        this.Name = method.Name;
    }

    private __MethodSignature(String name,
                              String call,
                              String signature,
                              String signatureWithNames,
                              Boolean returnsVoid)
    {

        m_Call = new(() => call);
        m_Signature = new(() => signature);
        m_SignatureWithNames = new(() => signatureWithNames);
        this.ReturnsVoid = returnsVoid;
        this.IsVirtual = true;
        this.Name = name;
    }

    static private String GenerateCallString(IMethodSymbol method)
    {
        String parameters = String.Join(", ", method.Parameters.Select(TransformParameterForCall));
        return $"{method.Name}({parameters})";
    }

    static private String GenerateSignatureString(IMethodSymbol method,
                                                  Boolean includeNames = default)
    {
        String parameters;
        if (includeNames)
        {
            parameters = String.Join(", ", method.Parameters.Select(TransformParameterForSignatureWithName));
        }
        else
        {
            parameters = String.Join(", ", method.Parameters.Select(TransformParameterForSignature));
        }

        String generics = String.Empty;
        String constraint = String.Empty;
        if (method.IsGenericMethod)
        {
            generics = $"<{String.Join(", ", method.TypeParameters.Select(parameter => parameter.ToFrameworkString()))}>";
            foreach (ITypeParameterSymbol argument in method.TypeArguments.OfType<ITypeParameterSymbol>())
            {
                Boolean anySpecial = false;
                Boolean any = false;
                if (argument.HasNotNullConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : notnull";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasReferenceTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : class";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasUnmanagedTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : unmanaged";
                    anySpecial = true;
                    any = true;
                }
                else if (argument.HasValueTypeConstraint)
                {
                    constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : struct";
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
                        constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : ";
                    }

                    constraint += $"{String.Join(", ", argument.ConstraintTypes.Select(type => type.ToFrameworkString()))}";
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
                        constraint += $"\r\n\t\twhere {argument.ToFrameworkString()} : new()";
                    }
                }
            }
        }

        return $"{(method.ReturnsVoid ? "void" : method.ReturnType.ToFrameworkString())} {method.Name}{generics}({parameters}){constraint}";
    }

    static private String TransformParameterForSignature(IParameterSymbol parameter)
    {
        StringBuilder builder = new();
        if (parameter.RefKind is RefKind.Out)
        {
            builder.Append("out ");
        }
        else if (parameter.RefKind is RefKind.In)
        {
            builder.Append("in ");
        }
        else if (parameter.RefKind is RefKind.Ref)
        {
            builder.Append("ref ");
        }

        builder.Append(parameter.Type.ToFrameworkString());
        return builder.ToString();
    }

    static private String TransformParameterForSignatureWithName(IParameterSymbol parameter)
    {
        StringBuilder builder = new();
        if (parameter.RefKind is RefKind.Out)
        {
            builder.Append("out ");
        }
        else if (parameter.RefKind is RefKind.In)
        {
            builder.Append("in ");
        }
        else if (parameter.RefKind is RefKind.Ref)
        {
            builder.Append("ref ");
        }

        builder.Append(parameter.Type.ToFrameworkString());
        builder.Append(' ');
        builder.Append(parameter.Name);
        return builder.ToString();
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