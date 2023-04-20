using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Narumikazuchi.Generators.TaggedUnions.Generators;

public partial class TaggedUnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<AttributeSyntax[]> attributes = context.SyntaxProvider.CreateSyntaxProvider(predicate: IsEligableSyntax,
                                                                                                              transform: Transform)
                                                                                        .Where(static x => x.Length > 0);

        IncrementalValueProvider<(Compilation, ImmutableArray<AttributeSyntax[]>)> compilationAndAttributes = context.CompilationProvider.Combine(attributes.Collect());
        context.RegisterSourceOutput(source: compilationAndAttributes,
                                     action: GenerateUnions);
    }
}