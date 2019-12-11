using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Specifies a CLR type mapped from a GIR type.
    /// </summary>
    public class TypeSpec
    {

        readonly TypeInfo typeInfo;
        readonly bool nullable;
        readonly bool array;

        /// <summary>
        /// Derives a new <see cref="TypeSpec"/> from a <see cref="TypeInfo"/>.
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="nullable"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public TypeSpec(
            TypeInfo typeInfo = null,
            bool nullable = false,
            bool array = false)
        {
            this.typeInfo = typeInfo;
            this.nullable = nullable;
            this.array = array;
        }

        /// <summary>
        /// Gets the referenced type info.
        /// </summary>
        public TypeInfo TypeInfo => typeInfo;

        /// <summary>
        /// Implements the getter for ClrTypeExpression.
        /// </summary>
        /// <returns></returns>
        public SyntaxNode GetClrTypeExpression(SyntaxGenerator syntax)
        {
            if (syntax is null)
                throw new ArgumentNullException(nameof(syntax));

            // missing type info represents object type
            if (typeInfo == null)
            {
                if (array)
                    return syntax.ArrayTypeExpression(syntax.TypeExpression(SpecialType.System_Object));
                else
                    return syntax.TypeExpression(SpecialType.System_Object);
            }

            // array type is based on normal type expression
            if (array)
                return syntax.ArrayTypeExpression(typeInfo.ClrTypeExpression);

            // nullable type fallsback to Nullable<>
            if (nullable)
                if (typeInfo.ClrNullableTypeExpression is SyntaxNode n)
                    return n;
                else
                    syntax.NullableTypeExpression(typeInfo.ClrTypeExpression);

            // standard type specification
            return typeInfo.ClrTypeExpression;
        }

        /// <summary>
        /// Implements the getter for ClrMarshalerTypeExpression.
        /// </summary>
        /// <returns></returns>
        public SyntaxNode GetClrMarshalerTypeExpression(SyntaxGenerator syntax)
        {
            if (syntax is null)
                throw new ArgumentNullException(nameof(syntax));

            // missing type info represents object type
            if (typeInfo == null)
            {
                if (array)
                    return syntax.ArrayTypeExpression(syntax.TypeExpression(SpecialType.System_Object));
                else
                    return syntax.TypeExpression(SpecialType.System_Object);
            }

            // array type is based on normal type expression
            if (array)
                return syntax.GenericName(typeof(ArrayMarshaler<>).FullName, typeInfo.ClrTypeExpression);

            // standard type specification
            return typeInfo.ClrMarshalerTypeExpression;
        }

        /// <summary>
        /// Returns a copy of the type spec referencing a nullable version.
        /// </summary>
        /// <returns></returns>
        public TypeSpec AsNullableType()
        {
            if (nullable)
                return this;
            else
                return new TypeSpec(typeInfo, true, array);
        }

        /// <summary>
        /// Returns a copy of the type spec referencing a nullable version.
        /// </summary>
        /// <returns></returns>
        public TypeSpec AsArrayType()
        {
            if (nullable)
                throw new InvalidOperationException("Cannot return an array type for a nullable type specification.");

            return new TypeSpec(typeInfo, false, true);
        }

    }

}
