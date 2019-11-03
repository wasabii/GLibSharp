using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a <see cref="IRepositoryXmlSource"/> implementation that holds static XML contents.
    /// </summary>
    public class StaticRepositorySource : IRepositoryXmlSource
    {

        readonly List<XDocument> repositories = new List<XDocument>();

        /// <summary>
        /// Retrieves the element associated with the namespace at the specified version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public XElement? ResolveNamespace(string name, string version)
        {
            return repositories
                .SelectMany(i => i.Root.Elements(Xmlns.Core_1_0 + "namespace"))
                .Where(i => (string)i.Attribute("name") == name)
                .Where(i => (string)i.Attribute("version") == version)
                .FirstOrDefault();
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(XDocument repository)
        {
            repositories.Add(repository);
            return this;
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(XmlReader repository)
        {
            return Add(XDocument.Load(repository));
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(TextReader input)
        {
            using var reader = XmlReader.Create(input);
            return Add(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(Stream input)
        {
            using var reader = new StreamReader(input);
            return Add(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(FileInfo input)
        {
            using var stream = input.OpenRead();
            return Add(stream);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public StaticRepositorySource Add(string path)
        {
            return Add(new FileInfo(path));
        }

    }

}
