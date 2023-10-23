using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Narumikazuchi.Generators.TaggedUnions.Analyzers;

namespace Narumikazuchi.Generators.TaggedUnions;

internal record struct __TaggedUnionParameters(AttributeSyntax Syntax, String Typename, HashSet<ITypeSymbol> Types)
{
    internal static __TaggedUnionParameters FromAttributeData(AttributeSyntax syntax,
                                                              AttributeData data,
                                                              out List<Diagnostic> diagnostics)
    {
        diagnostics = new();
        HashSet<ITypeSymbol> types = new(SymbolEqualityComparer.Default);

        if (data.ConstructorArguments[0].IsNull)
        {
            diagnostics.Add(TaggedUnionAnalyzer.CreateTypeIsNullDiagnostic(attribute: syntax,
                                                                           index: 0));
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[0].Value!))
        {
            diagnostics.Add(TaggedUnionAnalyzer.CreateDuplicateTypeDiagnostic(attribute: syntax,
                                                                              index: 0));
        }

        if (data.ConstructorArguments[1].IsNull)
        {
            diagnostics.Add(TaggedUnionAnalyzer.CreateTypeIsNullDiagnostic(attribute: syntax,
                                                                           index: 1));
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[1].Value!))
        {
            diagnostics.Add(TaggedUnionAnalyzer.CreateDuplicateTypeDiagnostic(attribute: syntax,
                                                                              index: 1));
        }

        if (data.ConstructorArguments.Length > 2)
        {
            Int32 index = 2;
            if (data.ConstructorArguments[2].IsNull)
            {
                diagnostics.Add(TaggedUnionAnalyzer.CreateTypeIsNullDiagnostic(attribute: syntax,
                                                                               index: index));
            }
            else
            {
                foreach (TypedConstant type in data.ConstructorArguments[2].Values)
                {
                    if (type.IsNull)
                    {
                        diagnostics.Add(TaggedUnionAnalyzer.CreateTypeIsNullDiagnostic(attribute: syntax,
                                                                                       index: index));
                        continue;
                    }

                    if (!types.Add((ITypeSymbol)type.Value!))
                    {
                        diagnostics.Add(TaggedUnionAnalyzer.CreateDuplicateTypeDiagnostic(attribute: syntax,
                                                                                          index: index));
                    }

                    index++;
                }
            }
        }

        String name;
        if (data.NamedArguments.Length > 0)
        {
            KeyValuePair<String, TypedConstant> typename = data.NamedArguments.Single(x => x.Key is "Typename");
            if (typename.Value.IsNull)
            {
                diagnostics.Add(TaggedUnionAnalyzer.CreateTypenameIsNullDiagnostic(syntax));
                return default;
            }

            name = (String)typename.Value.Value!;
            if (String.IsNullOrWhiteSpace(name))
            {
                diagnostics.Add(TaggedUnionAnalyzer.CreateTypenameIsEmptyDiagnostic(syntax));
                return default;
            }

            if (name.IndexOf(' ') > -1)
            {
                diagnostics.Add(TaggedUnionAnalyzer.CreateTypenameHasWhitespaceDiagnostic(syntax));
                return default;
            }

            if (!name.IsValidCSharpTypename())
            {
                diagnostics.Add(TaggedUnionAnalyzer.CreateInvalidTypenameDiagnostic(syntax));
                return default;
            }
        }
        else
        {
            name = $"UnionOf_{String.Join("_", types.Select(x => x.Name))}";
        }

        return new(Syntax: syntax,
                   Typename: name,
                   Types: types);
    }

    internal readonly Boolean ContentsEqual(__TaggedUnionParameters other)
    {
        if (this.Types.Count != other.Types.Count)
        {
            return false;
        }

        foreach (ITypeSymbol type in this.Types)
        {
            if (!other.Types.Contains(type))
            {
                return false;
            }
        }

        return true;
    }
}