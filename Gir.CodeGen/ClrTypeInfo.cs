using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Describes a CLR type produced from a GIR object.
    /// </summary>
    public class ClrTypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="clrTypeName"></param>
        public ClrTypeInfo(GirTypeName name, ClrTypeName clrTypeName)
        {
            Name = name;
            ClrTypeName = clrTypeName;
        }

        /// <summary>
        /// Name of the underlying GIR type.
        /// </summary>
        public GirTypeName Name { get; }

        /// <summary>
        /// Original element that produced the type symbol.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Full name of the CLR type being referenced.
        /// </summary>
        public ClrTypeName ClrTypeName { get; }

        /// <summary>
        /// Full name of the CLR marshaler type to use for native calls.
        /// </summary>
        public ClrTypeName? ClrMarshalerTypeName { get; set; }

    }

}
