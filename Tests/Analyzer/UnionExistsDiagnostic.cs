namespace Tests.Analyzer;

#pragma warning disable IDE1006 // No need to add postfix 'Asynchronously' here
[TestClass]
public class UnionExistsDiagnostic
{
    [TestMethod]
    public async Task UnionOf2TypesDuplicate()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""First"")]
[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""Second"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(4, 12),
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(5, 12)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task UnionOf2TypesSwitchedDuplicate()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""First"")]
[assembly: UnionOf(typeof(Int32), typeof(String), Typename = ""Second"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(4, 12),
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(5, 12)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task UnionOf3TypesDuplicate()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), typeof(Int64), Typename = ""First"")]
[assembly: UnionOf(typeof(String), typeof(Int32), typeof(Int64), Typename = ""Second"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(4, 12),
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(5, 12)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task UnionOf3TypesSwitchedDuplicate()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), typeof(Int64), Typename = ""First"")]
[assembly: UnionOf(typeof(Int64), typeof(String), typeof(Int32), Typename = ""Second"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(4, 12),
            new DiagnosticResult("NCG007", DiagnosticSeverity.Error).WithLocation(5, 12)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }
}