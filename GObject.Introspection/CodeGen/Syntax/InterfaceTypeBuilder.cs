using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class InterfaceTypeBuilder : SyntaxTypeBuilderBase<InterfaceType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public InterfaceTypeBuilder(ModuleContext context, InterfaceType type) :
            base(context, type)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.InterfaceDeclaration(
                        GetName(),
                        BuildTypeParameters(),
                        GetAccessibility(),
                        BuildInterfaceTypes(),
                        BuildMembers()),
                    BuildAttributes());
        }

        IEnumerable<string> BuildTypeParameters()
        {
            yield break;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes()
        {
            return Type.ImplementedInterfaces.Select(i => BuildImplementedInterface(i));
        }

        SyntaxNode BuildImplementedInterface(Model.ITypeSymbol implementedInterface)
        {
            return Syntax.TypeSymbol(implementedInterface);
        }

    }

}
