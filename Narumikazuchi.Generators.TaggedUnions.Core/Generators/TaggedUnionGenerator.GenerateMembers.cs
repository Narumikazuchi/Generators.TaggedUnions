using Microsoft.CodeAnalysis;
using System.Reflection;

namespace Narumikazuchi.Generators.TaggedUnions.Generators;

public partial class TaggedUnionGenerator
{
    static private String GenerateOperators(String name,
                                            HashSet<ITypeSymbol> types)
    {
        StringBuilder builder = new();
        foreach (ITypeSymbol type in types)
        {
            GenerateOperator(builder: builder,
                             name: name,
                             type: type);
        }

        return builder.ToString();
    }

    static private void GenerateOperator(StringBuilder builder,
                                         String name,
                                         ITypeSymbol type)
    {
        String typeString = type.ToFrameworkString();
        builder.AppendLine($"        static public implicit operator {name}({typeString} value)");
        builder.AppendLine("        {");
        builder.AppendLine($"            return new {name}(value);");
        builder.AppendLine("        }");
        builder.AppendLine();
        builder.AppendLine($"        static public {name} operator |({name} first, {typeString} second)");
        builder.AppendLine("        {");
        builder.AppendLine($"            if (first.m_Tag == Tag.Uninitialized)");
        builder.AppendLine("            {");
        builder.AppendLine($"                return new {name}(second);");
        builder.AppendLine("            }");
        builder.AppendLine("            else");
        builder.AppendLine("            {");
        builder.AppendLine("                return first;");
        builder.AppendLine("            }");
        builder.AppendLine("        }");
        builder.AppendLine();

        if (typeString[typeString.Length - 1] != '?')
        {
            typeString += "?";
        }
        builder.AppendLine($"        static public explicit operator {typeString}({name} source)");
        builder.AppendLine("        {");
        builder.AppendLine($"            if (source.Is(out {typeString} result))");
        builder.AppendLine("            {");
        builder.AppendLine("                return result;");
        builder.AppendLine("            }");
        builder.AppendLine("            else");
        builder.AppendLine("            {");
        builder.AppendLine("                return default;");
        builder.AppendLine("            }");
        builder.AppendLine("        }");
        builder.AppendLine();
    }

    static private String GenerateMethods(__ISignature[] members,
                                          HashSet<ITypeSymbol> types)
    {
        StringBuilder builder = new();

        foreach (__MethodSignature member in members.OfType<__MethodSignature>())
        {
            GenerateMethod(builder: builder,
                           member: member,
                           types: types);
        }

        Int32 index = 1;
        foreach (ITypeSymbol type in types)
        {
            String typeString = type.ToFrameworkString();
            if (typeString[typeString.Length - 1] != '?')
            {
                typeString += "?";
            }

            builder.AppendLine($"        public bool Is([NotNullWhen(true)] out {typeString} result)");
            builder.AppendLine("        {");
            builder.AppendLine($"            if (m_Tag == Tag.Tag_{index})");
            builder.AppendLine("            {");
            builder.AppendLine($"                result = m_{index};");
            builder.AppendLine("                return true;");
            builder.AppendLine("            }");
            builder.AppendLine("            else");
            builder.AppendLine("            {");
            builder.AppendLine("                result = default;");
            builder.AppendLine("                return false;");
            builder.AppendLine("            }");
            builder.AppendLine("        }");
            builder.AppendLine();
            index++;
        }

        return builder.ToString();
    }

    static private void GenerateMethod(StringBuilder builder,
                                       __MethodSignature member,
                                       HashSet<ITypeSymbol> types)
    {
        builder.Append("        public ");
        if (member.IsVirtual)
        {
            builder.Append("override ");
        }

        builder.AppendLine(member.ToSignatureString(true));
        builder.AppendLine("        {");
        builder.AppendLine("            switch (m_Tag)");
        builder.AppendLine("            {");
        for (Int32 index = 1;
             index <= types.Count;
             index++)
        {
            builder.AppendLine($"                case Tag.Tag_{index}:");
            if (member.ReturnsVoid)
            {
                builder.AppendLine($"                    m_{index}.{member.ToCallString()};");
                builder.AppendLine($"                    break;");
            }
            else
            {
                builder.AppendLine($"                    return m_{index}.{member.ToCallString()};");
            }
        }

        builder.AppendLine($"                default:");
        builder.AppendLine($"                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();");

        builder.AppendLine("            };");

        builder.AppendLine("        }");
        builder.AppendLine();
    }

    static private String GenerateProperties(__ISignature[] members,
                                             HashSet<ITypeSymbol> types)
    {
        StringBuilder builder = new();

        foreach (__PropertySignature member in members.OfType<__PropertySignature>())
        {
            GenerateProperty(builder: builder,
                             member: member,
                             types: types);
        }

        builder.AppendLine("        public bool HasValue");
        builder.AppendLine("        {");
        builder.AppendLine("            get");
        builder.AppendLine("            {");
        builder.AppendLine("                return m_Tag != Tag.Uninitialized;");
        builder.AppendLine("            }");
        builder.AppendLine("        }");
        builder.AppendLine();

        return builder.ToString();
    }

    static private void GenerateProperty(StringBuilder builder,
                                         __PropertySignature member,
                                         HashSet<ITypeSymbol> types)
    {
        builder.Append("        public ");
        builder.AppendLine(member.ToSignatureString(true));
        builder.AppendLine("        {");
        if (member.CanRead)
        {
            builder.AppendLine("            get");
            builder.AppendLine("            {");
            builder.AppendLine("                switch (m_Tag)");
            builder.AppendLine("                {");
            for (Int32 index = 1;
                 index <= types.Count;
                 index++)
            {
                builder.AppendLine($"                case Tag.Tag_{index}:");
                builder.AppendLine($"                   return m_{index}{member.ToCallString()};");
            }

            builder.AppendLine($"                default:");
            builder.AppendLine($"                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
        }

        if (member.CanWrite)
        {
            builder.AppendLine("            set");
            builder.AppendLine("            {");
            builder.AppendLine("                switch (m_Tag)");
            builder.AppendLine("                {");
            for (Int32 index = 1;
                 index <= types.Count;
                 index++)
            {
                builder.AppendLine($"                case Tag.Tag_{index}:");
                builder.AppendLine($"                    m_{index}{member.ToCallString()} = value;");
                builder.AppendLine($"                    break;");
            }

            builder.AppendLine($"                default:");
            builder.AppendLine($"                    throw new Narumikazuchi.Generators.TaggedUnions.NotInitialized();");
            builder.AppendLine("                }");
            builder.AppendLine("            }");
        }
        builder.AppendLine("        }");
        builder.AppendLine();
    }

    static private String GenerateInternals(String name,
                                            HashSet<ITypeSymbol> types)
    {
        StringBuilder constructors = new();
        StringBuilder fields = new();
        StringBuilder tags = new();
        tags.AppendLine("        private enum Tag");
        tags.AppendLine("        {");
        tags.AppendLine($"            Uninitialized,");
        Int32 index = 1;
        foreach (ITypeSymbol type in types)
        {
            constructors.AppendLine($"        private {name}({type.ToFrameworkString()} value) : this()");
            constructors.AppendLine("        {");
            constructors.AppendLine($"            m_{index} = value;");
            constructors.AppendLine($"            m_Tag = Tag.Tag_{index};");
            constructors.AppendLine("        }");
            constructors.AppendLine();

            fields.AppendLine($"        private readonly {type.ToFrameworkString()} m_{index};");

            tags.AppendLine($"            Tag_{index},");
            index++;
        }

        fields.Append($"        private readonly Tag m_Tag;");
        tags.AppendLine("        }");
        tags.AppendLine();
        tags.Append(fields.ToString());
        constructors.Append(tags.ToString());
        return constructors.ToString();
    }
}