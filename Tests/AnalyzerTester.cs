using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generators.TaggedUnions.Analyzers;

public sealed class AnalyzerTester : CSharpAnalyzerTest<TaggedUnionAnalyzer, MSTestVerifier>
{
    public AnalyzerTester()
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
        AnalyzerTester test = new()
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