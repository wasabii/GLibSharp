using System.IO;
using System.Linq;
using System.Xml.Linq;

using Gir.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gir.Tests
{

    [TestClass]
    public class RepositoryTests
    {

        [TestMethod]
        public void CanLoadRepositoryFile()
        {
            var t = File.ReadAllText("GLib-2.0.gir");
            var l = Repository.LoadFrom(XDocument.Parse(t)).First();
        }

    }

}
