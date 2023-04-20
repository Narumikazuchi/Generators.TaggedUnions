using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Narumikazuchi.Generators.TaggedUnions.Generators;

[Generator(LanguageNames.CSharp)]
public sealed partial class TaggedUnionGenerator
{
    internal const String UNION_ATTRIBUTE_FULLNAME = "Narumikazuchi.Generators.TaggedUnions.UnionOfAttribute";

    static private void GenerateUnions(SourceProductionContext context,
                                       (Compilation, ImmutableArray<AttributeSyntax[]>) compilationAndAttributes)
    {
        (Compilation compilation, ImmutableArray<AttributeSyntax[]> compactAttributes) = compilationAndAttributes;
        if (compactAttributes.IsDefaultOrEmpty)
        {
            return;
        }

        AttributeSyntax[] attributes = compactAttributes.SelectMany(attribute => attribute)
                                                        .ToArray();

        List<__TaggedUnionParameters> generated = new();
        foreach (AttributeSyntax attribute in attributes)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(attribute.SyntaxTree);
            SyntaxReference reference = attribute.GetReference();
            AttributeData data = semanticModel.Compilation.Assembly.GetAttributes()
                                                                   .Single(data => reference.SyntaxTree == data.ApplicationSyntaxReference?.SyntaxTree &&
                                                                                   reference.Span == data.ApplicationSyntaxReference?.Span);

            Boolean success = GenerateStruct(data: data,
                                             result: out __TaggedUnionParameters result,
                                             source: out SourceText source);

            if (!success)
            {
                continue;
            }

            if (generated.Any(union => union.Typename == result.Typename))
            {
                continue;
            }

            if (generated.Any(union => union.ContentsEqual(result)))
            {
                continue;
            }

            context.AddSource(hintName: $"Narumikazuchi.Generated.{result.Typename}.g.cs",
                              sourceText: source);
            generated.Add(result);
        }
    }

    static private Boolean GenerateStruct(AttributeData data,
                                          out __TaggedUnionParameters result,
                                          out SourceText source)
    {
        String name;
        HashSet<ITypeSymbol> types = new(SymbolEqualityComparer.Default);

        if (data.ConstructorArguments[0].IsNull)
        {
            result = default;
            source = default;
            return false;
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[0].Value))
        {
            result = default;
            source = default;
            return false;
        }

        if (data.ConstructorArguments[1].IsNull)
        {
            result = default;
            source = default;
            return false;
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[1].Value))
        {
            result = default;
            source = default;
            return false;
        }

        if (data.ConstructorArguments.Length > 2 &&
            !data.ConstructorArguments[2].IsNull)
        {
            Int32 index = 2;
            foreach (TypedConstant type in data.ConstructorArguments[2].Values)
            {
                if (type.IsNull)
                {
                    result = default;
                    source = default;
                    return false;
                }

                if (!types.Add((ITypeSymbol)type.Value))
                {
                    result = default;
                    source = default;
                    return false;
                }

                index++;
            }
        }

        if (data.NamedArguments.Length > 0)
        {
            KeyValuePair<String, TypedConstant> typename = data.NamedArguments.Single(argument => argument.Key is "Typename");
            if (typename.Value.IsNull)
            {
                result = default;
                source = default;
                return false;
            }

            name = (String)typename.Value.Value;
            if (String.IsNullOrWhiteSpace(name))
            {
                result = default;
                source = default;
                return false;
            }

            if (name.IndexOf(' ') > -1)
            {
                result = default;
                source = default;
                return false;
            }

            if (!name.IsValidCSharpTypename())
            {
                result = default;
                source = default;
                return false;
            }
        }
        else
        {
            name = $"UnionOf_{String.Join("_", types.Select(type => type.Name))}";
        }

        result = new __TaggedUnionParameters(Syntax: null,
                                             Typename: name,
                                             Types: types);

        source = GenerateSource(name: name,
                                types: types);

        return true;
    }

    static private SourceText GenerateSource(String name,
                                             HashSet<ITypeSymbol> types)
    {
        IEnumerable<__ISignature> members = Array.Empty<__ISignature>();

        foreach (ITypeSymbol type in types)
        {
            IEnumerable<__ISignature> myMembers = type.GetMembers()
                                                      .Where(member => !member.IsStatic &&
                                                                       member.DeclaredAccessibility is Accessibility.Public)
                                                      .Where(member => member is IMethodSymbol
                                                                              or IPropertySymbol)
                                                      .Where(ExcludeSpecialNames)
                                                      .Select(FormatMemberSignature)
                                                      .Distinct();
            ITypeSymbol baseType = type.BaseType;
            while (baseType is not null)
            {
                IEnumerable<__ISignature> baseMembers = baseType.GetMembers()
                                                                .Where(member => !member.IsStatic &&
                                                                                 member.DeclaredAccessibility is Accessibility.Public)
                                                                .Where(member => member is IMethodSymbol
                                                                                        or IPropertySymbol)
                                                                .Where(ExcludeSpecialNames)
                                                                .Select(FormatMemberSignature)
                                                                .Distinct();
                myMembers = myMembers.Concat(baseMembers);
                baseType = baseType.BaseType;
            }

            if (members is __ISignature[])
            {
                members = myMembers;
            }
            else
            {
                members = members.Intersect(myMembers);
            }
        }

        __ISignature[] commonMembers = members.Concat(new __ISignature[]
        {
            __MethodSignature.ObjectEquals,
            __MethodSignature.ObjectGetHashCode,
            __MethodSignature.ObjectToString
        }).OrderBy(signature => signature, new __SignatureComparer())
          .ToArray();

        String operators = GenerateOperators(name: name,
                                             types: types);
        String methods = GenerateMethods(members: commonMembers,
                                         types: types);
        String properties = GenerateProperties(members: commonMembers,
                                               types: types);
        String internals = GenerateInternals(name: name,
                                             types: types);

        String source = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Narumikazuchi.Generated
{{
    /// <summary>
    /// A type that can be assigned either of these types: <code>{String.Join(" | ", types.Select(type => $"<see cref=\"{type.ToDisplayString().Replace('<', '{').Replace('>', '}')}\"/>"))}</code>
    /// </summary>
    [CompilerGenerated]
    public readonly struct {name}
    {{
{operators}{methods}{properties}{internals}
    }}
}}";
        SourceText text = SourceText.From(text: source,
                                          encoding: Encoding.UTF8);
        return text;
    }

    static private __ISignature FormatMemberSignature(ISymbol member)
    {
        if (member is IMethodSymbol method)
        {
            return new __MethodSignature(method);
        }
        else if (member is IPropertySymbol property)
        {
            return new __PropertySignature(property);
        }
        else
        {
            // Filtered beforehand, Impossible
            return default!;
        }
    }

    static private Boolean ExcludeSpecialNames(ISymbol symbol)
    {
        if (symbol is IMethodSymbol method)
        {
            if (method.MethodKind is not MethodKind.Ordinary ||
                (method.Name.StartsWith(nameof(Equals)) &&
                method.Parameters.Length == 1 &&
                method.Parameters[0].Type.Name is nameof(Object)) ||
                method.Name.StartsWith(nameof(MemberwiseClone)) ||
                method.Name.StartsWith(nameof(GetHashCode)) ||
                method.Name.StartsWith(nameof(GetType)) ||
                (method.Name.StartsWith(nameof(ToString)) &&
                method.Parameters.Length == 0))
            {
                return false;
            }
        }

        return true;
    }
}