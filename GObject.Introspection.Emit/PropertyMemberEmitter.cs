using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    class PropertyMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public PropertyMemberEmitter(Context context) :
            base(context)
        {

        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member)
        {
            return EmitDynamicMember(type, (FieldMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, PropertyMember property)
        {
            yield break;
        }

    }

}
