using System;

namespace GObject.Introspection.CodeGen.Model
{

    abstract class PropertyMember : Member
    {

        readonly Lazy<Invokable> getterInvokable;
        readonly Lazy<Invokable> setterInvokable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal PropertyMember(Context context, Type declaringType) :
            base(context, declaringType)
        {
            getterInvokable = new Lazy<Invokable>(GetGetterInvokable);
            setterInvokable = new Lazy<Invokable>(GetSetterInvokable);
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public ITypeSymbol PropertyType => GetterInvokable.ReturnType;

        /// <summary>
        /// Gets the visibility of the getter.
        /// </summary>
        public virtual Visibility GetterVisibility => Visibility.Public;

        /// <summary>
        /// Gets the visibility of the setter.
        /// </summary>
        public virtual Visibility SetterVisibility => Visibility.Public;

        /// <summary>
        /// Gets the invokable that describes the getter method.
        /// </summary>
        public Invokable GetterInvokable => getterInvokable.Value;

        /// <summary>
        /// Gets the invokable that describes the setter method.
        /// </summary>
        public Invokable SetterInvokable => setterInvokable.Value;

        /// <summary>
        /// Gets the invokable for the property getter.
        /// </summary>
        /// <returns></returns>
        public abstract Invokable GetGetterInvokable();

        /// <summary>
        /// Gets the invokable for the property setter.
        /// </summary>
        /// <returns></returns>
        public abstract Invokable GetSetterInvokable();

    }

}
