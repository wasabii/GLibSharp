using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for delegate types.
    /// </summary>
    class DelegateTypeBuilder : SyntaxTypeBuilderBase<DelegateType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public DelegateTypeBuilder(ModuleContext context, DelegateType type) :
            base(context, type)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return Syntax.AddAttributes(
                Syntax.DelegateDeclaration(
                    GetName(),
                    BuildParameters(),
                    GetTypeParameters(),
                    BuildReturnType(),
                    GetAccessibility(),
                    GetModifiers()),
                BuildAttributes())
            .NormalizeWhitespace();
        }

        IEnumerable<SyntaxNode> BuildParameters()
        {
            yield break;
        }

        IEnumerable<string> GetTypeParameters()
        {
            yield break;
        }

        SyntaxNode BuildReturnType()
        {
            return Type.ReturnType != null ? Syntax.TypeSymbol(Type.ReturnType) : null;
        }

    }

}
