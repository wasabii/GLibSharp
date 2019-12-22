using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    class EventMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EventMemberEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member)
        {
            return EmitDynamicMember(type, (FieldMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, EventMember property)
        {
            yield break;
        }

    }

}
