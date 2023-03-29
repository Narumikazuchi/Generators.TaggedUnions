using Microsoft.CodeAnalysis.Text;
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generators.TaggedUnions.Generators;

namespace Tests;

public sealed class GeneratorTest : SourceGeneratorTest<MSTestVerifier>
{
    static public async Task VerifySourceGeneratorAsynchronously(String source,
                                                                 params (String filename, SourceText content)[] expected)
    {
        GeneratorTest test = new()
        {
            TestState =
            {
                Sources = { source },
                AdditionalReferences = { typeof(UnionOfAttribute).Assembly.Location.Replace("net6.0", "netstandard2.0") }
            }
        };
        foreach ((String filename, SourceText content) item in expected)
        {
            test.TestState.GeneratedSources.Add(item);
        }

        await test.RunAsync(CancellationToken.None);
    }

    public override String Language
    {
        get
        {
            return LanguageNames.CSharp;
        }
    }

    protected override CompilationOptions CreateCompilationOptions()
    {
        return new CSharpCompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary,
                                            allowUnsafe: true);
    }

    protected override GeneratorDriver CreateGeneratorDriver(Project project,
                                                             ImmutableArray<ISourceGenerator> sourceGenerators)
    {
        return CSharpGeneratorDriver.Create(generators: sourceGenerators,
                                            additionalTexts: project.AnalyzerOptions.AdditionalFiles,
                                            parseOptions: (CSharpParseOptions)project.ParseOptions!,
                                            optionsProvider: project.AnalyzerOptions.AnalyzerConfigOptionsProvider);
    }

    protected override ParseOptions CreateParseOptions()
    {
        return new CSharpParseOptions(languageVersion: s_DefaultLanguageVersion,
                                      documentationMode: DocumentationMode.Diagnose);
    }

    protected override IEnumerable<ISourceGenerator> GetSourceGenerators()
    {
        return new ISourceGenerator[] { new TaggedUnionGenerator().AsSourceGenerator() };
    }

    protected override String DefaultFileExt
    {
        get
        {
            return "cs";
        }
    }

    static private readonly LanguageVersion s_DefaultLanguageVersion =
        Enum.TryParse("Default", out LanguageVersion version) ? version : LanguageVersion.CSharp7_3;
}