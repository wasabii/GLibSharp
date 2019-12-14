using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    class MethodMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public MethodMemberEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, IntrospectionMember member)
        {
            return EmitDynamicMember(type, (MethodMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, MethodMember method)
        {
            yield break;
        }

    }

}
