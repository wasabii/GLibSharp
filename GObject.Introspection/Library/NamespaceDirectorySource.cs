using System;
using System.Collections.Concurrent;
using System.IO;
using System.Xml.Linq;

using GObject.Introspection.Model;

namespace GObject.Introspection.Library
{

    /// <summary>
    /// Provides a <see cref="INamespaceSource"/> implementation that consults a directory of repository files.
    /// </summary>
    public class NamespaceDirectorySource : INamespaceSource
    {

        readonly DirectoryInfo basePath;
        readonly ConcurrentDictionary<(string, string), NamespaceXmlSource> cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="basePath"></param>
        public NamespaceDirectorySource(DirectoryInfo basePath)
        {
            this.basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));

            cache = new ConcurrentDictionary<(string, string), NamespaceXmlSource>();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="basePath"></param>
        public NamespaceDirectorySource(string basePath) :
            this(new DirectoryInfo(basePath))
        {

        }

        public Namespace Resolve(string name, string version)
        {
            return cache.GetOrAdd((name, version), ((string ns, string version) i) => Load(i.ns, i.version))?.Resolve(name, version);
        }

        /// <summary>
        /// Loads the file in the directory that matches the requested namespace and version.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        NamespaceXmlSource Load(string name, string version)
        {
            var file = new FileInfo(Path.Combine(basePath.FullName, $"{name}.{version}.gir"));
            if (file.Exists)
                using (var s = file.OpenRead())
                    return new NamespaceXmlSource(XDocument.Load(s));

            return null;
        }

    }

}
