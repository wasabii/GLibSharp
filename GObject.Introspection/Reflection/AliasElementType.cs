using System;
using System.Collections.Generic;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a flag type generated from a bitfield.
    /// </summary>
    class AliasElementType : StructureType
    {

        /// <summary>
        /// Defines the single hidden field of an alias.
        /// </summary>
        class AliasMember : FieldMember
        {

            readonly Alias alias;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="declaringType"></param>
            /// <param name="alias"></param>
            public AliasMember(IntrospectionContext context, IntrospectionType declaringType, Alias alias) :
                base(context, declaringType)
            {
                this.alias = alias ?? throw new ArgumentNullException(nameof(alias));
            }

            public override string Name => "Value";

            protected override TypeSpec GetFieldType()
            {
                if (alias.Type != null && alias.Type.Name != "none")
                    return alias.Type.ToSpec(Context);
                else
                    return null;
            }

        }

        readonly Alias alias;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="alias"></param>
        public AliasElementType(IntrospectionContext context, Alias alias) :
            base(context)
        {
            this.alias = alias ?? throw new ArgumentNullException(nameof(alias));
        }

        /// <summary>
        /// Gets the CLR name of the type.
        /// </summary>
        public override string Name => alias.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => alias.Name;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => alias.CType;

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            yield return new AliasMember(Context, this, alias);
        }

    }

}
