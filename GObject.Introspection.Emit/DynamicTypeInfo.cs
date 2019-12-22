using System;
using System.Reflection;

using GObject.Introspection.Model;

namespace GObject.Introspection.Emit
{

    /// <summary>
    /// Represents the process of generating a dynamic type.
    /// </summary>
    public class DynamicTypeInfo<TIntrospectionType> :
        DynamicTypeInfo
        where TIntrospectionType : Model.Type
    {

        readonly TIntrospectionType type;
        readonly Func<TIntrospectionType, TypeInfo> finalizer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="built"></param>
        /// <param name="finalizer"></param>
        public DynamicTypeInfo(TIntrospectionType type, System.Type built, Func<TIntrospectionType, TypeInfo> finalizer) :
            base(built)
        {
            this.type = type ?? throw new ArgumentNullException(nameof(type));
            this.finalizer = finalizer ?? throw new ArgumentNullException(nameof(finalizer));
        }

        protected override TypeInfo Finalize() => finalizer(type);

    }

    /// <summary>
    /// Represents the process of generating a dynamic type.
    /// </summary>
    public abstract class DynamicTypeInfo
    {

        /// <summary>
        /// Creates a new <see cref="DynamicTypeInfo"/> instance.
        /// </summary>
        /// <typeparam name="TIntrospectionType"></typeparam>
        /// <param name="type"></param>
        /// <param name="built"></param>
        /// <param name="finalizer"></param>
        /// <returns></returns>
        public static DynamicTypeInfo Create<TIntrospectionType>(TIntrospectionType type, System.Type built, Func<TIntrospectionType, TypeInfo> finalizer)
            where TIntrospectionType : Model.Type
        {
            return new DynamicTypeInfo<TIntrospectionType>(type, built, finalizer);
        }

        readonly System.Type built;
        readonly Lazy<TypeInfo> final;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="built"></param>
        public DynamicTypeInfo(System.Type built)
        {
            this.built = built ?? throw new ArgumentNullException(nameof(built));

            final = new Lazy<TypeInfo>(Finalize);
        }

        /// <summary>
        /// Implement this method to invoke the finalizer.
        /// </summary>
        /// <returns></returns>
        protected abstract TypeInfo Finalize();

        /// <summary>
        /// Gets the built type info, before finalization.
        /// </summary>
        public System.Type BuiltTypeInfo => built;

        /// <summary>
        /// Gets the final type info, after finalization.
        /// </summary>
        public System.Type FinalTypeInfo => final.Value;

    }

}
