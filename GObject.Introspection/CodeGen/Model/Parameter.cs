using System;

using GObject.Introspection.CodeGen.Model.Expressions;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes an argument relating to an invokable.
    /// </summary>
    class Parameter
    {

        readonly Lazy<ParameterExpression> expression;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="modifier"></param>
        public Parameter(Context context, string name, ITypeSymbol type, ParameterModifier modifier = ParameterModifier.Input)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Modifier = modifier;

            expression = new Lazy<ParameterExpression>(CreateParameterExpression);
        }

        /// <summary>
        /// Gets the context that owns this parameter.
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// Gets the name of the variable.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the variable.
        /// </summary>
        public ITypeSymbol Type { get; }

        /// <summary>
        /// Describes the direction of the argument.
        /// </summary>
        public ParameterModifier Modifier { get; }

        /// <summary>
        /// Gets the variable expression associated with this parameter.
        /// </summary>
        public ParameterExpression Expression => expression.Value;

        /// <summary>
        /// Gets the variable expression associated with this parameter.
        /// </summary>
        /// <returns></returns>
        ParameterExpression CreateParameterExpression()
        {
            return new ParameterExpression(Context, this);
        }

    }

}
