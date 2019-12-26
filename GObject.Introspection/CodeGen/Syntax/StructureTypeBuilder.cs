using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for structure types.
    /// </summary>
    class StructureTypeBuilder : SyntaxTypeBuilderBase<StructureType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public StructureTypeBuilder(ModuleContext context, StructureType type) :
            base(context, type)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return
                Syntax.AddAttributes(
                    Syntax.StructDeclaration(
                        GetName(),
                        BuildTypeParameters(),
                        GetAccessibility(),
                        GetModifiers(),
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
            return Type.ImplementedInterfaces.Select(i => Syntax.TypeSymbol(i));
        }

        protected override IEnumerable<SyntaxNode> BuildAttributes()
        {
            foreach (var node in base.BuildAttributes())
                yield return node;

            yield return BuildStructLayoutAttribute();
        }

        SyntaxNode BuildStructLayoutAttribute()
        {
            return Syntax.Attribute(
                typeof(StructLayoutAttribute).FullName,
                Syntax.AttributeArgument(
                    Syntax.MemberAccessExpression(
                        Syntax.DottedName(typeof(LayoutKind).FullName),
                        Syntax.IdentifierName(nameof(LayoutKind.Sequential)))));
        }

    }

}
