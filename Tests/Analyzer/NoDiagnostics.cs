namespace Tests.Analyzer;

#pragma warning disable IDE1006 // No need to add postfix 'Asynchronously' here
[TestClass]
public class NoDiagnostics
{
    [TestMethod]
    public async Task UnionOf2Types()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32))]";

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source);
    }

    [TestMethod]
    public async Task UnionOf2TypesNamed()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32), Typename = ""StringOrInt"")]";

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source);
    }

    [TestMethod]
    public async Task UnionOfMoreThan2Types()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(SByte), typeof(Int16), typeof(Int32), typeof(Int64))]";

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source);
    }

    [TestMethod]
    public async Task UnionOfMoreThan2TypesNamed()
    {
        String source = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(SByte), typeof(Int16), typeof(Int32), typeof(Int64), Typename = ""SignedInteger"")]";

        await AnalyzerTest.VerifyAnalyzerAsynchronously(source);
    }
}