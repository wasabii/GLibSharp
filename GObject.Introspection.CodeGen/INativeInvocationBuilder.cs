namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds an invocation of a native function from a set of managed input items.
    /// </summary>
    public interface INativeInvocationBuilder
    {

        NativeInvocation Build(IParameter[] parameters);

    }

}
