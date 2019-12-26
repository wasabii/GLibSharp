using System;
using System.Collections.Generic;
using System.Linq;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a type that is a delegate.
    /// </summary>
    abstract class DelegateType : Type
    {

        readonly Lazy<IReadOnlyList<Argument>> arguments;
        readonly Lazy<Argument> returnArgument;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        internal DelegateType(Context context) :
            base(context)
        {
            arguments = new Lazy<IReadOnlyList<Argument>>(() => GetArguments().ToList());
            returnArgument = new Lazy<Argument>(GetReturnArgument);
        }

        /// <summary>
        /// Gets the arguments that describe the delegate.
        /// </summary>
        public IReadOnlyList<Argument> Arguments => arguments.Value;

        /// <summary>
        /// Gets the arguments that describe the delegate.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Argument> GetArguments();

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public Argument ReturnArgument => returnArgument.Value;

        /// <summary>
        /// Gets the return type of the delegate.
        /// </summary>
        /// <returns></returns>
        protected abstract Argument GetReturnArgument();

    }

}
