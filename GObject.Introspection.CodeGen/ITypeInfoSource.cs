namespace GObject.Introspection.CodeGen
{

    public interface ITypeInfoSource
    {

        TypeInfo Resolve(QualifiedTypeName type);

    }

}
