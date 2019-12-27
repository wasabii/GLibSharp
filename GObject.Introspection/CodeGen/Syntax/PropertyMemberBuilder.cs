using System;
using System.Collections.Generic;
using System.Linq;
using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for property elements.
    /// </summary>
    class PropertyMemberBuilder : SyntaxMemberBuilderBase<PropertyMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="property"></param>
        public PropertyMemberBuilder(ModuleContext context, PropertyMember property) :
            base(context, property)
        {

        }

        public override IEnumerable<SyntaxNode> Build()
        {
            yield return Syntax.AddAttributes(
                    BuildPropertyDeclaration(),
                    BuildAttributes())
                .NormalizeWhitespace();
        }

        /// <summary>
        /// Builds the property declaration.
        /// </summary>
        /// <returns></returns>
        SyntaxNode BuildPropertyDeclaration()
        {
            var decl = Syntax.PropertyDeclaration(
                GetName(),
                BuildType(),
                GetAccessibility(),
                GetModifiers(),
                BuildGetAccessorStatements(),
                BuildSetAccessorStatements());

            switch (decl)
            {
                case Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax cs:
                    var getter = cs.AccessorList.Accessors.FirstOrDefault(i => i.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.GetAccessorDeclaration);
                    if (getter != null)
                        getter = getter.WithModifiers(new SyntaxTokenList(ToCSharpAccessibility(GetGetterAccessibility())));
                    var setter = cs.AccessorList.Accessors.FirstOrDefault(i => i.Kind() == Microsoft.CodeAnalysis.CSharp.SyntaxKind.SetAccessorDeclaration);
                    if (setter != null)
                        setter = setter.WithModifiers(new SyntaxTokenList(ToCSharpAccessibility(GetSetterAccessibility())));
                    decl = cs = cs.WithAccessorList(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.AccessorList(new SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax>(new[] { getter, setter })));
                    break;
                case Microsoft.CodeAnalysis.VisualBasic.Syntax.PropertyBlockSyntax vb:
                    break;
            }
            return decl;
        }

        /// <summary>
        /// Gets the tokens to apply to a CSharp declaration based on <see cref="Accessibility"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<SyntaxToken> ToCSharpAccessibility(Accessibility value)
        {
            switch (value)
            {
                case Accessibility.Public:
                    yield return Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword);
                    break;
                case Accessibility.Private:
                    yield return Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PrivateKeyword);
                    break;
                case Accessibility.Internal:
                    yield return Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.InternalKeyword);
                    break;
                case Accessibility.Protected:
                    yield return Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ProtectedKeyword);
                    break;
            }
        }

        /// <summary>
        /// Gets the accessibility of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual Accessibility GetGetterAccessibility()
        {
            return ToAccessibility(Member.GetterVisibility);
        }

        /// <summary>
        /// Gets the accessibility of the type.
        /// </summary>
        /// <returns></returns>
        protected virtual Accessibility GetSetterAccessibility()
        {
            return ToAccessibility(Member.SetterVisibility);
        }

        /// <summary>
        /// Converts the visibility into an accessibility type.
        /// </summary>
        /// <param name="visibility"></param>
        /// <returns></returns>
        Accessibility ToAccessibility(Visibility visibility)
        {
            return visibility switch
            {
                Visibility.Public => Accessibility.Public,
                Visibility.Private => Accessibility.Private,
                Visibility.Internal => Accessibility.Internal,
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Builds the property type syntax.
        /// </summary>
        /// <returns></returns>
        SyntaxNode BuildType()
        {
            return Syntax.TypeSymbol(Member.PropertyType);
        }

        /// <summary>
        /// Builds the statements within the getter.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildGetAccessorStatements()
        {
            if (Member.GetterInvokable != null)
                foreach (var statement in Member.GetterInvokable.Statements)
                    yield return Context.Build(statement);
        }

        /// <summary>
        /// Builds the statements within the setter.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildSetAccessorStatements()
        {
            if (Member.SetterInvokable != null)
                foreach (var statement in Member.SetterInvokable.Statements)
                    yield return Context.Build(statement);
        }

        protected IEnumerable<SyntaxNode> BuildAttributes()
        {
            yield break;
        }

    }

}
