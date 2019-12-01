using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Gir.Model;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a <see cref="IRepositorySource"/> implementation that holds static XML contents.
    /// </summary>
    public class RepositoryXmlSource : IRepositorySource
    {

        readonly List<Repository> repositories = new List<Repository>();

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(Repository repository)
        {
            if (repository is null)
                throw new ArgumentNullException(nameof(repository));

            repositories.Add(repository);
            return this;
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(XDocument girXml)
        {
            if (girXml is null)
                throw new ArgumentNullException(nameof(girXml));

            foreach (var r in Repository.LoadFrom(girXml))
                Load(r);

            return this;
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(XmlReader xml)
        {
            if (xml is null)
                throw new ArgumentNullException(nameof(xml));

            return Load(XDocument.Load(xml));
        }

        public IEnumerable<Repository> GetRepositories()
        {
            return repositories;
        }

    }

}
