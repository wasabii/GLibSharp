using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes an object type within a namespace.
    /// </summary>
    abstract class StructureType : IntrospectionType
    {

        readonly Structure structure;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="structure"></param>
        public StructureType(IntrospectionContext context, Structure structure) :
            base(context)
        {
            this.structure = structure ?? throw new ArgumentNullException(nameof(structure));
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldMembers())
                .Concat(GetConstructorMembers())
                .Concat(GetFunctionMembers())
                .Concat(GetMethodMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetMethodMembers()
        {
            return structure.Methods.Select(i => new MethodMethodMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetFunctionMembers()
        {
            return structure.Functions.Select(i => new FunctionMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetConstructorMembers()
        {
            return structure.Constructors.Select(i => new ConstructorMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetFieldMembers()
        {
            return structure.Fields.Select(i => new FieldMember(Context, i));
        }

    }

}
