using System;
using System.Collections.Generic;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
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

            readonly AliasElement alias;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="declaringType"></param>
            /// <param name="alias"></param>
            public AliasMember(Context context, Type declaringType, AliasElement alias) :
                base(context, declaringType)
            {
                this.alias = alias ?? throw new ArgumentNullException(nameof(alias));
            }

            public override string Name => "Value";

            protected override ITypeSymbol GetFieldType()
            {
                if (alias.Type != null && alias.Type.Name != "none")
                    return alias.Type.ToSpec(Context).Type;
                else
                    return null;
            }

        }

        readonly AliasElement alias;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="alias"></param>
        public AliasElementType(Context context, AliasElement alias) :
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
        protected override IEnumerable<Member> GetMembers()
        {
            yield return new AliasMember(Context, this, alias);
        }

    }

}
