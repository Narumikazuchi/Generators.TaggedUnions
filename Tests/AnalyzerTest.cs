using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generators.TaggedUnions.Analyzers;

namespace Tests;

public sealed class AnalyzerTest : CSharpAnalyzerTest<TaggedUnionAnalyzer, MSTestVerifier>
{
    public AnalyzerTest()
    {
        this.SolutionTransforms.Add((solution, projectId) =>
        {
            CompilationOptions compilationOptions = solution.GetProject(projectId)!.CompilationOptions!;
            compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(compilationOptions.SpecificDiagnosticOptions.SetItem("CS1705", ReportDiagnostic.Hidden));
            solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

            return solution;
        });
    }

    static public async Task VerifyAnalyzerAsynchronously(String source,
                                                          params DiagnosticResult[] expected)
    {
        AnalyzerTest test = new()
        {
            TestState =
            {
                Sources = { source },
                AdditionalReferences = { typeof(UnionOfAttribute).Assembly.Location.Replace("net6.0", "netstandard2.0") }
            }
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }
}