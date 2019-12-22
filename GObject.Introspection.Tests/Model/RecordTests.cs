using System.Xml.Linq;

using GObject.Introspection.Xml;
using GObject.Introspection.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.Tests.Model
{

    [TestClass]
    public class RecordTests : IntrospectionLibraryTestsBase
    {

        [TestMethod]
        public void Should_generate_valuetype()
        {
            var asm = ExportModule(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "int"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldTwo"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "int"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldThree"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "int")))));

            var t = (StructureType)asm.ResolveType("TestRecord");
            var f = (FieldMember)t.Members[0];
            var z = f.FieldType;
        }

        [TestMethod]
        public void Should_generate_proper_native_type()
        {
            var asm = ExportModule(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "const int*")))));

            var t = (StructureType)asm.ResolveType("TestRecord");
            var f = (FieldMember)t.Members[0];
            var z = f.FieldType;
        }

        [TestMethod]
        public void Should_generate_proper_native_pointer_to_record()
        {
            var asm = ExportModule(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XAttribute(Xmlns.C_1_0_NS + "type", "GTestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "int")))),
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord2"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "TestRecord"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "GTestRecord*")))));

            var t = (StructureType)asm.ResolveType("TestRecord2");
            var f = (FieldMember)t.Members[0];
            var z = f.FieldType;
        }

    }

}
