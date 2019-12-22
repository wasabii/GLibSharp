using System.Reflection;

namespace GObject.Introspection.Model
{

    /// <summary>
    /// Describes a reference to a managed type.
    /// </summary>
    public interface IManagedTypeReference
    {

        /// <summary>
        /// Assembly name of the managed type.
        /// </summary>
        AssemblyName AssemblyName { get; }

        /// <summary>
        /// Name of the managed type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns whether or not the type represents an array.
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// Returns whether or not the type is blittable.
        /// </summary>
        bool IsBlittable { get; }

    }

}