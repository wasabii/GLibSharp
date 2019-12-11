using Microsoft.CodeAnalysis;

namespace GObject.Introspection.CodeGen
{

    /// <summary>
    /// Builds the syntax for marshalling a type.
    /// </summary>
    public interface ITypeMarshaler
    {

        /// <summary>
        /// Gets the syntax for a parameter of the specified type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetParameter(string name, TypeInfo type);

        /// <summary>
        /// Gets the syntax for the native parameter of the specified type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetNativeParameter(string name, TypeInfo type);

        /// <summary>
        /// Gets the syntax to marshal the incoming managed parameter to the destination native parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetMarshalParameterSyntax(string name, string targetName, TypeInfo type);

        /// <summary>
        /// Gets the syntax to dispose of the results of marshalling the incoming managed parameter to the destination native parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="targetName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        SyntaxNode GetMarshalParameterDisposeSyntax(string name, string targetName, TypeInfo type);

    }

}
