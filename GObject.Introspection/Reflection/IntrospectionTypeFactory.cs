using System;
using System.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Reflection
{

    class IntrospectionTypeFactory
    {

        readonly IntrospectionContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public IntrospectionTypeFactory(IntrospectionContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the record.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public IntrospectionTypeDef CreateType(Element element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            switch (element)
            {
                case Alias e:
                    return CreateType(e);
                case BitField e:
                    return CreateType(e);
                case Enumeration e:
                    return CreateType(e);
                case Class e:
                    return CreateType(e);
                case Record e:
                    return CreateType(e);
                case Callback e:
                    return CreateType(e);
                case Interface e:
                    return CreateType(e);
                case Union e:
                    return CreateType(e);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns the appropriate introspection type for the alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Alias alias)
        {
            if (alias is null)
                throw new ArgumentNullException(nameof(alias));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + alias.Name, alias.Name, alias.CType, () => new AliasElementType(context, alias));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the bitfield.
        /// </summary>
        /// <param name="bitfield"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(BitField bitfield)
        {
            if (bitfield is null)
                throw new ArgumentNullException(nameof(bitfield));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + bitfield.Name, bitfield.Name, bitfield.CType, () => new BitFieldElementType(context, bitfield));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the enumeration.
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Enumeration @enum)
        {
            if (@enum is null)
                throw new ArgumentNullException(nameof(@enum));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + @enum.Name, @enum.Name, @enum.CType, () => new EnumerationElementType(context, @enum));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the class.
        /// </summary>
        /// <param name="klass"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Class klass)
        {
            if (klass is null)
                throw new ArgumentNullException(nameof(klass));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + klass.Name, klass.Name, klass.CType, () => new ClassElementType(context, klass));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Record record)
        {
            if (record is null)
                throw new ArgumentNullException(nameof(record));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + record.Name, record.Name, record.CType, () => CreateRecordType(record));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        IntrospectionType CreateRecordType(Record record)
        {
            // kind might be explicitly specified
            var kind = record.ClrInfo?.Kind ?? ClrObjectKind.Auto;
            if (kind == ClrObjectKind.Class)
                return new RecordElementClassType(context, record);
            if (kind == ClrObjectKind.Value)
                return new RecordElementStructureType(context, record);

            // disguised types can only be pointers to some other data, of which we don't know the format of
            if (record.Disguised == true)
                return new RecordElementClassType(context, record);

            // records with constructors should be handles to the data
            // TODO this is not always true, technically, we could copy the data
            // how do we know???
            if (record.Constructors.Count > 0)
                return new RecordElementClassType(context, record);

            // record might contain ref or unref functions, which indicate that reference counting of instances is
            // handled by the library, and we should not keep a copy
            if (record.Functions.Any(i => i.Name == "ref" || i.Name == "unref" || i.Name == "free" || i.Name == "new"))
                return new RecordElementClassType(context, record);

            return new RecordElementStructureType(context, record);
        }

        /// <summary>
        /// Returns <c>true</c> if the type is blittable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsBlittable(AnyType type)
        {
            return type.ToSpec(context).IsBlittable;
        }

        /// <summary>
        /// Returns <c>true</c> if the field is blittable.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        bool IsBlittable(Field field)
        {
            // the field is of a field type
            if (field.Type != null)
                return IsBlittable(field.Type);

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if the record type is blittable.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        bool IsBlittable(Record record)
        {
            return record.Fields.All(i => IsBlittable(i)) && record.Unions.All(i => IsBlittable(i));
        }

        /// <summary>
        /// Returns <c>true</c> if the union type is blittable.
        /// </summary>
        /// <param name="union"></param>
        /// <returns></returns>
        bool IsBlittable(Union union)
        {
            return union.Fields.All(i => IsBlittable(i)) && union.Records.All(i => IsBlittable(i));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the record.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Callback cb)
        {
            if (cb is null)
                throw new ArgumentNullException(nameof(cb));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + cb.Name, cb.Name, cb.CType, () => new CallbackElementType(context, cb));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the interface.
        /// </summary>
        /// <param name="iface"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Interface iface)
        {
            if (iface is null)
                throw new ArgumentNullException(nameof(iface));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + iface.Name, iface.Name, iface.CType, () => new InterfaceElementType(context, iface));
        }

        /// <summary>
        /// Returns the appropriate introspection type for the union.
        /// </summary>
        /// <param name="union"></param>
        /// <returns></returns>
        IntrospectionTypeDef CreateType(Union union)
        {
            if (union is null)
                throw new ArgumentNullException(nameof(union));

            return new IntrospectionTypeDef(context.CurrentNamespace + "." + union.Name, union.Name, union.CType, () => CreateUnionType(union));
        }

        IntrospectionType CreateUnionType(Union union)
        {
            if (IsBlittable(union) == false)
                return CreateUnionClassType(union);
            else
                return CreateUnionStructureType(union);
        }

        IntrospectionType CreateUnionClassType(Union union)
        {
            return new UnionElementClassType(context, union);
        }

        IntrospectionType CreateUnionStructureType(Union union)
        {
            return new UnionElementStructureType(context, union);
        }

    }

}
