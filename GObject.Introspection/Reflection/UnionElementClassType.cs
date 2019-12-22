using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class UnionElementClassType : ClassType
    {

        readonly Union union;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="union"></param>
        public UnionElementClassType(IntrospectionContext context, Union union) :
            base(context)
        {
            this.union = union ?? throw new ArgumentNullException(nameof(union));
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public override string Name => union.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => union.Name;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => union.CType;

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldMembers())
                .Concat(GetRecordMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetFieldMembers()
        {
            // no fields for a class
            yield break;
        }

        protected virtual IEnumerable<IntrospectionMember> GetRecordMembers()
        {
            // stack all records on top of each other
            return union.Records.Select(i => new IntrospectionTypeMember(Context, this, CreateRecordElement(i)));
        }

        IntrospectionType CreateRecordElement(Record record)
        {
            return Context.CreateType(record).Type;
        }

    }

}
