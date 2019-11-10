using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Gir.Model;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace Gir.CodeGen.Builders
{

    static class BuilderUtil
    {

        /// <summary>
        /// Builds a native wrapper function capable of invoking the specified callable.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callable"></param>
        /// <returns></returns>
        public static IEnumerable<SyntaxNode> BuildNativeFunction(IContext context, CallableWithSignature callable)
        {
            if (callable is null)
                throw new ArgumentNullException(nameof(callable));

            // resolve current namespace
            var ns = context.Repositories.GetRepositories().SelectMany(i => i.Namespaces).FirstOrDefault(i => i.Name == context.CurrentNamespace);
            if (ns == null)
                throw new InvalidOperationException("Unable to resolve default namespace.");

            var decl = context.Syntax.MethodDeclaration(
                "__" + callable.Name,
                accessibility: Accessibility.Private,
                modifiers: DeclarationModifiers.Static | DeclarationModifiers.Abstract,
                returnType: BuildNativeFunctionReturn(context, callable),
                parameters: BuildNativeFunctionParameters(context, callable),
                statements: null);

            switch (decl)
            {
                case Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax cs:
                    decl = cs
                        .WithModifiers(new SyntaxTokenList(cs.Modifiers
                            .Where(i => !i.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.AbstractKeyword))
                            .Concat(new[] { Microsoft.CodeAnalysis.CSharp.SyntaxFactory.Token(Microsoft.CodeAnalysis.CSharp.SyntaxKind.ExternKeyword) })));
                    break;
                case Microsoft.CodeAnalysis.VisualBasic.Syntax.MethodBlockSyntax vb:
                default:
                    throw new InvalidOperationException("Unsupported language.");
            }

            var importName = ns.SharedLibraries.Count > 0 ? ns.SharedLibraries[0] : "unknown";
            var entryPoint = callable.CIdentifier ?? callable.Name;

            // DllImport attribute for the function
            decl = context.Syntax.AddAttributes(decl,
                context.Syntax.Attribute(typeof(DllImportAttribute).FullName,
                    context.Syntax.AttributeArgument(
                        context.Syntax.LiteralExpression(importName)),
                    context.Syntax.AttributeArgument(nameof(DllImportAttribute.EntryPoint),
                        context.Syntax.LiteralExpression(entryPoint)),
                    context.Syntax.AttributeArgument(nameof(DllImportAttribute.CallingConvention),
                        context.Syntax.MemberAccessExpression(context.Syntax.DottedName(typeof(CallingConvention).FullName), nameof(CallingConvention.Cdecl)))));

            // TODO apply if return value is marshalable, need some sort of type mapping
            decl = context.Syntax.AddReturnAttributes(decl,
                BuildNativeFunctionReturnAttributes(context, callable));

            yield return decl;
        }

        /// <summary>
        /// Builds the attributes added to the return value of a native function.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callable"></param>
        /// <returns></returns>
        static IEnumerable<SyntaxNode> BuildNativeFunctionReturnAttributes(IContext context, CallableWithSignature callable)
        {
            yield break;

            //context.Syntax.Attribute(typeof(MarshalAsAttribute).FullName,
            //    context.Syntax.AttributeArgument(nameof(MarshalAsAttribute.MarshalTypeRef),
            //        context.Syntax.TypeOfExpression(
            //            context.Syntax.DottedName(typeof(Gir.Marshaler).FullName))))
        }

        /// <summary>
        /// Builds a syntax node for the return value of the specified <see cref="Callable"/>.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="callable"></param>
        /// <returns></returns>
        static SyntaxNode BuildNativeFunctionReturn(IContext context, Callable callable)
        {
            var returnTypeElement = callable.ReturnValue?.Type;
            if (returnTypeElement == null)
                return null;

            // resolve type
            var returnType = BuildTypeReference(context, returnTypeElement);

            // value allows nulls
            if (callable.ReturnValue.Nullable == true)
                returnType = context.Syntax.NullableTypeExpression(returnType);

            return returnType;
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameters(IContext context, Callable callable)
        {
            return callable.Parameters.SelectMany(i => BuildNativeFunctionParameter(context, callable, i));
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameter(IContext context, Callable callable, IParameter parameter)
        {
            switch (parameter)
            {
                case Parameter p:
                    return BuildNativeFunctionParameter(context, callable, p);
                case InstanceParameter i:
                    return BuildNativeFunctionParameter(context, callable, i);
                default:
                    throw new InvalidOperationException();
            }
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameter(IContext context, Callable callable, Parameter parameter)
        {
            // replace varargs name with args
            var name = parameter.Name;
            if (name == "...")
                name = "args";

            // resolve type name
            var typeName = GetTypeName(context, parameter.Type);
            if (typeName == null)
                throw new GirException("Could not resolve type name for parameter.");

            // build type syntax
            var typeNode = BuildTypeReference(context, parameter.Type);
            if (typeNode == null)
                throw new GirException("Could not build type reference for parameter.");

            // parameter supports nullable types
            if (parameter.Nullable == true)
                typeNode = context.Syntax.NullableTypeExpression(typeNode);

            // varargs takes specified type and converts to array
            if (parameter.VarArgs)
                typeNode = context.Syntax.ArrayTypeExpression(typeNode);

            // parameter declaration
            var decl = context.Syntax.ParameterDeclaration(
                name,
                typeNode,
                null,
                GetNativeParameterRefKind(parameter));

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

            // apply attributes to the parameter
            decl = context.Syntax.AddAttributes(decl,
                BuildNativeFunctionParameterAttributes(context, callable, parameter, (GirTypeName)typeName));

            yield return decl;
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameterAttributes(IContext context, Callable callable, Parameter parameter, GirTypeName typeName)
        {
            if (BuildNativeFunctionParameterMarshalAsAttribute(context, callable, parameter, typeName) is SyntaxNode n)
                yield return n;
        }

        static SyntaxNode BuildNativeFunctionParameterMarshalAsAttribute(IContext context, Callable callable, Parameter parameter, GirTypeName typeName)
        {
            var clrTypeInfo = context.ResolveClrTypeInfo(typeName);
            if (clrTypeInfo == null)
                throw new GirException($"Unable to resolve CLR type name from {typeName}.");

            var unmanagedType = (UnmanagedType?)null;
            var arguments = new List<SyntaxNode>();

            if (clrTypeInfo.ClrMarshalerTypeName != null)
            {
                unmanagedType = UnmanagedType.CustomMarshaler;
                arguments.Add(context.Syntax.AttributeArgument(
                    nameof(MarshalAsAttribute.MarshalTypeRef),
                    context.Syntax.TypeOfExpression(context.Syntax.DottedName(clrTypeInfo.ClrMarshalerTypeName))));
            }

            if (unmanagedType != null)
            {
                // type argument comes first
                arguments.Insert(0,
                    context.Syntax.AttributeArgument(
                        context.Syntax.MemberAccessExpression(
                            context.Syntax.DottedName(typeof(UnmanagedType).FullName),
                            Enum.GetName(typeof(UnmanagedType), unmanagedType))));

                // build attribute
                return context.Syntax.Attribute(typeof(MarshalAsAttribute).FullName, arguments);
            }

            return null;
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameter(IContext context, Callable callable, InstanceParameter parameter)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (callable is null)
                throw new ArgumentNullException(nameof(callable));

            // standard native parameter declaration
            var decl = context.Syntax.ParameterDeclaration(
                parameter.Name,
                BuildTypeReference(context, parameter.Type),
                null,
                GetNativeParameterRefKind(parameter));

            // apply attributes to the parameter
            decl = context.Syntax.AddAttributes(decl,
                BuildNativeFunctionParameterAttributes(context, callable, parameter));

            yield return decl;
        }

        static IEnumerable<SyntaxNode> BuildNativeFunctionParameterAttributes(IContext context, Callable callable, InstanceParameter parameter)
        {
            yield break;

            //context.Syntax.Attribute(typeof(MarshalAsAttribute).FullName,
            //    context.Syntax.AttributeArgument(nameof(MarshalAsAttribute.MarshalTypeRef),
            //        context.Syntax.TypeOfExpression(
            //            context.Syntax.DottedName(typeof(Gir.Marshaler).FullName)))));
        }

        /// <summary>
        /// Gets the <see cref="RefKind"/> of a paremeter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static RefKind GetNativeParameterRefKind(IParameter parameter)
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

        /// <summary>
        /// Builds a reference to a specific GIR-described type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SyntaxNode BuildTypeReference(IContext context, AnyType type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type switch
            {
                Gir.Model.Type t => BuildTypeReference(context, t),
                ArrayType a => BuildTypeReference(context, a),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Builds a syntax node specifying the type of a GIR type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static SyntaxNode BuildTypeReference(IContext context, Gir.Model.Type type)
        {
            // default to object for unknown types
            if (type == null || type.Name == "none")
                return null;

            var typeName = context.ResolveTypeName(type.Name);
            if (typeName == null)
                throw new GirException($"Unable to resolve type name {type.Name}.");

            var clrTypeName = context.ResolveClrTypeName((GirTypeName)typeName);
            if (clrTypeName == null)
                throw new GirException($"Unable to resolve CLR type name from {typeName}.");

            return context.Syntax.DottedName(clrTypeName);
        }

        /// <summary>
        /// Builds a syntax node specifying the type of a GIR array type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static SyntaxNode BuildTypeReference(IContext context, ArrayType type)
        {
            return context.Syntax.ArrayTypeExpression(BuildTypeReference(context, type.Type));
        }

        /// <summary>
        /// Builds a reference to a specific GIR-described type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GirTypeName? GetTypeName(IContext context, AnyType type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type switch
            {
                Gir.Model.Type t => GetTypeName(context, t),
                ArrayType a => GetTypeName(context, a),
                _ => throw new InvalidOperationException()
            };
        }

        /// <summary>
        /// Gets the original underlying type of the given <see cref="Type"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static GirTypeName? GetTypeName(IContext context, Gir.Model.Type type)
        {
            // default to object for unknown types
            if (type == null || type.Name == "none")
                return null;

            return context.ResolveTypeName(type.Name);
        }

        /// <summary>
        /// Gets the original underlying type of the given <see cref="ArrayType"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static GirTypeName? GetTypeName(IContext context, ArrayType type)
        {
            return GetTypeName(context, type.Type);
        }

    }

}
