using Gir.Model;

using Microsoft.CodeAnalysis;

namespace Gir.CodeGen
{

    /// <summary>
    /// Describes the information about a type.
    /// </summary>
    public class TypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public TypeInfo(TypeName name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public TypeName Name { get; }

        /// <summary>
        /// Repository element that produced the type info.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Full name of the CLR type to which the type is represented by.
        /// </summary>
        public SyntaxNode ClrTypeExpression { get; set; }

        /// <summary>
        /// Full name of the CLR type to which the nullable version of the type is represented by.
        /// </summary>
        public SyntaxNode ClrNullableTypeExpression { get; set; }

        /// <summary>
        /// Expression to use to represent the null value of the nullable version of the type.
        /// </summary>
        public SyntaxNode ClrNullExpression { get; set; }

        /// <summary>
        /// Full name of the CLR marshaler type to use for native calls.
        /// </summary>
        public SyntaxNode ClrMarshalerTypeExpression { get; set; }

    }

}
