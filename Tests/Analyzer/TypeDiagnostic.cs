namespace Tests.Analyzer;

#pragma warning disable IDE1006 // No need to add postfix 'Asynchronously' here
[TestClass]
public class TypeDiagnostic
{
    [TestMethod]
    public async Task DuplicatePrimitveTypes()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Int32))]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG002", DiagnosticSeverity.Error).WithLocation(4, 35)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task DuplicateComplexTypes()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Version), typeof(Version))]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG002", DiagnosticSeverity.Error).WithLocation(4, 37)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }
    [TestMethod]
    public async Task DuplicatePrimitveTypesMoreThan2()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Int64), typeof(Int32))]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG002", DiagnosticSeverity.Error).WithLocation(4, 50)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task DuplicateComplexTypesMoreThan2()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Version), typeof(String), typeof(Version))]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG002", DiagnosticSeverity.Error).WithLocation(4, 53)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task TypeIsNull()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Version), typeof(String), null)]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG001", DiagnosticSeverity.Error).WithLocation(4, 53)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }
}