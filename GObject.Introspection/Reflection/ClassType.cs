using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ClassType : ObjectType
    {

        readonly Class klass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="klass"></param>
        public ClassType(IntrospectionContext context, Class klass) :
            base(context, klass)
        {
            this.klass = klass ?? throw new ArgumentNullException(nameof(klass));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => klass.Name;

        public override string Name => klass.Name;

        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Class;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetRecordMembers())
                .Concat(GetCallbackMembers())
                .Concat(GetUnionMembers())
                .Concat(GetConstantMembers())
                .Concat(GetSignalMembers())
                .Concat(GetVirtualMethodMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetRecordMembers()
        {
            return klass.Records.Select(i => new IntrospectionTypeMember(Context, new RecordType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionMember> GetCallbackMembers()
        {
            return klass.Callbacks.Select(i => new IntrospectionTypeMember(Context, new CallbackType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionMember> GetUnionMembers()
        {
            return klass.Unions.Select(i => new IntrospectionTypeMember(Context, new UnionType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionMember> GetConstantMembers()
        {
            return klass.Constants.Select(i => new ConstantMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetSignalMembers()
        {
            return klass.Signals.Select(i => new SignalMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetVirtualMethodMembers()
        {
            return klass.VirtualMethods.Select(i => new VirtualMethodMethodMember(Context, i));
        }

    }

}
