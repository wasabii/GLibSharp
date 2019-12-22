﻿using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for method elements.
    /// </summary>
    class MethodBuilder : SyntaxNodeBuilderBase<MethodMember>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MethodBuilder(SyntaxBuilderContext context) :
            base(context)
        {

        }

        protected override IEnumerable<SyntaxNode> Build(MethodMember method)
        {
            yield return BuildMethod(method);
        }

        SyntaxNode BuildMethod(MethodMember method) =>
            Syntax.AddAttributes(
                Syntax.MethodDeclaration(
                    GetName(method),
                    BuildParameters(method),
                    BuildTypeParameters(method),
                    BuildReturnType(method),
                    GetAccessibility(method),
                    GetModifiers(method),
                    BuildStatements(method)),
                BuildAttributes(method))
            .NormalizeWhitespace();

        string GetName(MethodMember method)
        {
            return method.Name;
        }

        IEnumerable<SyntaxNode> BuildParameters(MethodMember method)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> BuildTypeParameters(MethodMember method)
        {
            throw new NotImplementedException();
        }

        SyntaxNode BuildReturnType(MethodMember method)
        {
            throw new NotImplementedException();
        }

        Accessibility GetAccessibility(MethodMember method)
        {
            throw new NotImplementedException();
        }

        DeclarationModifiers GetModifiers(MethodMember method)
        {
            return DeclarationModifiers.Static;
        }

        IEnumerable<SyntaxNode> BuildStatements(MethodMember method)
        {
            throw new NotImplementedException();
        }

        IEnumerable<SyntaxNode> BuildAttributes(MethodMember method)
        {
            yield break;
        }

    }

}
