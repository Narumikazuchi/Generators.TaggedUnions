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
#if NET6_0
                ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
#elif NET7_0
                ReferenceAssemblies = s_Net7Assemblies,
#endif
                AdditionalReferences =
                {
                    typeof(UnionOfAttribute).Assembly.Location
                }
            }
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None);
    }

#if NET7_0
    static private readonly ReferenceAssemblies s_Net7Assemblies = new("net7.0",
                                                                       new PackageIdentity("Microsoft.NETCore.App.Ref",
                                                                                           "7.0.4"),
                                                                       Path.Combine("ref", "net7.0"));
#endif
}