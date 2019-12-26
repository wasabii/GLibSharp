using System;

namespace GObject.Introspection.CodeGen.Model
{

    abstract class MethodMember : Member
    {

        readonly Lazy<Invokable> invokable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        internal MethodMember(Context context, Type declaringType) :
            base(context, declaringType)
        {
            invokable = new Lazy<Invokable>(GetInvokable);
        }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ITypeSymbol ReturnType => invokable.Value.ReturnArgument.Type;

        /// <summary>
        /// Gets the invokable which describes the method.
        /// </summary>
        public Invokable Invokable => invokable.Value;

        /// <summary>
        /// Generates the invokable which describes the method.
        /// </summary>
        /// <returns></returns>
        protected abstract Invokable GetInvokable();

    }

}
