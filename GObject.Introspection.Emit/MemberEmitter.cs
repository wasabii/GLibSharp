using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    /// <summary>
    /// Base type for classes that emit interop members.
    /// </summary>
    abstract class MemberEmitter
    {

        readonly DynamicEmitContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MemberEmitter(DynamicEmitContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context of the emission process.
        /// </summary>
        public DynamicEmitContext Context => context;

        /// <summary>
        /// Emits the member to the type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public abstract IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, IntrospectionMember member);

    }

}
