using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using GObject.Introspection.Xml;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides a <see cref="INamespaceSource"/> implementation that holds static XML contents.
    /// </summary>
    public class NamespaceXmlSource : INamespaceSource
    {

        readonly RepositoryElement repository;

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public NamespaceXmlSource(RepositoryElement repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public NamespaceXmlSource(XDocument girXml) :
            this(RepositoryElement.LoadFrom(girXml).FirstOrDefault())
        {

        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public NamespaceXmlSource(XmlReader xml) :
            this(XDocument.Load(xml))
        {

        }

        /// <summary>
        /// Returns the first namespace from the repository that matches.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NamespaceElement Resolve(string name, string version)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (version is null)
                throw new ArgumentNullException(nameof(version));

            return repository.Namespaces.FirstOrDefault(i => i.Name == name && i.Version == version);
        }

    }

}
