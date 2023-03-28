﻿namespace Narumikazuchi.Generators.TaggedUnions.Generators;

[Generator]
public sealed partial class TaggedUnionGenerator
{
    static private Boolean IsEligableSyntax(SyntaxNode syntaxNode,
                                            CancellationToken cancellationToken = default)
    {
        return syntaxNode is AttributeListSyntax;
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
            // Impossible
            return default!;
        }
    }

    static private Boolean ExcludeSpecialNames(ISymbol symbol)
    {
        if (symbol is IMethodSymbol method)
        {
            if (method.MethodKind is not MethodKind.Ordinary ||
                method.Name.StartsWith(nameof(MemberwiseClone)) ||
                method.Name.StartsWith(nameof(GetType)))
            {
                return false;
            }
        }

        return true;
    }

    static private AttributeSyntax[] Transform(GeneratorSyntaxContext context,
                                               CancellationToken cancellationToken = default)
    {
        if (context.Node is not AttributeListSyntax attributeList)
        {
            return Array.Empty<AttributeSyntax>();
        }

        List<AttributeSyntax> attributes = new();
        foreach (AttributeSyntax attribute in attributeList.Attributes)
        {
            if (context.SemanticModel.GetSymbolInfo(attributeSyntax: attribute,
                                                    cancellationToken: cancellationToken)
                                     .Symbol is not IMethodSymbol constructorSymbol)
            {
                continue;
            }

            INamedTypeSymbol attributeSymbol = constructorSymbol.ContainingType;
            String fullName = attributeSymbol.ToDisplayString();
            if (fullName is UNION_ATTRIBUTE_FULLNAME)
            {
                attributes.Add(attribute);
            }
        }

        return attributes.ToArray();
    }

    static private SourceText GenerateStruct(String name,
                                             HashSet<ITypeSymbol> types)
    {
        Stopwatch watch = Stopwatch.StartNew();
        IEnumerable<__ISignature> members = Array.Empty<__ISignature>();

        foreach (ITypeSymbol type in types)
        {
            IEnumerable<__ISignature> myMembers = type.GetMembers()
                                                      .Where(x => !x.IsStatic && x.DeclaredAccessibility is Accessibility.Public)
                                                      .Where(x => x is IMethodSymbol or IPropertySymbol)
                                                      .Where(ExcludeSpecialNames)
                                                      .Select(FormatMemberSignature)
                                                      .Distinct();
            ITypeSymbol? baseType = type.BaseType;
            while (baseType is not null)
            {
                IEnumerable<__ISignature> baseMembers = baseType.GetMembers()
                                                                .Where(x => !x.IsStatic)
                                                                .Where(x => x is IMethodSymbol or IPropertySymbol)
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

        __ISignature[] commonMembers = members.ToArray();

        String operators = GenerateOperators(name: name,
                                             types: types);
        String methods = GenerateMethods(members: commonMembers,
                                         types: types);
        String properties = GenerateProperties(members: commonMembers,
                                               types: types);
        String internals = GenerateInternals(name: name,
                                             types: types);

        watch.Stop();
        Console.WriteLine($"Generation of {name} took {watch.ElapsedMilliseconds}ms.");

        String source = $@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Code generation took {watch.ElapsedMilliseconds}ms.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#pragma warning disable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Narumikazuchi.TypeExtensions.Generated;

/// <summary>
/// A type that can be assigned either of these types: <code>{String.Join(" | ", types.Select(x => $"<see cref=\"{x.ToDisplayString().Replace('<', '{').Replace('>', '}')}\"/>"))}</code>
/// </summary>
[CompilerGenerated]
public readonly struct {name}
{{
{operators}{methods}{properties}{internals}
}}";
        SourceText text = SourceText.From(text: source,
                                          encoding: Encoding.UTF8);
        return text;
    }

    private void GenerateStructs(SourceProductionContext context,
                                 (Compilation, ImmutableArray<AttributeSyntax[]>) compilationAndAttributes)
    {
        m_TaggedUnions.Clear();
        (Compilation compilation, ImmutableArray<AttributeSyntax[]> compactAttributes) = compilationAndAttributes;
        if (compactAttributes.IsDefaultOrEmpty)
        {
            return;
        }

        AttributeSyntax[] attributes = compactAttributes.SelectMany(x => x)
                                                        .ToArray();

        foreach (AttributeSyntax attribute in attributes)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(attribute.SyntaxTree);
            SyntaxReference reference = attribute.GetReference();
            AttributeData data = semanticModel.Compilation.Assembly.GetAttributes()
                                                                   .Single(x => reference.SyntaxTree == x.ApplicationSyntaxReference?.SyntaxTree &&
                                                                                reference.Span == x.ApplicationSyntaxReference?.Span);

            this.GenerateStruct(context, data);
        }
    }

    private void GenerateStruct(SourceProductionContext context,
                                AttributeData data)
    {
        String name;
        SourceText source;
        if (m_Cache.TryGetValue(key: data.ToString(),
                                value: out (String name, SourceText source) tuple))
        {
            (name, source) = tuple;
            context.AddSource(hintName: $"{name}.g.cs",
                              sourceText: source);
            return;
        }

        HashSet<ITypeSymbol> types = new(SymbolEqualityComparer.Default);

        Boolean emit = true;
        if (data.ConstructorArguments[0].IsNull)
        {
            emit = false;
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[0].Value!))
        {
            emit = false;
        }

        if (data.ConstructorArguments[1].IsNull)
        {
            emit = false;
        }
        else if (!types.Add((ITypeSymbol)data.ConstructorArguments[1].Value!))
        {
            emit = false;
        }

        if (data.ConstructorArguments.Length > 2 &&
            !data.ConstructorArguments[2].IsNull)
        {
            Int32 index = 2;
            foreach (TypedConstant type in data.ConstructorArguments[2].Values)
            {
                if (type.IsNull)
                {
                    emit = false;
                    continue;
                }

                if (!types.Add((ITypeSymbol)type.Value!))
                {
                    emit = false;
                }

                index++;
            }
        }

        if (data.NamedArguments.Length > 0)
        {
            KeyValuePair<String, TypedConstant> typename = data.NamedArguments.Single(x => x.Key is "Typename");
            if (typename.Value.IsNull)
            {
                emit = false;
                return;
            }

            name = (String)typename.Value.Value!;
        }
        else
        {
            name = $"UnionOf_{String.Join("_", types.Select(x => x.Name))}";
        }

        __TaggedUnionParameters unionParameters = new(Syntax: null,
                                                      Typename: name,
                                                      Types: types);

        if (m_TaggedUnions.Select(x => x.Typename)
                          .Contains(name))
        {
            emit = false;
        }

        __TaggedUnionParameters potentialDuplicate = m_TaggedUnions.FirstOrDefault(x => x.ContentsEqual(unionParameters));
        if (potentialDuplicate.Typename is not null)
        {
            emit = false;
        }

        if (emit)
        {
            source = GenerateStruct(name: name,
                                    types: types);
            m_Cache.Add(key: data.ToString(),
                        value: (name, source));
            m_TaggedUnions.Add(unionParameters);
            try
            {
                context.AddSource(hintName: $"{name}.g.cs",
                                  sourceText: source);
            }
            catch { }
        }
    }

    private readonly Dictionary<String, (String name, SourceText source)> m_Cache = new();
    private readonly List<__TaggedUnionParameters> m_TaggedUnions = new();

    internal const String UNION_ATTRIBUTE_FULLNAME = "Narumikazuchi.TypeExtensions.UnionOfAttribute";
}