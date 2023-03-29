namespace Tests.Analyzer;

#pragma warning disable IDE1006 // No need to add postfix 'Asynchronously' here
[TestClass]
public class TypenameDiagnostic
{
    [TestMethod]
    public async Task TypenameIsNull()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = null)]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG003", DiagnosticSeverity.Error).WithLocation(4, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task TypenameIsEmpty()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = """")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG004", DiagnosticSeverity.Error).WithLocation(4, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task TypenameIsWhitespace()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""  "")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG004", DiagnosticSeverity.Error).WithLocation(4, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task TypenameHasWhitespace()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""Some Name"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG005", DiagnosticSeverity.Error).WithLocation(4, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task DuplicateTypename()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""StringOrInt"")]
[assembly: UnionOf(typeof(String), typeof(Int64), Typename = ""StringOrInt"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG006", DiagnosticSeverity.Error).WithLocation(4, 51),
            new DiagnosticResult("NCG006", DiagnosticSeverity.Error).WithLocation(5, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }

    [TestMethod]
    public async Task TypenameIsInvalid()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""2Types"")]";

        DiagnosticResult[] results = new[]
        {
            new DiagnosticResult("NCG008", DiagnosticSeverity.Error).WithLocation(4, 51)
        };

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source, results);
    }
}