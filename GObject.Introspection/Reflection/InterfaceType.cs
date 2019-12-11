using System;
using System.Collections.Generic;
using System.Linq;
using GObject.Introspection.Internal;
using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class InterfaceType : IntrospectionType
    {

        readonly Interface iface;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="iface"></param>
        public InterfaceType(IntrospectionContext context, Interface iface) :
            base(context)
        {
            this.iface = iface ?? throw new ArgumentNullException(nameof(iface));
        }

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => iface.Name;

        public override string Name => iface.Name;

        public override IntrospectionTypeKind Kind => IntrospectionTypeKind.Interface;

        /// <summary>
        /// Gets the base types of the interface.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<TypeSymbol> GetBaseTypes()
        {
            return base.GetBaseTypes();
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IntrospectionMember> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetCallbackMembers())
                .Concat(GetConstantMembers())
                .Concat(GetConstructorMembers())
                .Concat(GetFieldMembers())
                .Concat(GetFunctionMembers())
                .Concat(GetMethodMembers())
                .Concat(GetPropertyMembers())
                .Concat(GetSignalMembers())
                .Concat(GetVirtualMethodMembers());
        }

        protected virtual IEnumerable<IntrospectionMember> GetCallbackMembers()
        {
            return iface.Callbacks.Select(i => new IntrospectionTypeMember(Context, new CallbackType(Context, i)));
        }

        protected virtual IEnumerable<IntrospectionMember> GetConstantMembers()
        {
            return iface.Constants.Select(i => new ConstantMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetConstructorMembers()
        {
            if (iface.Constructor != null)
                yield return new ConstructorMember(Context, iface.Constructor);
        }

        protected virtual IEnumerable<IntrospectionMember> GetFieldMembers()
        {
            return iface.Fields.Select(i => new FieldMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetFunctionMembers()
        {
            return iface.Functions.Select(i => new FunctionMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetMethodMembers()
        {
            return iface.Methods.Select(i => new MethodMethodMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetPropertyMembers()
        {
            return iface.Properties.Select(i => new PropertyMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetSignalMembers()
        {
            return iface.Signals.Select(i => new SignalMember(Context, i));
        }

        protected virtual IEnumerable<IntrospectionMember> GetVirtualMethodMembers()
        {
            return iface.VirtualMethods.Select(i => new VirtualMethodMethodMember(Context, i));
        }

    }

}
