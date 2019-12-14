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
            base(context)
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

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldMembers())
                .Concat(GetRecordMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetFieldMembers()
        {
            // stack all fields on top of each other
            return union.Fields.Select(i => new FieldElementMember(Context, this, i, 0));
        }

        protected virtual IEnumerable<IntrospectionMember> GetRecordMembers()
        {
            // stack all records on top of each other
            return union.Records.Select(i => new IntrospectionTypeMember(Context, this, new RecordElementType(Context, i)));
        }

    }

}
