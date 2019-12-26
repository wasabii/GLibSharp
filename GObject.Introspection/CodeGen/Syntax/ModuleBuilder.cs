using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.CodeGen.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Syntax
{

    /// <summary>
    /// Provides an entry point to generate .NET code from a set of GIR sources.
    /// </summary>
    class ModuleBuilder
    {

        readonly Module module;
        readonly SyntaxGenerator syntax;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="syntax"></param>
        public ModuleBuilder(Module module, SyntaxGenerator syntax)
        {
            this.module = module ?? throw new ArgumentNullException(nameof(module));
            this.syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
        }

        /// <summary>
        /// Extracts the code of the generated module.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SyntaxNode> Build()
        {
            return BuildModule(new ModuleContext(syntax), module);
        }

        /// <summary>
        /// Builds the specified types within the specified namespace name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildModule(ModuleContext context, Module module)
        {
            yield return context.Syntax.NamespaceDeclaration(module.Name, module.Types.SelectMany(i => BuildType(context, i)));
        }

        /// <summary>
        /// Initiates a build for the given node.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildType(ModuleContext context, Model.Type type)
        {
            return context.GetBuilder(type).Build();
        }

    }

}
