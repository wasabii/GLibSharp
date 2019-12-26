using System;
using System.Collections.Generic;
using System.Linq;
using GObject.Introspection.CodeGen.Model.Expressions;
using GObject.Introspection.Library.Model;

namespace GObject.Introspection.CodeGen.Model
{

    /// <summary>
    /// Describes a record that must be reflected as a reference type.
    /// </summary>
    class RecordElementStructureType : StructureType
    {

        readonly RecordElement record;
        readonly Lazy<List<FieldMember>> fields;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="record"></param>
        public RecordElementStructureType(Context context, RecordElement record) :
            base(context)
        {
            this.record = record ?? throw new ArgumentNullException(nameof(record));

            fields = new Lazy<List<FieldMember>>(() => GetFieldMembers().ToList());
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public override string Name => record.Name;

        /// <summary>
        /// Gets the original introspected name of the type.
        /// </summary>
        public override string IntrospectionName => record.Name;

        /// <summary>
        /// Gets the native name of the type.
        /// </summary>
        public override string NativeName => record.CType;

        /// <summary>
        /// Blittability is determined by whether the record is a struct or class implementation.
        /// </summary>
        /// <returns></returns>
        protected override bool GetIsBlittable()
        {
            return true;
        }

        protected override IEnumerable<ITypeSymbol> GetImplementedInterfaces()
        {
            yield return Context.ResolveManagedSymbol("System.IEquatable`1").MakeGenericType(Context.ResolveSymbol(IntrospectionName));
        }

        /// <summary>
        /// Gets the members of the type.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Member> GetMembers()
        {
            return base.GetMembers()
                .Concat(GetFieldCallbackTypes())
                .Concat(GetPropertyMembers())
                .Concat(fields.Value)
                .Concat(GetConstructorMembers())
                .Concat(GetFunctionMembers())
                .Concat(GetMethodMembers())
                .Concat(GetUnionMembers())
                .Append(BuildEqualsMethod())
                .Append(BuildTypedEqualsMethod())
                .Append(BuildGetHashCodeMethod());
        }

        /// <summary>
        /// Gets callback types declared as the type of fields.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<Member> GetFieldCallbackTypes()
        {
            return record.Fields.Where(i => i.Callback != null).Select(i => GetFieldCallbackType(i));
        }

        protected virtual Member GetFieldCallbackType(FieldElement field)
        {
            return new TypeMember(Context, this, new FieldElementMemberCallbackType(Context, this, field));
        }

        protected virtual IEnumerable<PropertyMember> GetPropertyMembers()
        {
            return record.Properties.Select(i => new PropertyElementMember(Context, this, i));
        }

        protected virtual IEnumerable<FieldMember> GetFieldMembers()
        {
            return record.Fields.Select(i => new FieldElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetConstructorMembers()
        {
            return record.Constructors.Select(i => new ConstructorElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetFunctionMembers()
        {
            return record.Functions.Select(i => new FunctionElementMember(Context, this, i));
        }

        protected virtual IEnumerable<MethodMember> GetMethodMembers()
        {
            return record.Methods.Select(i => new MethodElementMember(Context, this, i));
        }

        protected virtual IEnumerable<TypeMember> GetUnionMembers()
        {
            return record.Unions.Select(i => new TypeMember(Context, this, Context.CreateType(i).Type));
        }

        /// <summary>
        /// Builds the standard equals method.
        /// </summary>
        /// <returns></returns>
        MethodMember BuildEqualsMethod()
        {
            var slf = new ParameterExpression(Context, new ThisArgument(Context, this));
            var arg = new[] { new Argument("other", Context.ResolveManagedSymbol(typeof(object).FullName)) };
            var prm = arg.Select(i => new ParameterExpression(Context, i)).ToArray();
            var ret = new Argument("", Context.ResolveManagedSymbol(typeof(bool).FullName));

            var stm =
                new ReturnStatement(Context,
                    new ConditionalExpression(Context,
                        new IsTypeExpression(Context, prm[0], Context.ResolveSymbol(IntrospectionName)),
                        new InvokeExpression(Context, Context.ResolveSymbol(IntrospectionName), nameof(object.Equals), Context.ResolveManagedSymbol(typeof(bool).FullName), slf, new ConvertExpression(Context, Context.ResolveSymbol(IntrospectionName), prm[0])),
                        new LiteralExpression(Context, false)));

            return new InvokableMethodMember(
                Context,
                this,
                nameof(object.Equals),
                new Invokable(arg, ret, stm),
                Visibility.Public,
                MemberModifier.Override);
        }

        /// <summary>
        /// Builds the standard equals method.
        /// </summary>
        /// <returns></returns>
        MethodMember BuildTypedEqualsMethod()
        {
            var slf = new ParameterExpression(Context, new ThisArgument(Context, this));
            var arg = new[] { new Argument("other", Context.ResolveSymbol(IntrospectionName)) };
            var prm = arg.Select(i => new ParameterExpression(Context, i)).ToArray();
            var ret = new Argument("", Context.ResolveManagedSymbol(typeof(bool).FullName));

            var stm =
                new ReturnStatement(Context,
                    BinaryExpression.AndAlso(
                        fields.Value.Select(i =>
                            new BinaryExpression(Context, BinaryExpressionType.Equal,
                                new PropertyOrFieldExpression(Context, slf, i.Name, i.FieldType),
                                new PropertyOrFieldExpression(Context, prm[0], i.Name, i.FieldType)))));

            return new InvokableMethodMember(
                Context,
                this,
                nameof(object.Equals),
                new Invokable(arg, ret, stm),
                Visibility.Public,
                MemberModifier.Default);
        }

        MethodMember BuildGetHashCodeMethod()
        {
            var slf = new ParameterExpression(Context, new ThisArgument(Context, this));
            var ret = new Argument("", Context.ResolveManagedSymbol(typeof(int).FullName));

            // begin with a call to GetType().FullName.GetHashCode()
            var exp = (Expression)new InvokeExpression(
                Context,
                Context.ResolveSymbol(IntrospectionName),
                nameof(object.GetHashCode),
                Context.ResolveManagedSymbol(typeof(int).FullName),
                new PropertyOrFieldExpression(
                    Context,
                    new InvokeExpression(
                        Context,
                        Context.ResolveSymbol(IntrospectionName),
                        nameof(object.GetType),
                        Context.ResolveManagedSymbol(typeof(System.Type).FullName),
                        slf),
                    nameof(System.Type.FullName),
                    Context.ResolveManagedSymbol(typeof(string).FullName)));

            // append GetHashCode calls for each field
            foreach (var i in fields.Value)
                exp = new BinaryExpression(
                    Context, 
                    BinaryExpressionType.ExclusiveOr,
                    exp, 
                    new InvokeExpression(
                        Context,
                        Context.ResolveSymbol(IntrospectionName),
                        nameof(object.GetHashCode),
                        Context.ResolveManagedSymbol(typeof(int).FullName),
                        new PropertyOrFieldExpression(Context, slf, i.Name, i.FieldType)));

            var stm = new ReturnStatement(Context, exp);

            return new InvokableMethodMember(
                Context,
                this,
                nameof(object.GetHashCode),
                new Invokable(new List<Argument>(), ret, stm),
                Visibility.Public,
                MemberModifier.Override);
        }

    }

}
