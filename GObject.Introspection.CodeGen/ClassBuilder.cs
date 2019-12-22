using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class ClassBuilder : SyntaxNodeBuilderBase<ClassType>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ClassBuilder(SyntaxBuilderContext context) :
            base(context)
        {

        }

        protected override IEnumerable<SyntaxNode> Build(ClassType klass)
        {
            yield return BuildClass(klass);
        }

        SyntaxNode BuildClass(ClassType klass) =>
            Syntax.AddAttributes(
                Syntax.ClassDeclaration(
                    GetName(klass),
                    BuildTypeParameters(klass),
                    GetAccessibility(klass),
                    GetModifiers(klass),
                    BuildBaseType(klass),
                    BuildInterfaceTypes(klass),
                    BuildMembers(klass)),
                BuildAttributes(klass))
            .NormalizeWhitespace();

        string GetName(ClassType klass)
        {
            return klass.Name;
        }

        IEnumerable<string> BuildTypeParameters(ClassType klass)
        {
            yield break;
        }

        Accessibility GetAccessibility(ClassType klass)
        {
            return Accessibility.Public;
        }

        DeclarationModifiers GetModifiers(ClassType klass)
        {
            var modifiers = DeclarationModifiers.Partial;

            if (klass.Modifiers.HasFlag(IntrospectionTypeModifier.Static))
                modifiers |= DeclarationModifiers.Static;

            if (klass.Modifiers.HasFlag(IntrospectionTypeModifier.Abstract))
                modifiers |= DeclarationModifiers.Abstract;

            return modifiers;
        }

        SyntaxNode BuildBaseType(ClassType klass)
        {
            return klass.BaseType != null ? Syntax.DottedName(klass.BaseType.Name) : null;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(ClassType klass)
        {
            return klass.ImplementedInterfaces.SelectMany(i => BuildInterfaceType(klass, i));
        }

        IEnumerable<SyntaxNode> BuildInterfaceType(ClassType klass, TypeSymbol interfaceType)
        {
            yield return Syntax.DottedName(interfaceType.Name);
        }

        IEnumerable<SyntaxNode> BuildMembers(ClassType klass)
        {
            return klass.Members.SelectMany(i => BuildMember(klass, i));
        }

        IEnumerable<SyntaxNode> BuildMember(ClassType klass, IntrospectionMember member)
        {
            return Context.Build(member);
        }

        IEnumerable<SyntaxNode> BuildAttributes(ClassType klass)
        {
            yield break;
        }

    }

}
