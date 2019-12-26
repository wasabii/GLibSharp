using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Base type for classes that emit interop members.
    /// </summary>
    abstract class MemberEmitter
    {

        readonly Context context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MemberEmitter(Context context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the context of the emission process.
        /// </summary>
        public Context Context => context;

        /// <summary>
        /// Emits the member to the type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public abstract IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member);

    }

}
