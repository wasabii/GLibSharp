using System;
using System.Collections.Generic;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Implements the base class for building syntax nodes from members.
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    abstract partial class SyntaxMemberBuilderBase<TMember> : SyntaxMemberBuilderBase
        where TMember : Member
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        public SyntaxMemberBuilderBase(ModuleContext context, TMember member) :
            base(context, member)
        {

        }

        /// <summary>
        /// Gets the member to be built.
        /// </summary>
        protected new TMember Member => (TMember)base.Member;

    }

    /// <summary>
    /// Implements the base class for building syntax nodes from members.
    /// </summary>
    abstract partial class SyntaxMemberBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public SyntaxMemberBuilderBase(ModuleContext context, Member member)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Gets the generator used to create syntax nodes.
        /// </summary>
        protected ModuleContext Context { get; }

        /// <summary>
        /// Gets the member to be built.
        /// </summary>
        protected Member Member { get; }

        /// <summary>
        /// Gets a reference to the syntax generator.
        /// </summary>
        protected SyntaxGenerator Syntax => Context.Syntax;

        /// <summary>
        /// Builds the syntax for the member.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<SyntaxNode> Build();

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetName()
        {
            return Member.Name;
        }

        /// <summary>
        /// Gets the accessibility of the member.
        /// </summary>
        /// <returns></returns>
        protected virtual Accessibility GetAccessibility()
        {
            return Member.Visibility switch
            {
                Visibility.Public => Accessibility.Public,
                Visibility.Private => Accessibility.Private,
                Visibility.Internal => Accessibility.Internal,
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets the declaration modifiers of the member.
        /// </summary>
        /// <returns></returns>
        protected virtual DeclarationModifiers GetModifiers()
        {
            var modifiers = DeclarationModifiers.None;

            if (Member.Modifiers.HasFlag(MemberModifier.Static))
                modifiers |= DeclarationModifiers.Static;

            if (Member.Modifiers.HasFlag(MemberModifier.Abstract))
                modifiers |= DeclarationModifiers.Abstract;

            if (Member.Modifiers.HasFlag(MemberModifier.Virtual))
                modifiers |= DeclarationModifiers.Virtual;

            if (Member.Modifiers.HasFlag(MemberModifier.Override))
                modifiers |= DeclarationModifiers.Override;

            return modifiers;
        }

    }

}
