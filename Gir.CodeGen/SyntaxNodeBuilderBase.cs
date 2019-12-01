using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    abstract partial class SyntaxNodeBuilderBase<TElement> : SyntaxNodeBuilderBase
        where TElement : Element
    {

        protected virtual IEnumerable<SyntaxNode> Build(IContext context, TElement element)
        {
            yield break;
        }

        protected virtual SyntaxNode Adjust(IContext context, TElement element, SyntaxNode initial)
        {
            return initial;
        }

        public sealed override IEnumerable<SyntaxNode> Build(IContext context, Element element)
        {
            return element is TElement s ? Build(context, s) : base.Build(context, element);
        }

        public sealed override SyntaxNode Adjust(IContext context, Element element, SyntaxNode initial)
        {
            return element is TElement s ? Adjust(context, s, initial) : base.Adjust(context, element, initial);
        }

        protected virtual IEnumerable<SyntaxNode> BuildAttributes(IContext context, TElement element)
        {
            return base.BuildAttributes(context, element);
        }

    }

    abstract partial class SyntaxNodeBuilderBase : ISyntaxNodeBuilder
    {

        public virtual IEnumerable<SyntaxNode> Build(IContext context, Element element)
        {
            yield break;
        }

        public virtual SyntaxNode Adjust(IContext context, Element element, SyntaxNode initial)
        {
            return initial;
        }

        IEnumerable<SyntaxNode> ISyntaxNodeBuilder.Build(IContext context, Element element)
        {
            if (element is IHasInfo i)
                if (i.Info.Introspectable == false)
                    return Enumerable.Empty<SyntaxNode>();

            return Build(context, element);
        }

        SyntaxNode ISyntaxNodeBuilder.Adjust(IContext context, Element element, SyntaxNode initial)
        {
            return Adjust(context, element, initial);
        }

        /// <summary>
        /// Builds the attributes for the given element.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildAttributes(IContext context, Element element)
        {
            if (element is IHasInfo info)
                foreach (var i in BuildInfoAttributes(context, info))
                    yield return i;

            if (element is IHasAnnotations a)
                foreach (var i in BuildAnnotationAttributes(context, a))
                    yield return i;
        }

        /// <summary>
        /// Builds the set of attributes for the given info.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildInfoAttributes(IContext context, IHasInfo element)
        {
            return BuildInfoAttributes(context, element.Info);
        }

        /// <summary>
        /// Builds the set of attributes for the given annotation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="annotations"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildInfoAttributes(IContext context, Info info)
        {
            if (info != null)
            {
                yield return context.Syntax.Attribute(typeof(InfoAttribute).FullName, BuildInfoAttributeArguments(context, info));

                if (info.Deprecated == true)
                    yield return context.Syntax.Attribute(typeof(ObsoleteAttribute).FullName);
            }
        }

        /// <summary>
        /// Builds the set of attributes for the given annotation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="annotations"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildInfoAttributeArguments(IContext context, Info info)
        {
            if (info.Introspectable != null)
                yield return context.Syntax.AttributeArgument(nameof(InfoAttribute.Introspectable), context.Syntax.LiteralExpression(info.Introspectable));
            if (info.Stability != null)
                yield return context.Syntax.AttributeArgument(nameof(InfoAttribute.Stability), context.Syntax.LiteralExpression(info.Stability));
        }

        /// <summary>
        /// Builds the set of attributes for the given annotation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildAnnotationAttributes(IContext context, IHasAnnotations element)
        {
            return BuildAnnotationAttributes(context, element.Annotations);
        }

        /// <summary>
        /// Builds the set of attributes for the given annotation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="annotations"></param>
        /// <returns></returns>
        protected virtual IEnumerable<SyntaxNode> BuildAnnotationAttributes(IContext context, IEnumerable<Annotation> annotations)
        {
            foreach (var i in annotations)
                yield return context.Syntax.Attribute(typeof(AnnotationAttribute).FullName,
                    context.Syntax.AttributeArgument(nameof(AnnotationAttribute.Name), context.Syntax.LiteralExpression(i.Name)),
                    context.Syntax.AttributeArgument(nameof(AnnotationAttribute.Value), context.Syntax.LiteralExpression(i.Value)));
        }

        /// <summary>
        /// Applies the documentation to the given member.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected virtual SyntaxNode ApplyMemberDocumentation(IContext context, SyntaxNode member, IHasDocumentation documentable)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (member is null)
                throw new ArgumentNullException(nameof(member));
            if (documentable is null)
                throw new ArgumentNullException(nameof(documentable));

            if (documentable.Documentation != null)
            {
                switch (context.Syntax.IdentifierName("_"))
                {
                    case Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax _:
                        member = ApplyCSharpMemberDocumentation(context, member, documentable.Documentation);
                        break;
                    case Microsoft.CodeAnalysis.VisualBasic.Syntax.IdentifierNameSyntax _:
                        member = ApplyVisualBasicMemberDocumentation(context, member, documentable.Documentation);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            return member;
        }

    }

}
