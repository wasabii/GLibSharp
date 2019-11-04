using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Gir.CodeGen.Symbols;
using Gir.Xml;

namespace Gir.CodeGen
{

    /// <summary>
    /// Provides a <see cref="ISymbolSource"/> implementation that holds static XML contents.
    /// </summary>
    class SymbolXmlSource : ISymbolSource
    {

        readonly ISymbolResolver resolver;
        readonly List<Repository> repositories = new List<Repository>();
        readonly Dictionary<SymbolName, ISymbol> cache = new Dictionary<SymbolName, ISymbol>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public SymbolXmlSource(ISymbolResolver resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(Repository repository)
        {
            repositories.Add(repository);
            return this;
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(XDocument girXml)
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
        public SymbolXmlSource Load(XmlReader girXml)
        {
            return Load(XDocument.Load(girXml));
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(TextReader girXml)
        {
            using var reader = XmlReader.Create(girXml);
            return Load(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXml"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(Stream girXml)
        {
            using var reader = new StreamReader(girXml);
            return Load(reader);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXmlPath"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(FileInfo girXmlPath)
        {
            using var stream = girXmlPath.OpenRead();
            return Load(stream);
        }

        /// <summary>
        /// Adds an input GIR file to the builder.
        /// </summary>
        /// <param name="girXmlPath"></param>
        /// <returns></returns>
        public SymbolXmlSource Load(string girXmlPath)
        {
            return Load(new FileInfo(girXmlPath));
        }

        /// <summary>
        /// Attempts to resolve the symbol with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ISymbol Resolve(SymbolName name)
        {
            if (cache.TryGetValue(name, out var symbol) == false)
                cache[name] = symbol = TryBuildSymbol(name);

            return symbol;
        }

        /// <summary>
        /// Attempts to build the symbol.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISymbol TryBuildSymbol(SymbolName name)
        {
            return
                TryBuildClassSymbol(name) ??
                TryBuildRecordSymbol(name) ??
                TryBuildFunctionSymbol(name) ??
                TryBuildEnumSymbol(name) ??
                TryBuildAliasSymbol(name) ??
                TryBuildBitFieldSymbol(name) ??
                TryBuildCallbackSymbol(name) ??
                TryBuildConstantSymbol(name) ??
                TryBuildInterfaceSymbol(name) ??
                TryBuildUnionSymbol(name);
        }

        /// <summary>
        /// Searches the collection for the given <typeparamref name="TElement"/> by name.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="name"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        TElement FindByName<TElement>(SymbolName name, Func<Namespace, IEnumerable<TElement>> getter)
            where TElement : Element, IHasName
        {
            foreach (var repository in repositories)
                foreach (var @namespace in repository.Namespaces)
                    if (@namespace.Name == name.Namespace)
                        foreach (var element in getter(@namespace))
                            if (element.Name == name.Name)
                                return element;

            return null;
        }

        /// <summary>
        /// Attempts to build a symbol describing a class.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISymbol TryBuildClassSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Classes) is Class klass ? new ClassSymbol(resolver, name.Namespace, klass) : null;
        }

        ISymbol TryBuildRecordSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Records) is Record record ? new RecordSymbol(resolver, name.Namespace, record) : null;
        }

        ISymbol TryBuildFunctionSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Functions) is Function function ? new FunctionSymbol(resolver, name.Namespace, function) : null;
        }

        ISymbol TryBuildEnumSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Enums) is Gir.Xml.Enum @enum ? new EnumSymbol(resolver, name.Namespace, @enum) : null;
        }

        ISymbol TryBuildAliasSymbol(SymbolName name)
        {
            var alias = FindByName(name, ns => ns.Aliases);
            if (alias != null)
                return resolver.ResolveSymbol(SymbolName.IsQualified(alias.Type.Name) ? SymbolName.Parse(alias.Type.Name) : new SymbolName(name.Namespace, alias.Type.Name));

            return null;
        }

        ISymbol TryBuildBitFieldSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.BitFields) is BitField bitfield ? new BitFieldSymbol(resolver, name.Namespace, bitfield) : null;
        }

        ISymbol TryBuildCallbackSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Callbacks) is Callback callback ? new CallbackSymbol(resolver, name.Namespace, callback) : null;
        }

        ISymbol TryBuildConstantSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Constants) is Constant constant ? new ConstantSymbol(resolver, name.Namespace, constant) : null;
        }

        ISymbol TryBuildInterfaceSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Interfaces) is Interface iface ? new InterfaceSymbol(resolver, name.Namespace, iface) : null;
        }

        ISymbol TryBuildUnionSymbol(SymbolName name)
        {
            return FindByName(name, ns => ns.Unions) is Union union ? new UnionSymbol(resolver, name.Namespace, union) : null;
        }

    }

}
