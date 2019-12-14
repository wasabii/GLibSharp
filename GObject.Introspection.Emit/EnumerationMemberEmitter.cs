using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using GObject.Introspection.Reflection;

namespace GObject.Introspection.Dynamic
{

    class EnumerationMemberEmitter : MemberEmitter
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public EnumerationMemberEmitter(DynamicEmitContext context) :
            base(context)
        {

        }

        public override IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, IntrospectionMember member)
        {
            return EmitDynamicMember(type, (EnumerationMember)member);
        }

        IEnumerable<MemberInfo> EmitDynamicMember(TypeBuilder type, EnumerationMember member)
        {
            var f = type.DefineField(member.Name, typeof(int), FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static);
            f.SetConstant(member.Value);
            yield return f;
        }

    }

}
