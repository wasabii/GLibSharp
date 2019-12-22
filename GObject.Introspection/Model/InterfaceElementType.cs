using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Model
{

    class InterfaceElementType : InterfaceType
    {

        readonly InterfaceElement iface;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="iface"></param>
        public InterfaceElementType(Context context, InterfaceElement iface) :
            base(context)
        {
            this.iface = iface ?? throw new ArgumentNullException(nameof(iface));
        }

        /// <summary>
        /// Gets the managed name of the type.
        /// </summary>
        public override string Name => iface.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => iface.Name;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => iface.CType;

        /// <summary>
        /// Gets the implemented interfaces of this interface.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<TypeSymbol> GetImplementedInterfaces()
        {
            return iface.Implements.Select(i => Context.ResolveSymbol(i.Name));
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Member> GetMembers()
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

        protected virtual IEnumerable<Member> GetCallbackMembers()
        {
            return iface.Callbacks.Select(i => new TypeMember(Context, this, new CallbackElementType(Context, i)));
        }

        protected virtual IEnumerable<FieldMember> GetConstantMembers()
        {
            return iface.Constants.Select(i => new ConstantElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetConstructorMembers()
        {
            if (iface.Constructor != null)
                yield return new ConstructorElementMember(Context, this, iface.Constructor);
        }

        protected virtual IEnumerable<FieldMember> GetFieldMembers()
        {
            return iface.Fields.Select(i => new FieldElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetFunctionMembers()
        {
            return iface.Functions.Select(i => new FunctionElementMember(Context, this, i));
        }

        protected virtual IEnumerable<PropertyMember> GetPropertyMembers()
        {
            return iface.Properties.Select(i => new PropertyElementMember(Context, this, i));
        }

        protected virtual IEnumerable<EventMember> GetSignalMembers()
        {
            return iface.Signals.Select(i => new SignalElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetMethodMembers()
        {
            return iface.Methods.Select(i => new MethodElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetVirtualMethodMembers()
        {
            return iface.VirtualMethods.Select(i => new VirtualMethodElementMember(Context, this, i));
        }

    }

}
