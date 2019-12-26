using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for nested types.
    /// </summary>
    class TypeMemberBuilder : SyntaxMemberBuilderBase<TypeMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public TypeMemberBuilder(ModuleContext context, TypeMember member) :
            base(context, member)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            return Context.GetBuilder(Member.Type).Build();
        }

    }

}
