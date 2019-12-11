using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen.Builders
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
            return symbol.Name.ToPascalCase();
        }

        protected virtual IEnumerable<SyntaxNode> BuildParameters(IContext context, TElement callable)
        {
            return callable.Parameters.OfType<Parameter>().SelectMany(i => BuildParameter(context, callable, i));
        }

        IEnumerable<SyntaxNode> BuildParameter(IContext context, TElement callable, Parameter parameter)
        {
            // obtain parameter name
            var name = GetArgumentName(parameter);
            if (name == null)
                throw new GirException("Unable to determine argument name.");

            // parameter type might be unknown, and thus object
            var type = parameter.Type;
            if (type != null && type.Name == "none")
                type = null;

            // type node either derived from specified type, or defaulted to object
            var typeSpec = type != null ? BuilderUtil.GetTypeSpec(context, type) : new TypeSpec();

            // varargs takes specified type and converts to array
            if (parameter.VarArgs)
                typeSpec = typeSpec.AsArrayType();

            // construct type syntax
            var typeNode = typeSpec.GetClrTypeExpression(context.Syntax);
            if (typeNode == null)
                throw new InvalidOperationException("Unable to retrieve type node.");

            // parameter declaration
            var decl = context.Syntax.AddAttributes(
                    context.Syntax.ParameterDeclaration(
                    name,
                    typeNode,
                    null,
                    GetParameterRefKind(parameter)),
                BuildParameterAttributes(context, callable, parameter));

            // varargs requires 'params' keyword
            if (parameter.VarArgs)
            {
                switch (decl)
                {
                    case Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax cs:
                        decl = cs.AddModifiers(Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ParamsKeyword));
                        break;
                    case Microsoft.CodeAnalysis.VisualBasic.Syntax.ParameterSyntax _:
                    default:
                        throw new InvalidOperationException("Unsupported language.");
                }
            }

            yield return decl;
        }

        /// <summary>
        /// Generates the attributes for a parameter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callable"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IEnumerable<SyntaxNode> BuildParameterAttributes(IContext context, TElement callable, Parameter parameter)
        {
            yield return BuildParameterAttribute(context, callable, parameter);
        }

        SyntaxNode BuildParameterAttribute(IContext context, TElement callable, Parameter parameter)
        {
            return context.Syntax.Attribute(
                typeof(ParameterAttribute).FullName,
                BuildParameterAttributeArguments(context, callable, parameter));
        }

        IEnumerable<SyntaxNode> BuildParameterAttributeArguments(IContext context, TElement callable, Parameter parameter)
        {
            yield return context.Syntax.AttributeArgument(context.Syntax.LiteralExpression(parameter.Name));
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

        /// <summary>
        /// Builds the return type syntax node.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected virtual SyntaxNode BuildReturnType(IContext context, TElement method)
        {
            var returnTypeElement = method.ReturnValue?.Type;
            if (returnTypeElement == null)
                return null;

            // void return
            if (returnTypeElement.Name == "none")
                return null;

            // resolve return type
            var typeSpec = BuilderUtil.GetTypeSpec(context, method.ReturnValue.Type);
            if (typeSpec == null)
                throw new InvalidOperationException("Unable to resolve type specification.");

            // value allows nulls
            if (method.ReturnValue.Nullable == true)
                typeSpec = typeSpec.AsNullableType();

            return typeSpec.GetClrTypeExpression(context.Syntax);
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
            return symbol.Parameters.Select(i => BuildNativeArgument(context, symbol, i));
        }

        protected virtual SyntaxNode BuildNativeArgument(IContext context, TElement symbol, IParameter parameter)
        {
            // instance parameter represents the instance itself
            if (parameter is InstanceParameter)
                return context.Syntax.ThisExpression();

            return context.Syntax.Argument(
                null,
                BuilderUtil.GetNativeParameterRefKind(parameter),
                context.Syntax.IdentifierName(GetArgumentName(parameter)));
        }

        string GetNativeArgumentName(IParameter parameter)
        {
            return parameter.Name switch
            {
                "..." => "args",
                _ => parameter.Name,
            };
        }

        string GetArgumentName(IParameter parameter)
        {
            return parameter.Name switch
            {
                "..." => "args",
                _ => parameter.Name.ToCamelCase(),
            };
        }

    }

}
