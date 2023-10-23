public readonly struct TestResult
{
    public Boolean Compiles
    {
        get;
        init;
    }

    public ImmutableArray<Diagnostic> Diagnostics
    {
        get;
        init;
    }

    public Exception? RuntimeException
    {
        get;
        init;
    }
}