using GObject.Introspection.Model;

using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Describes the information about a type.
    /// </summary>
    public class TypeInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public TypeInfo(QualifiedTypeName name)
        {
            Name = name;
        }

        /// <summary>
        /// Repository element that produced the type info.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Name of the type.
        /// </summary>
        public QualifiedTypeName Name { get; }

        /// <summary>
        /// Gets the managed name of the type.
        /// </summary>
        public ClrTypeName ClrName { get; }

        /// <summary>
        /// Gets whether the type is ultimately a wrapper for a managed handle.
        /// </summary>
        public bool IsHandle { get; }

        /// <summary>
        /// Gets the marshaler used to generate syntax to translate from a managed value to a native value.
        /// </summary>
        public ITypeMarshaler ClrTypeMarshaler { get; set; }

    }

}
