namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Represents a literal value.
    /// </summary>
    class LiteralExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public LiteralExpression(Context context, ITypeSymbol type, object value) :
            base(context, type)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        public LiteralExpression(Context context, object value) :
            this(context, context.ResolveManagedSymbol(value.GetType().FullName), value)
        {

        }

        /// <summary>
        /// Literal value.
        /// </summary>
        public object Value { get; }

    }

}
