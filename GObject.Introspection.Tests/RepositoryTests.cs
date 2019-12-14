using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Xsl;

using GObject.Introspection.Dynamic;
using GObject.Introspection.Library;
using GObject.Introspection.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.Tests
{

    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void Test()
        {
            var z = new XslCompiledTransform();
            z.Load(XDocument.Parse(File.ReadAllText("GLib-2.0.gir.xslt")).CreateReader());

            var t = new XDocument();
            using (var wrt = t.CreateWriter())
                z.Transform(XDocument.Parse(File.ReadAllText("GLib-2.0.gir")).CreateReader(), wrt);

            var l2 = new IntrospectionLibrary(new NamespaceLibrary((NamespaceXmlSource)new NamespaceXmlSource(t)));
            var n2 = l2.ResolveModule("GLib", "2.0");
            var t2 = n2.ResolveType("ByteArray");
            var f2 = (FieldMember)t2.ResolveMember("Data");
            var r2 = f2.FieldType;

            var em = new DynamicModuleEmitter(l2);
            var am = em.Emit(n2, AppDomain.CurrentDomain.GetAssemblies());

            var fn = am.GetName().Name + ".dll";
            if (File.Exists(fn))
                File.Delete(fn);

            am.Save(fn);
        }

        [TestMethod]
        public void CanLoadRepositoryFile()
        {
            var t = File.ReadAllText("GLib-2.0.gir");
            var l = new NamespaceXmlSource(XDocument.Parse(t));
            var n = l.Resolve("GLib", "2.0");
        }

    }

}
