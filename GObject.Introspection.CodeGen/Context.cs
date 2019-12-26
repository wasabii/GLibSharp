using System;
using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Passes some contextual information down to each syntax node builder.
    /// </summary>
    public class Context
    {

        readonly SyntaxGenerator syntax;
        readonly SyntaxModuleBuilder builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="builder"></param>
        public Context(SyntaxGenerator syntax, SyntaxModuleBuilder builder)
        {
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            this.builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        public SyntaxGenerator Syntax => syntax;

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(Dynamic.Type type)
        {
            switch (type)
            {
                case ClassType c:
                    return new ClassTypeBuilder(this).Build(c);
                case StructureType c:
                    return new StructureTypeBuilder(this).Build(c);
                case EnumType c:
                    return new EnumTypeBuilder(this).Build(c);
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build(Dynamic.Member member)
        {
            switch (member)
            {
                case FieldMember f:
                    return new FieldMemberBuilder(this).Build(f);
                case MethodMember f:
                    return new MethodMemberBuilder(this).Build(f);
                case PropertyMember f:
                    return new PropertyMemberBuilder(this).Build(f);
                default:
                    throw new NotSupportedException();
            }
        }

    }

}
