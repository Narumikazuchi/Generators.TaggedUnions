using Microsoft.CodeAnalysis.Emit;
using Narumikazuchi.Generators.TaggedUnions;
using Narumikazuchi.Generators.TaggedUnions.Generators;

static public class CompilationTester
{
    static CompilationTester()
    {
        s_ParseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
        s_CompilationOptions = new(OutputKind.DynamicallyLinkedLibrary,
                                   optimizationLevel: OptimizationLevel.Release,
                                   platform: Platform.X64);

        List<String> fileLocations = new()
        {
            typeof(Object).Assembly.Location,
            Path.Join(Path.GetDirectoryName(typeof(Object).Assembly.Location), "System.Console.dll"),
            Path.Join(Path.GetDirectoryName(typeof(Object).Assembly.Location), "System.Runtime.dll"),
            typeof(UnionOfAttribute).Assembly.Location
        };

        s_MetadataReferences = fileLocations.Select(location => MetadataReference.CreateFromFile(location))
                                            .ToImmutableArray<MetadataReference>();
    }

    static public TestResult TestSource(String assemblySource,
                                        String executionSource)
    {
        List<SyntaxTree> syntaxTrees = new(2);
        SyntaxTree syntaxTree = SyntaxFactory.ParseSyntaxTree(text: assemblySource,
                                                              options: s_ParseOptions,
                                                              encoding: Encoding.UTF8);
        syntaxTrees.Add(syntaxTree);

        String wrappedSource = $@"using Narumikazuchi.Generated;
using System;
using System.Threading.Tasks;

static public class Runner
{{
    static public Task Execute()
    {{{executionSource}
        return Task.CompletedTask;
    }}
}}";
        syntaxTree = SyntaxFactory.ParseSyntaxTree(text: wrappedSource,
                                                   options: s_ParseOptions,
                                                   encoding: Encoding.UTF8);
        syntaxTrees.Add(syntaxTree);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new TaggedUnionGenerator());
        Compilation compilation = CSharpCompilation.Create($"Test-{Guid.NewGuid()}")
                                                   .WithOptions(s_CompilationOptions)
                                                   .AddReferences(s_MetadataReferences)
                                                   .AddSyntaxTrees(syntaxTrees);
        _ = driver.RunGeneratorsAndUpdateCompilation(compilation, out compilation, out _);

        using MemoryStream stream = new();
        EmitResult result = compilation.Emit(stream);

        if (result.Success)
        {
            try
            {
                CollectibleAssemblyLoadContext loadContext = new();

                stream.Position = 0;
                Assembly assembly = loadContext.LoadFromStream(stream);
                Type type = assembly.GetType("Runner")!;
                MethodInfo method = type.GetMethod("Execute")!;
                method.Invoke(obj: null,
                              parameters: null);

                loadContext.Unload();

                return new()
                {
                    Compiles = true,
                    Diagnostics = result.Diagnostics
                };

            }
            catch (TargetInvocationException ex)
            {
                return new()
                {
                    Compiles = true,
                    Diagnostics = result.Diagnostics,
                    RuntimeException = ex.InnerException
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Compiles = true,
                    Diagnostics = result.Diagnostics,
                    RuntimeException = ex
                };
            }
        }
        else
        {
            return new()
            {
                Compiles = false,
                Diagnostics = result.Diagnostics
            };
        }
    }

    static private readonly CSharpCompilationOptions s_CompilationOptions;
    static private readonly CSharpParseOptions s_ParseOptions;
    static private readonly ImmutableArray<MetadataReference> s_MetadataReferences;
}