using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Generates the code for marshalling class types.
    /// </summary>
    class ClassTypeMarshaler : ITypeMarshaler
    {

        readonly SyntaxGenerator syntax;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        public ClassTypeMarshaler(SyntaxGenerator syntax)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
        }

        public SyntaxNode GetParameter(string name, TypeInfo type)
        {
            return syntax.ParameterDeclaration(name, syntax.DottedName(type.ClrName.ToString()));
        }

        public SyntaxNode GetNativeParameter(string name, TypeInfo type)
        {
            return syntax.ParameterDeclaration(name, syntax.DottedName(type.ClrName.ToString()));
        }

        public SyntaxNode GetMarshalParameterSyntax(string name, string targetName, TypeInfo type)
        {
            return null;
        }

        public SyntaxNode GetMarshalParameterDisposeSyntax(string name, string targetName, TypeInfo type)
        {
            return null;
        }

    }

}
