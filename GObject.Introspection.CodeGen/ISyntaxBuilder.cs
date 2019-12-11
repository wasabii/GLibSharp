namespace GObject.Introspection.CodeGen
{

    public interface ISyntaxBuilder
    {

        /// <summary>
        /// Adds an import to the builder.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        ISyntaxBuilder AddSource(IRepositorySource source);

        /// <summary>
        /// Adds a namespace to be exported.
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        ISyntaxBuilder AddExport(string ns);

        /// <summary>
        /// Initiates a build of the configured namespaces.
        /// </summary>
        /// <returns></returns>
        SyntaxBuilderResult Export();

    }

}
