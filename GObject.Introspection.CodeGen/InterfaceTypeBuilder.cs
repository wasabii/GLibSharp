﻿using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Builds the syntax for class elements.
    /// </summary>
    class InterfaceTypeBuilder : SyntaxTypeBuilderBase
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public InterfaceTypeBuilder(Context context) :
            base(context)
        {

        }

        public IEnumerable<SyntaxNode> Build(InterfaceType iface)
        {
            yield return BuildInterface(iface);
        }

        SyntaxNode BuildInterface(InterfaceType iface) =>
            Syntax.AddAttributes(
                Syntax.InterfaceDeclaration(
                    GetName(iface),
                    BuildTypeParameters(iface),
                    GetAccessibility(iface),
                    BuildInterfaceTypes(iface),
                    BuildMembers(iface)),
                BuildAttributes(iface))
            .NormalizeWhitespace();

        string GetName(InterfaceType face)
        {
            return face.Name;
        }

        IEnumerable<string> BuildTypeParameters(InterfaceType iface)
        {
            yield break;
        }

        Accessibility GetAccessibility(InterfaceType iface)
        {
            return Accessibility.Public;
        }

        IEnumerable<SyntaxNode> BuildInterfaceTypes(InterfaceType iface)
        {
            return iface.ImplementedInterfaces.Select(i => BuildImplementedInterface(iface, i));
        }

        SyntaxNode BuildImplementedInterface(InterfaceType iface, TypeSymbol implementedInterface)
        {
            return Syntax.DottedName(implementedInterface.Name);
        }

        IEnumerable<SyntaxNode> BuildMembers(InterfaceType iface)
        {
            return iface.Members.SelectMany(i => Context.Build(i));
        }

        IEnumerable<SyntaxNode> BuildAttributes(InterfaceType iface)
        {
            yield break;
        }

    }

}
