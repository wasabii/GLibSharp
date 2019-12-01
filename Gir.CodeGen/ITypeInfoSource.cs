namespace Gir.CodeGen
{

    public interface ITypeInfoSource
    {

        TypeInfo Resolve(TypeName type);

    }

}
