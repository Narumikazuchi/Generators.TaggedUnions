using Narumikazuchi.Generators.TaggedUnions;

namespace Generation;

[TestClass]
public sealed class A_union_of
{
    [TestClass]
    public sealed class two_value_types
    {
        private const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double))]";

        [TestClass]
        public sealed class can_be_assigned_by
        {
            [TestMethod]
            public void a_value_of_the_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)42d;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_first_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
value |= (Int32)42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
value |= 42d;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_not_be_assigned_using_binary_or_operator
        {
            [TestMethod]
            public void if_value_has_already_been_set()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
value |= (Int32)42;
if (!value.Is(out Int32? check) ||
    check != 69)
{
    throw new Exception(""Value should be the first that had been set."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
Int32? check = (Int32?)value;
if (!check.HasValue)
{
    throw new Exception(""Value should be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
Double? check = (Double?)value;
if (!check.HasValue)
{
    throw new Exception(""Value should be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_not_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
Int32? check = (Int32?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
Double? check = (Double?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_first_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
Int32? check = (Int32?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
Double? check = (Double?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_for_equals
        {
            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (!value.Equals(69))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (!value.Equals(69d))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (value.Equals(42))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (value.Equals(42d))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_type_does_not_equal_the_contained_value_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (value.Equals(69d))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_equals
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
Console.WriteLine(value.Equals(69));";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_gethashcode
        {
            [TestMethod]
            public void the_value_of_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (value.GetHashCode() != 69.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_value_of_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (value.GetHashCode() != 69d.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_gethashcode
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
Console.WriteLine(value.GetHashCode());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_tostring
        {
            [TestMethod]
            public void the_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (value.ToString() != 69.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (value.ToString() != 69d.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_tostring
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = default;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_true_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (!value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (!value.Is(out Double? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_false_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Int32)69;
if (value.Is(out Double? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double value = (Double)69d;
if (value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }
    }

    [TestClass]
    public sealed class two_reference_types
    {
        private const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Version))]";

        [TestClass]
        public sealed class can_be_assigned_by
        {
            [TestMethod]
            public void a_value_of_the_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_first_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
value |= ""Foo"";
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
value |= new Version(2, 4);
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_not_be_assigned_using_binary_or_operator
        {
            [TestMethod]
            public void if_value_has_already_been_set()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
value |= ""Foo"";
if (!value.Is(out String check) ||
    check != ""Foo"")
{
    throw new Exception(""Value should be the first that had been set."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
String check = (String)value;
if (check is null)
{
    throw new Exception(""Value should be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
Version check = (Version)value;
if (check is null)
{
    throw new Exception(""Value should be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_not_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
String check = (String)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
Version check = (Version)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_first_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
String check = (String)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
Version check = (Version)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_for_equals
        {
            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (!value.Equals(""Foo""))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (!value.Equals(new Version(2, 4)))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (value.Equals(""Bar""))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (value.Equals(new Version(4, 2)))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_type_does_not_equal_the_contained_value_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (value.Equals(new Version(2, 4)))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_equals
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
Console.WriteLine(value.Equals(""Foo""));";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_gethashcode
        {
            [TestMethod]
            public void the_value_of_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (value.GetHashCode() != ""Foo"".GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_value_of_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (value.GetHashCode() != new Version(2, 4).GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_gethashcode
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
Console.WriteLine(value.GetHashCode());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_tostring
        {
            [TestMethod]
            public void the_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (value.ToString() != ""Foo"".ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (value.ToString() != new Version(2, 4).ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_tostring
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Version value = default;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_true_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (!value.Is(out String _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (!value.Is(out Version _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_false_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = ""Foo"";
if (value.Is(out Version _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Version value = new Version(2, 4);
if (value.Is(out String _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }
    }

    [TestClass]
    public sealed class a_value_and_a_reference_type
    {
        private const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(String), typeof(Int32))]";

        [TestClass]
        public sealed class can_be_assigned_by
        {
            [TestMethod]
            public void a_value_of_the_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_first_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
value |= ""Foo"";
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
value |= 42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_not_be_assigned_using_binary_or_operator
        {
            [TestMethod]
            public void if_value_has_already_been_set()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
value |= ""Foo"";
if (!value.Is(out String check) ||
    check != ""Foo"")
{
    throw new Exception(""Value should be the first that had been set."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
String check = (String)value;
if (check is null)
{
    throw new Exception(""Value should be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
Int32? check = (Int32?)value;
if (check is null)
{
    throw new Exception(""Value should be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_not_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
String check = (String)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
Int32? check = (Int32?)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_first_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
String check = (String)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
Int32? check = (Int32?)value;
if (check is not null)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_for_equals
        {
            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (!value.Equals(""Foo""))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (!value.Equals(42))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (value.Equals(""Bar""))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (value.Equals(69))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_type_does_not_equal_the_contained_value_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (value.Equals(42))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_equals
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
Console.WriteLine(value.Equals(""Foo""));";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_gethashcode
        {
            [TestMethod]
            public void the_value_of_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (value.GetHashCode() != ""Foo"".GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_value_of_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (value.GetHashCode() != 42.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_gethashcode
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
Console.WriteLine(value.GetHashCode());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_tostring
        {
            [TestMethod]
            public void the_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (value.ToString() != ""Foo"".ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (value.ToString() != 42.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_tostring
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = default;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_true_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (!value.Is(out String _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (!value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_false_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = ""Foo"";
if (value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_String_Int32 value = 42;
if (value.Is(out String _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }
    }

    [TestClass]
    public sealed class three_or_more_types
    {
        private const String assemblySource = @"using Narumikazuchi.Generators.TaggedUnions;
using System;

[assembly: UnionOf(typeof(Int32), typeof(Double), typeof(Guid))]";

        [TestClass]
        public sealed class can_be_assigned_by
        {
            [TestMethod]
            public void a_value_of_the_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)42d;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_first_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
value |= (Int32)42;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_second_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
value |= 42d;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void a_value_of_the_third_type_using_binary_or_operator()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
value |= Guid.NewGuid();
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_not_be_assigned_using_binary_or_operator
        {
            [TestMethod]
            public void if_value_has_already_been_set()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
value |= (Int32)42;
if (!value.Is(out Int32? check) ||
    check != 69)
{
    throw new Exception(""Value should be the first that had been set."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
Int32? check = (Int32?)value;
if (!check.HasValue)
{
    throw new Exception(""Value should be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
Double? check = (Double?)value;
if (!check.HasValue)
{
    throw new Exception(""Value should be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_third_type_if_contained_value_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
Guid? check = (Guid?)value;
if (!check.HasValue)
{
    throw new Exception(""Value should be castable to third type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class can_not_be_explicitly_cast
        {
            [TestMethod]
            public void to_first_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
Int32? check = (Int32?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_first_type_if_contained_value_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
Int32? check = (Int32?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
Double? check = (Double?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_contained_value_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
Double? check = (Double?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_third_type_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
Guid? check = (Guid?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_third_type_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
Guid? check = (Guid?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_first_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Int32? check = (Int32?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to first type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_second_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Double? check = (Double?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void to_third_type_if_no_value_is_contained()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Guid? check = (Guid?)value;
if (check.HasValue)
{
    throw new Exception(""Value should not be castable to second type."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_for_equals
        {
            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (!value.Equals(69))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (!value.Equals(69d))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void true_if_the_value_equals_the_contained_value_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.Empty;
if (!value.Equals(Guid.Empty))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.Equals(42))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (value.Equals(42d))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_does_not_equal_the_contained_value_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
if (value.Equals(Guid.Empty))
{
    throw new Exception(""Value should be equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void false_if_the_value_type_does_not_equal_the_contained_value_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.Equals(69d))
{
    throw new Exception(""Value should be not equal to source."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_equals
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Console.WriteLine(value.Equals(42));";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_gethashcode
        {
            [TestMethod]
            public void the_value_of_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.GetHashCode() != 69.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_value_of_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (value.GetHashCode() != 69d.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_value_of_third_type_value_if_contained_value_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.Empty;
if (value.GetHashCode() != Guid.Empty.GetHashCode())
{
    throw new Exception(""Value.GetHashCode() should be equal to source.GetHashCode()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_gethashcode
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Console.WriteLine(value.GetHashCode());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_for_tostring
        {
            [TestMethod]
            public void the_first_type_value_if_contained_value_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.ToString() != 69.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_second_type_value_if_contained_value_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (value.ToString() != 69d.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void the_third_type_value_if_contained_value_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.Empty;
if (value.ToString() != Guid.Empty.ToString())
{
    throw new Exception(""Value.ToString() should be equal to source.ToString()."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_throw_notinitialized_for_tostring
        {
            [TestMethod]
            public void if_value_is_set_to_default()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = default;
Console.WriteLine(value.ToString());";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNotNull(results.RuntimeException, "Should throw 'NotInitialized' exception here.");
                Assert.AreEqual(results.RuntimeException.GetType(), typeof(NotInitialized));
            }
        }

        [TestClass]
        public sealed class will_return_true_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (!value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (!value.Is(out Double? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_third_type_and_out_parameter_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
if (!value.Is(out Guid? _))
{
    throw new Exception(""Value.Is() should not return false."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }

        [TestClass]
        public sealed class will_return_false_for_is
        {
            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.Is(out Double? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_first_type_and_out_parameter_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Int32)69;
if (value.Is(out Guid? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_second_type_and_out_parameter_is_of_third_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = (Double)69d;
if (value.Is(out Guid? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_third_type_and_out_parameter_is_of_first_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
if (value.Is(out Int32? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }

            [TestMethod]
            public void if_contained_value_is_of_third_type_and_out_parameter_is_of_second_type()
            {
                const String methodBodySource = @"UnionOf_Int32_Double_Guid value = Guid.NewGuid();
if (value.Is(out Double? _))
{
    throw new Exception(""Value.Is() should not return true."");
}";

                TestResult results = CompilationTester.TestSource(assemblySource: assemblySource,
                                                                  executionSource: methodBodySource);

                Assert.IsTrue(results.Compiles, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.AreEqual(0, results.Diagnostics.Length, String.Join('\n', results.Diagnostics.Select(diagnostic => diagnostic.GetMessage())));
                Assert.IsNull(results.RuntimeException, results.RuntimeException?.ToString() ?? String.Empty);
            }
        }
    }
}