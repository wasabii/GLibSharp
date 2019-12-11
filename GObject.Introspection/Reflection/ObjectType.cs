using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an object type within a namespace.
    /// </summary>
    abstract class ObjectType : StructureType
    {

        readonly GObject.Introspection.Model.Object @object;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="object"></param>
        public ObjectType(IntrospectionContext context, GObject.Introspection.Model.Object @object) :
            base(context, @object)
        {
            this.@object = @object ?? throw new ArgumentNullException(nameof(@object));
        }

        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetPropertyMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetPropertyMembers()
        {
            return @object.Properties.Select(i => new PropertyMember(Context, i));
        }

    }

}
