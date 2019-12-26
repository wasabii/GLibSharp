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

        protected override IEnumerable<Member> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetRecordMembers())
                .Concat(GetCallbackMembers())
                .Concat(GetUnionMembers())
                .Concat(GetConstantMembers())
                .Concat(GetSignalMembers())
                .Concat(GetVirtualMethodMembers());
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

    }

}
