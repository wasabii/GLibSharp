﻿namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes the kinds of <see cref="Model.Member"/>s.
    /// </summary>
    public enum MemberKind
    {

        /// <summary>
        /// Member is a method.
        /// </summary>
        Method,

        /// <summary>
        /// Member is a field.
        /// </summary>
        Field,

        /// <summary>
        /// Member is a property.
        /// </summary>
        Property,

        /// <summary>
        /// Member is an enum member.
        /// </summary>
        Member,

        /// <summary>
        /// Member is an event.
        /// </summary>
        Event,

        /// <summary>
        /// Member is a nested type.
        /// </summary>
        Type,

    }

}