using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class UnionElementClassType : ClassType
    {

        readonly UnionElement union;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="union"></param>
        public UnionElementClassType(Context context, UnionElement union) :
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

        protected override IEnumerable<Member> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldMembers())
                .Concat(GetRecordMembers());
        }

        protected virtual IEnumerable<Member> GetFieldMembers()
        {
            // no fields for a class
            yield break;
        }

        protected virtual IEnumerable<Member> GetRecordMembers()
        {
            // stack all records on top of each other
            return union.Records.Select(i => new TypeMember(Context, this, CreateRecordElement(i)));
        }

        Type CreateRecordElement(RecordElement record)
        {
            return Context.CreateType(record).Type;
        }

    }

}
