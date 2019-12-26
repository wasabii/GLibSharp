using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.CodeGen.Model;

namespace GObject.Introspection.Emit
{

    class EnumMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EnumMemberEmitter(Context context) :
            base(context)
        {

        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, Member member)
        {
            return EmitDynamicMember(type, (EnumMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, EnumMember member)
        {
            var f = type.DefineField(member.Name, typeof(int), FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static);
            f.SetConstant(member.Value);
            yield return f;
        }

    }

}
