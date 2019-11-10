namespace Gir.CodeGen
{

    public interface IClrTypeInfoProvider
    {

        ClrTypeInfo Resolve(GirTypeName type);

    }

}
