using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a type that contains the namespace level functions.
    /// </summary>
    class FunctionClassType : ClassType
    {

        readonly List<Function> functions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="functions"></param>
        public FunctionClassType(IntrospectionContext context, List<Function> functions) :
            base(context)
        {
            this.functions = functions ?? throw new ArgumentNullException(nameof(functions));
        }

        /// <summary>
        /// Gets the managed name of the type.
        /// </summary>
        public override string Name => "Functions";

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => null;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => null;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            foreach (var function in functions)
                yield return new FunctionElementMember(Context, this, function);
        }

    }

}
