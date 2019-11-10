using System.Collections.Generic;
using System.IO;
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
            foreach (var r in Repository.LoadFrom(girXml))
                Load(r);

            return this;
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(XmlReader girXml)
        {
            return Load(XDocument.Load(girXml));
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(TextReader girXml)
        {
            using var reader = XmlReader.Create(girXml);
            return Load(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(Stream girXml)
        {
            using var reader = new StreamReader(girXml);
            return Load(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXmlPath"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(FileInfo girXmlPath)
        {
            using var stream = girXmlPath.OpenRead();
            return Load(stream);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXmlPath"></param>
        /// <returns></returns>
        public RepositoryXmlSource Load(string girXmlPath)
        {
            return Load(new FileInfo(girXmlPath));
        }

        public IEnumerable<Repository> GetRepositories()
        {
            return repositories;
        }

    }

}
