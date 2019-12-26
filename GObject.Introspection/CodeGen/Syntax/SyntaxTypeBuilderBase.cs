using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Implements the base class for building a syntax node.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    abstract partial class SyntaxTypeBuilderBase<TType> : SyntaxTypeBuilderBase
        where TType : Model.Type
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        public SyntaxTypeBuilderBase(ModuleContext context, TType type) :
            base(context, type)
        {

        }

        /// <summary>
        /// Gets the type to be built.
        /// </summary>
        protected new TType Type => (TType)base.Type;

    }

    /// <summary>
    /// Implements the base class for building a syntax node.
    /// </summary>
    abstract partial class SyntaxTypeBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public SyntaxTypeBuilderBase(ModuleContext context, Model.Type type)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the generator used to create syntax nodes.
        /// </summary>
        protected ModuleContext Context { get; }

        /// <summary>
        /// Gets the type to be built.
        /// </summary>
        protected Model.Type Type { get; }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        protected SyntaxGenerator Syntax => Context.Syntax;

        /// <summary>
        /// Builds the type.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<SyntaxNode> Build();

        /// <summary>
        /// Builds the specified member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> Build(Member member) => Context.GetBuilder(member).Build();

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetName()
        {
            return Type.Name;
        }

        /// <summary>
        /// Gets the accessibility of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual Accessibility GetAccessibility()
        {
            return Type.Visibility switch
            {
                Visibility.Public => Accessibility.Public,
                Visibility.Private => Accessibility.Private,
                Visibility.Internal => Accessibility.Internal,
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets the declaration modifiers of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual DeclarationModifiers GetModifiers()
        {
            var modifiers = DeclarationModifiers.Partial;

            if (Type.Modifiers.HasFlag(Modifier.Static))
                modifiers |= DeclarationModifiers.Static;

            if (Type.Modifiers.HasFlag(Modifier.Abstract))
                modifiers |= DeclarationModifiers.Abstract;

            return modifiers;
        }

        /// <summary>
        /// Builds the members of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildMembers()
        {
            return Type.Members.SelectMany(i => BuildMember(i));
        }

        /// <summary>
        /// Builds the specified member of the type.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildMember(Member member)
        {
            return Build(member);
        }

        /// <summary>
        /// Builds the attributes to apply to the type.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildAttributes()
        {
            return Type.Attributes.SelectMany(i => BuildAttribute(i)).Where(i => i != null);
        }

        /// <summary>
        /// Builds the attribute to apply to the type.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildAttribute(Attribute attribute)
        {
            yield return Syntax.Attribute(attribute.GetType().FullName);
        }

    }

}
