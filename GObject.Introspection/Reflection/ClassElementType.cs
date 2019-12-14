using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class ClassElementType : ClassType
    {

        readonly Class klass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="klass"></param>
        public ClassElementType(IntrospectionContext context, Class klass) :
            base(context)
        {
            this.klass = klass ?? throw new ArgumentNullException(nameof(klass));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => klass.Name;

        public override string Name => klass.Name;

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

        protected virtual IEnumerable<IntrospectionTypeMember> GetRecordMembers()
        {
            return klass.Records.Select(i => new IntrospectionTypeMember(Context, this, new RecordElementType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionTypeMember> GetCallbackMembers()
        {
            return klass.Callbacks.Select(i => new IntrospectionTypeMember(Context, this, new CallbackElementType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionTypeMember> GetUnionMembers()
        {
            return klass.Unions.Select(i => new IntrospectionTypeMember(Context, this, new UnionType(Context, i)));
        }

        protected virtual IEnumerable<FieldMember> GetConstantMembers()
        {
            return klass.Constants.Select(i => new ConstantElementMember(Context, this, i));
        }

        protected virtual IEnumerable<FieldMember> GetFieldMembers()
        {
            return klass.Fields.Select(i => new FieldElementMember(Context, this, i));
        }

        protected virtual IEnumerable<EventMember> GetSignalMembers()
        {
            return klass.Signals.Select(i => new SignalElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetMethodMembers()
        {
            return klass.Methods.Select(i => new MethodElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetVirtualMethodMembers()
        {
            return klass.VirtualMethods.Select(i => new VirtualMethodElementMember(Context, this, i));
        }

    }

}
