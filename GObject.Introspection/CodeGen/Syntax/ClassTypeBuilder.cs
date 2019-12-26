using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassTypeBuilder : SyntaxTypeBuilderBase<ClassType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public ClassTypeBuilder(ModuleContext context, ClassType type) :
            base(context, type)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.ClassDeclaration(
                        GetName(),
                        BuildTypeParameters(),
                        GetAccessibility(),
                        GetModifiers(),
                        BuildBaseType(),
                        BuildInterfaceTypes(),
                        BuildMembers()),
                    BuildAttributes())
                .NormalizeWhitespace();
        }

        IEnumerable<string> BuildTypeParameters()
        {
            yield break;
        }

        SyntaxNode BuildBaseType()
        {
            return Type.BaseType != null ? Syntax.TypeSymbol(Type.BaseType) : null;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes()
        {
            return Type.ImplementedInterfaces.SelectMany(i => BuildInterfaceType(i));
        }

        IEnumerable<SyntaxNode> BuildInterfaceType(Model.ITypeSymbol interfaceType)
        {
            yield return Syntax.TypeSymbol(interfaceType);
        }

    }

}
