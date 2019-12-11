using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type that contains the namespace level functions.
    /// </summary>
    class FunctionType : IntrospectionType
    {

        readonly List<Function> functions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="functions"></param>
        public FunctionType(IntrospectionContext context, List<Function> functions) :
            base(context)
        {
            this.functions = functions ?? throw new ArgumentNullException(nameof(functions));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => null;

        public override string Name => "Functions";

        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Class;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            foreach (var function in functions)
                yield return new FunctionMember(Context, function);
        }

    }

}
