using System;
using System.Collections.Generic;
using System.Linq;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen.Builders
{

    abstract class CallableWithSignatureBuilderBase<TElement> : CallableBuilderBase<TElement>
        where TElement : CallableWithSignature
    {

        protected override IEnumerable<SyntaxNode> Build(IContext context, TElement callable)
        {
            var sig = context.Annotation<CallableBuilderOptions>()?.SignatureOnly == true;

            var rlt = Enumerable.Empty<SyntaxNode>();

            // native function required for non-sigonly operation
            if (sig == false)
                rlt = rlt.Concat(BuildNativeFunction(context, callable));

            // build rest of callable requirements
            if (BuildCallable(context, callable) is SyntaxNode c)
                rlt = rlt.Append(ApplyMemberDocumentation(context, c, callable));

            return rlt;
        }

        protected virtual IEnumerable<SyntaxNode> BuildNativeFunction(IContext context, TElement callable)
        {
            return BuilderUtil.BuildNativeFunction(context, callable);
        }

        protected virtual string GetName(IContext context, TElement symbol)
        {
            return symbol.Name;
        }

        protected virtual IEnumerable<SyntaxNode> BuildParameters(IContext context, TElement callable)
        {
            return callable.Parameters.OfType<Parameter>().SelectMany(i => BuildParameter(context, callable, i));
        }

        static IEnumerable<SyntaxNode> BuildParameter(IContext context, TElement callable, Parameter parameter)
        {
            // replace varargs name with args
            var name = parameter.Name;
            if (name == "...")
                name = "args";

            // parameter type might be unknown, and thus object
            var type = parameter.Type;
            if (type != null && type.Name == "none")
                type = null;

            // type node either derived from specified type, or defaulted to object
            var typeNode = type != null ? BuilderUtil.BuildTypeReference(context, type) : context.Syntax.TypeExpression(SpecialType.System_Object);

            // varargs takes specified type and converts to array
            if (parameter.VarArgs)
                typeNode = context.Syntax.ArrayTypeExpression(typeNode);

            // parameter declaration
            var decl = context.Syntax.ParameterDeclaration(
                name,
                typeNode,
                null,
                GetParameterRefKind(parameter));

            // varargs requires 'params' keyword
            if (parameter.VarArgs)
            {
                switch (decl)
                {
                    case Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax cs:
                        decl = cs.AddModifiers(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ParamsKeyword));
                        break;
                    case Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterSyntax vb:
                    default:
                        throw new InvalidOperationException("Unsupported language.");
                }
            }

            yield return decl;
        }

        /// <summary>
        /// Gets the <see cref="RefKind"/> of a paremeter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static RefKind GetParameterRefKind(IParameter parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            return parameter.Direction switch
            {
                ParameterDirection.InOut => RefKind.Ref,
                ParameterDirection.In => RefKind.None,
                ParameterDirection.Out => RefKind.Out,
                null => RefKind.None,
                _ => throw new InvalidOperationException(),
            };
        }
        protected virtual IEnumerable<string> BuildTypeParameters(IContext context, TElement symbol)
        {
            yield break;
        }

        protected virtual SyntaxNode BuildReturnType(IContext context, TElement method)
        {
            var returnTypeElement = method.ReturnValue?.Type;
            if (returnTypeElement == null)
                return null;

            // TODO lookup mapped type
            var returnTypeName = returnTypeElement.Name;
            if (returnTypeName == null || returnTypeName == "none")
                return null;

            var returnType = context.Syntax.IdentifierName(returnTypeName);

            // value is an array of the underlying type
            if (returnTypeElement is ArrayType arrayType)
                returnType = context.Syntax.ArrayTypeExpression(returnType);

            // value allows nulls
            if (method.ReturnValue.Nullable == true)
                returnType = context.Syntax.NullableTypeExpression(returnType);

            return returnType;
        }

        protected virtual Accessibility GetAccessibility(IContext context, TElement symbol)
        {
            return Accessibility.Public;
        }

        protected virtual DeclarationModifiers GetModifiers(IContext context, TElement symbol)
        {
            return DeclarationModifiers.None;
        }

        protected virtual IEnumerable<SyntaxNode> BuildStatements(IContext context, TElement symbol)
        {
            var sigOnly = context.Annotation<CallableBuilderOptions>()?.SignatureOnly == true;
            if (sigOnly)
                yield break;

            var invoke = context.Syntax.InvocationExpression(
                context.Syntax.IdentifierName("__" + symbol.Name),
                GetNativeArguments(context, symbol));

            if (symbol.ReturnValue == null ||
                symbol.ReturnValue.Type == null ||
                symbol.ReturnValue.Type.Name == "none")
                yield return invoke;
            else
                yield return context.Syntax.ReturnStatement(invoke);
        }

        protected virtual IEnumerable<SyntaxNode> GetNativeArguments(IContext context, TElement symbol)
        {
            return symbol.Parameters.Select(i => GetNativeArgument(context, symbol, i));
        }

        protected virtual SyntaxNode GetNativeArgument(IContext context, TElement symbol, IParameter parameter)
        {
            // instance parameter represents the instance itself
            if (parameter is InstanceParameter)
                return context.Syntax.ThisExpression();

            return context.Syntax.Argument(
                null,
                BuilderUtil.GetNativeParameterRefKind(parameter),
                context.Syntax.IdentifierName(parameter.Name));
        }

    }

}
