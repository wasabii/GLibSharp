using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class RecordType : ObjectType
    {

        readonly Record record;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        public RecordType(IntrospectionContext context, Record record) :
            base(context, record)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => record.Name;

        public override string Name => record.Name;

        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Struct;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetUnionMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetUnionMembers()
        {
            return record.Unions.Select(i => new IntrospectionTypeMember(Context, new UnionType(Context, i)));
        }

    }

}
