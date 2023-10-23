namespace Analysis;

[TestClass]
public sealed class A_union_of
{
    [TestClass]
    public sealed class at_least_one_null_value_for_a_type
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), null)]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG001").WithLocation(4, 35)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_identical_types
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Int32))]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG002").WithLocation(4, 35)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_null_as_typename
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = null)]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG003").WithLocation(4, 51)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_an_empty_string_as_typename
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = """")]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG004").WithLocation(4, 51)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_a_string_with_whitespaces_as_typename
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = ""Type Name"")]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG005").WithLocation(4, 51)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_a_duplicate_typename
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = ""TypeName"")]
[assembly: UnionOf(typeof(Int32), typeof(Single), Typename = ""TypeName"")]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG006").WithLocation(4, 51),
                DiagnosticResult.CompilerError("NCG006").WithLocation(5, 51)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_a_duplicate_union
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = ""TypeName"")]
[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = ""TypeNameX"")]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG007").WithLocation(4, 12),
                DiagnosticResult.CompilerError("NCG007").WithLocation(5, 12)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }

    [TestClass]
    public sealed class two_or_more_types_with_an_invalid_typename
    {
        [TestMethod]
        public Task will_not_compile()
        {
            const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), Typename = ""123"")]";

            const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

            TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                              executionSource: methodBodySource);

            Assert.IsFalse(results.Compiles, "This should never compile.");
            Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);

            DiagnosticResult[] expectedDiagnostics = new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("NCG008").WithLocation(4, 51)
            };
            return AnalyzerTester.VerifyAnalyzerAsynchronously(assemblySource, expectedDiagnostics);
        }
    }
}