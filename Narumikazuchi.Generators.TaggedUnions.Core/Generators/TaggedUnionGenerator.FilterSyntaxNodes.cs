using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Narumikazuchi.Generators.TaggedUnions.Generators;

public partial class TaggedUnionGenerator
{
    static private Boolean IsEligableSyntax(SyntaxNode syntaxNode,
                                            CancellationToken cancellationToken = default)
    {
        return syntaxNode is AttributeListSyntax;
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
            if (context.SemanticModel.GetSymbolInfo(node: attribute,
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
}