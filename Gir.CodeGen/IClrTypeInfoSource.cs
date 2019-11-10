namespace Gir.CodeGen
{

    public interface IClrTypeInfoSource
    {

        ClrTypeInfo Resolve(GirTypeName type);

    }

}
