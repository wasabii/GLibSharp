using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class UnionType : StructureType
    {

        readonly Union union;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="union"></param>
        public UnionType(IntrospectionContext context, Union union) :
            base(context, union)
        {
            this.union = union ?? throw new ArgumentNullException(nameof(union));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => union.Name;

        /// <summary>
        /// Gets the name of the union.
        /// </summary>
        public override string Name => union.Name;

        /// <summary>
        /// Gets the type of the union.
        /// </summary>
        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Struct;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetRecordMembers());
        }

        protected override IEnumerable<IntrospectionMember> GetFieldMembers()
        {
            // stack all fields on top of each other
            return union.Fields.Select(i => new FieldMember(Context, i, 0));
        }

        protected virtual IEnumerable<IntrospectionMember> GetRecordMembers()
        {
            return union.Records.Select(i => new IntrospectionTypeMember(Context, new RecordType(Context, i)));
        }

    }

}
