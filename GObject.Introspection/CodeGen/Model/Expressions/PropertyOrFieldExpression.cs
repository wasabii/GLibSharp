using System;

namespace GObject.Introspection.CodeGen.Model.Expressions
{

    /// <summary>
    /// Describes an expression that assigns a value to a variable.
    /// </summary>
    class PropertyOrFieldExpression : Expression
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="instance"></param>
        /// <param name="memberName"></param>
        /// <param name="memberType"></param>
        public PropertyOrFieldExpression(Context context, Expression instance, string memberName, ITypeSymbol memberType) :
            base(context, memberType)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            MemberType = memberType ?? throw new ArgumentNullException(nameof(memberType));
        }

        /// <summary>
        /// Gets the object instance to get the member value of.
        /// </summary>
        public Expression Instance { get; }

        /// <summary>
        /// Gets the name of the member who's value will be returned.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the return type of the member.
        /// </summary>
        public ITypeSymbol MemberType { get; }

    }

}
