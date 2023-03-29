namespace Narumikazuchi.Generators.TaggedUnions.Generators;

internal sealed class __SignatureComparer : IComparer<__ISignature>
{
    public Int32 Compare(__ISignature left,
                         __ISignature right)
    {
        if (left is __MethodSignature &&
            right is __PropertySignature)
        {
            return -1;
        }
        else if (left is __PropertySignature &&
                 right is __MethodSignature)
        {
            return 1;
        }
        else if (left is __MethodSignature leftMethod &&
                 right is __MethodSignature rightMethod)
        {
            return leftMethod.Name.CompareTo(rightMethod.Name);
        }
        else if (left is __PropertySignature leftProperty &&
                 right is __PropertySignature rightProperty)
        {
            return leftProperty.Name.CompareTo(rightProperty.Name);
        }
        else
        {
            throw new InvalidCastException();
        }
    }
}