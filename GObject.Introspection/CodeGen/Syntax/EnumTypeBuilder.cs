using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class EnumTypeBuilder : SyntaxTypeBuilderBase<EnumType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public EnumTypeBuilder(ModuleContext context, EnumType type) :
            base(context, type)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.EnumDeclaration(
                        GetName(),
                        GetAccessibility(),
                        GetModifiers(),
                        BuildMembers()),
                    BuildAttributes());
        }

    }

}
