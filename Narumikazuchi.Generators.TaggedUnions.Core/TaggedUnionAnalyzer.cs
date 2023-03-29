namespace Narumikazuchi.Generators.TaggedUnions.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed partial class TaggedUnionAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        m_Marked.Clear();
        m_TaggedUnions.Clear();

        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(action: this.Analyze,
                                         SyntaxKind.Attribute);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = new DiagnosticDescriptor[]
    {
        s_TypeIsNullDescriptor,
        s_DuplicateTypeDescriptor,
        s_TypenameIsNullDescriptor,
        s_TypenameIsEmptyDescriptor,
        s_TypenameHasWhitespaceDescriptor,
        s_TypenameExistsDescriptor,
        s_UnionExistsDescriptor
    }.ToImmutableArray();

    private void Analyze(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is AttributeSyntax attribute)
        {
            this.AnalyzeAttribute(context: context,
                                  attribute: attribute);
        }
    }

    private void AnalyzeAttribute(SyntaxNodeAnalysisContext context,
                                  AttributeSyntax attribute)
    {
        IMethodSymbol? constructorSymbol = (IMethodSymbol?)context.SemanticModel.GetSymbolInfo(attribute).Symbol;
        if (constructorSymbol is null)
        {
            return;
        }

        INamedTypeSymbol attributeSymbol = constructorSymbol.ContainingType;
        String fullName = attributeSymbol.ToDisplayString();
        if (fullName is not Generators.TaggedUnionGenerator.UNION_ATTRIBUTE_FULLNAME)
        {
            return;
        }

        SemanticModel semanticModel = context.SemanticModel;
        SyntaxReference reference = attribute.GetReference();
        AttributeData data = context.Compilation.Assembly.GetAttributes()
                                                         .Single(x => reference.SyntaxTree == x.ApplicationSyntaxReference?.SyntaxTree &&
                                                                      reference.Span == x.ApplicationSyntaxReference?.Span);

        ImmutableArray<AttributeData> otherDeclarations = context.Compilation.Assembly.GetAttributes()
                                                                                      .Where(data => data.AttributeClass is not null &&
                                                                                                     data.AttributeClass.ToDisplayString() is Generators.TaggedUnionGenerator.UNION_ATTRIBUTE_FULLNAME &&
                                                                                                     (reference.SyntaxTree != data.ApplicationSyntaxReference?.SyntaxTree ||
                                                                                                     reference.Span != data.ApplicationSyntaxReference?.Span))
                                                                                      .ToImmutableArray();

        __TaggedUnionParameters unionParameters = __TaggedUnionParameters.FromAttributeData(syntax: attribute,
                                                                                            data: data,
                                                                                            diagnostics: out List<Diagnostic> diagnostics);

        if (diagnostics.Count > 0)
        {
            foreach (Diagnostic diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }

            return;
        }

        ImmutableArray<__TaggedUnionParameters> otherUnions = otherDeclarations.Select(GetUnionParameters)
                                                                               .ToImmutableArray();

        ImmutableArray<__TaggedUnionParameters> nameDuplicates = otherUnions.Where(other => other.Typename == unionParameters.Typename)
                                                                            .ToImmutableArray();
        if (nameDuplicates.Length > 0)
        {
            context.ReportDiagnostic(CreateTypenameExistsDiagnostic(attribute));
        }

        ImmutableArray<__TaggedUnionParameters> typeDuplicates = otherUnions.Where(other => TypesListsAreEqual(other, unionParameters))
                                                                            .ToImmutableArray();
        if (typeDuplicates.Length > 0)
        {
            foreach (__TaggedUnionParameters typeDuplicate in typeDuplicates)
            {
                context.ReportDiagnostic(CreateUnionExistsDiagnostic(attribute: attribute,
                                                                     typename: typeDuplicate.Typename));
            }
        }
    }

    static private __TaggedUnionParameters GetUnionParameters(AttributeData data)
    {
        if (data.ApplicationSyntaxReference is null)
        {
            return default;
        }

        AttributeSyntax syntax = (AttributeSyntax)data.ApplicationSyntaxReference.GetSyntax();
        return __TaggedUnionParameters.FromAttributeData(syntax: syntax,
                                                         data: data,
                                                         diagnostics: out _);
    }

    static private Boolean TypesListsAreEqual(__TaggedUnionParameters left,
                                              __TaggedUnionParameters right)
    {
        if (left.Types.Count > right.Types.Count)
        {
            return !left.Types.Except(right.Types)
                              .Any();
        }
        else
        {
            return !right.Types.Except(left.Types)
                               .Any();
        }
    }

    private readonly List<String> m_Marked = new();
    private readonly List<__TaggedUnionParameters> m_TaggedUnions = new();
}