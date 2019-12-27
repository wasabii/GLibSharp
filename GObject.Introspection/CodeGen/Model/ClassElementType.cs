using System;
using System.Collections.Generic;
using System.Linq;

using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    class ClassElementType : ClassType
    {

        readonly ClassElement klass;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="klass"></param>
        public ClassElementType(Context context, ClassElement klass) :
            base(context)
        {
            this.klass = klass ?? throw new ArgumentNullException(nameof(klass));
        }

        public override string Name => klass.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => klass.Name;

        /// <summary>
        /// Gets the native name of the class.
        /// </summary>
        public override string NativeName => klass.CType;

        /// <summary>
        /// Gets the base type for the class.
        /// </summary>
        /// <returns></returns>
        protected override ITypeSymbol GetBaseType()
        {
            if (klass.Parent != null)
            {
                var baseType = Context.ResolveSymbol(klass.Parent);
                if (baseType == null)
                    throw new InvalidOperationException($"Cannot resolve {klass.Parent} base type for class. Missing reference to namespace?");

                return baseType;
            }

            return null;
        }

        protected override IEnumerable<ITypeSymbol> GetImplementedInterfaces()
        {
            var wrapperType = Context.ResolveManagedSymbol(typeof(GLib.Interop.INativeHandle).FullName);
            if (wrapperType == null)
                throw new InvalidOperationException($"Cannot resolve {typeof(GLib.Interop.INativeHandle).FullName} interface. Ensure a reference to the GLib.Interop assembly is provided.");

            return base.GetImplementedInterfaces()
                .Append(wrapperType);
        }

        protected override IEnumerable<Member> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetRecordMembers())
                .Concat(GetCallbackMembers())
                .Concat(GetUnionMembers())
                .Concat(GetConstantMembers())
                .Concat(GetPropertyMembers())
                .Concat(GetSignalMembers())
                .Concat(GetVirtualMethodMembers())
                .Concat(GetCustomMembers());
        }

        protected virtual IEnumerable<TypeMember> GetRecordMembers()
        {
            return klass.Records.Select(i => new TypeMember(Context, this, Context.CreateType(i).Type));
        }

        protected virtual IEnumerable<TypeMember> GetCallbackMembers()
        {
            return klass.Callbacks.Select(i => new TypeMember(Context, this, Context.CreateType(i).Type));
        }

        protected virtual IEnumerable<TypeMember> GetUnionMembers()
        {
            return klass.Unions.Select(i => new TypeMember(Context, this, Context.CreateType(i).Type));
        }

        protected virtual IEnumerable<FieldMember> GetConstantMembers()
        {
            return klass.Constants.Select(i => new ConstantElementMember(Context, this, i));
        }

        protected virtual IEnumerable<FieldMember> GetFieldMembers()
        {
            return klass.Fields.Select(i => new FieldElementMember(Context, this, i));
        }

        protected virtual IEnumerable<PropertyMember> GetPropertyMembers()
        {
            return klass.Properties.Select(i => new PropertyElementMember(Context, this, i));
        }

        protected virtual IEnumerable<EventMember> GetSignalMembers()
        {
            return klass.Signals.Select(i => new SignalElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetMethodMembers()
        {
            return klass.Methods.Select(i => new MethodElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetVirtualMethodMembers()
        {
            return klass.VirtualMethods.Select(i => new VirtualMethodElementMember(Context, this, i));
        }

        /// <summary>
        /// Emits any custom members.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Member> GetCustomMembers()
        {
            return GetNativeHandleMembers();
        }

        /// <summary>
        /// Gets the members that make up the native handle.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Member> GetNativeHandleMembers()
        {
            var handleField = new HandleFieldMember(Context, this);
            var handleProperty = new HandlePropertyMember(Context, this, handleField);
            var handleConstructor = new HandleConstructorMember(Context, this, handleField);

            yield return handleConstructor;
            yield return handleField;
            yield return handleProperty;
        }

    }

}
