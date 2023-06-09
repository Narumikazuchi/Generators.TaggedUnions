﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Narumikazuchi.Generators.TaggedUnions.Analyzers;

public partial class TaggedUnionAnalyzer
{
    static public Diagnostic CreateTypeIsNullDiagnostic(AttributeSyntax attribute,
                                                        Int32 index)
    {
        return Diagnostic.Create(descriptor: s_TypeIsNullDescriptor,
                                 location: attribute.ArgumentList!.Arguments[index].GetLocation());
    }

    static public Diagnostic CreateDuplicateTypeDiagnostic(AttributeSyntax attribute,
                                                           Int32 index)
    {
        return Diagnostic.Create(descriptor: s_DuplicateTypeDescriptor,
                                 location: attribute.ArgumentList!.Arguments[index].GetLocation());
    }

    static public Diagnostic CreateTypenameIsNullDiagnostic(AttributeSyntax attribute)
    {
        return Diagnostic.Create(descriptor: s_TypenameIsNullDescriptor,
                                 location: attribute.ArgumentList!.Arguments.Last().GetLocation());
    }

    static public Diagnostic CreateTypenameIsEmptyDiagnostic(AttributeSyntax attribute)
    {
        return Diagnostic.Create(descriptor: s_TypenameIsEmptyDescriptor,
                                 location: attribute.ArgumentList!.Arguments.Last().GetLocation());
    }

    static public Diagnostic CreateTypenameHasWhitespaceDiagnostic(AttributeSyntax attribute)
    {
        return Diagnostic.Create(descriptor: s_TypenameHasWhitespaceDescriptor,
                                 location: attribute.ArgumentList!.Arguments.Last().GetLocation());
    }

    static public Diagnostic CreateTypenameExistsDiagnostic(AttributeSyntax attribute)
    {
        return Diagnostic.Create(descriptor: s_TypenameExistsDescriptor,
                                 location: attribute.ArgumentList!.Arguments.Last().GetLocation());
    }

    static public Diagnostic CreateUnionExistsDiagnostic(AttributeSyntax attribute,
                                                        String typename)
    {
        return Diagnostic.Create(descriptor: s_UnionExistsDescriptor,
                                 location: attribute.GetLocation(),
                                 typename);
    }

    static public Diagnostic CreateInvalidTypenameDiagnostic(AttributeSyntax attribute)
    {
        return Diagnostic.Create(descriptor: s_InvalidTypenameDescriptor,
                                 location: attribute.ArgumentList!.Arguments.Last().GetLocation());
    }

    static private readonly DiagnosticDescriptor s_TypeIsNullDescriptor = new(id: "NCG001",
                                                                              category: "Code Generation",
                                                                              title: "Union of 'null'",
                                                                              messageFormat: "A tagged union of type 'null' is not allowed.",
                                                                              description: "A tagged union of type 'null' is not allowed.",
                                                                              defaultSeverity: DiagnosticSeverity.Error,
                                                                              isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_DuplicateTypeDescriptor = new(id: "NCG002",
                                                                                 category: "Code Generation",
                                                                                 title: "Union of identical types",
                                                                                 messageFormat: "A tagged union of identical types is not allowed.",
                                                                                 description: "A tagged union of type identical types will result in the exact same type.",
                                                                                 defaultSeverity: DiagnosticSeverity.Error,
                                                                                 isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_TypenameIsNullDescriptor = new(id: "NCG003",
                                                                                  category: "Code Generation",
                                                                                  title: "Typename is null",
                                                                                  messageFormat: "A tagged union can not be created with a typename of 'null'.",
                                                                                  description: "A tagged union can not be created with a typename of 'null'.",
                                                                                  defaultSeverity: DiagnosticSeverity.Error,
                                                                                  isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_TypenameIsEmptyDescriptor = new(id: "NCG004",
                                                                                   category: "Code Generation",
                                                                                   title: "Typename is empty",
                                                                                   messageFormat: "A tagged union can not be created with an empty typename.",
                                                                                   description: "A tagged union can not be created with an empty typename.",
                                                                                   defaultSeverity: DiagnosticSeverity.Error,
                                                                                   isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_TypenameHasWhitespaceDescriptor = new(id: "NCG005",
                                                                                         category: "Code Generation",
                                                                                         title: "Typename has spaces",
                                                                                         messageFormat: "The typename of a tagged union can not contain spaces.",
                                                                                         description: "The typename of a tagged union can not contain spaces.",
                                                                                         defaultSeverity: DiagnosticSeverity.Error,
                                                                                         isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_TypenameExistsDescriptor = new(id: "NCG006",
                                                                                  category: "Code Generation",
                                                                                  title: "Typename already exists",
                                                                                  messageFormat: "A tagged union can not be created with this typename since another type with that name already exists.",
                                                                                  description: "A tagged union can not be created with this typename since another type with that name already exists.",
                                                                                  defaultSeverity: DiagnosticSeverity.Error,
                                                                                  isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_UnionExistsDescriptor = new(id: "NCG007",
                                                                               category: "Code Generation",
                                                                               title: "Union already exists",
                                                                               messageFormat: "A tagged union consisting of the specified types already exists as '{0}'.",
                                                                               description: "A tagged union consisting of the specified types already exists.",
                                                                               defaultSeverity: DiagnosticSeverity.Error,
                                                                               isEnabledByDefault: true);
    static private readonly DiagnosticDescriptor s_InvalidTypenameDescriptor = new(id: "NCG008",
                                                                                   category: "Code Generation",
                                                                                   title: "Invalid Typename",
                                                                                   messageFormat: "This typename is not valid in a C# program. See https://www.ecma-international.org/publications-and-standards/standards/ecma-334/ for more details on how to name types in C#.",
                                                                                   description: "This typename is not valid in a C# program. See https://www.ecma-international.org/publications-and-standards/standards/ecma-334/ for more details on how to name types in C#.",
                                                                                   defaultSeverity: DiagnosticSeverity.Error,
                                                                                   isEnabledByDefault: true);
}