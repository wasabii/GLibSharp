using System;
using System.Xml.Linq;

using FluentAssertions;

using GObject.Introspection.Library.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.Tests.CodeGen.Syntax
{

    [TestClass]
    public class ClassTests : TestBase
    {

        [TestMethod]
        public void Should_generate_class()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "class",
                    new XAttribute("name", "TestClass")));

            var t = asm.GetType("Test.TestClass");
            t.Should().NotBeNull();
            t.IsValueType.Should().BeFalse();
            t.IsClass.Should().BeTrue();
            t.GetInterface("GLib.Interop.INativeHandle").Should().NotBeNull();

            var m = t.GetProperty("Handle");
            m.Should().NotBeNull();
            m.PropertyType.Should().Be(typeof(IntPtr));
        }

        [TestMethod]
        public void Should_generate_class_with_primitive_property()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "class",
                    new XAttribute("name", "TestClass"),
                    new XAttribute(Xmlns.C_1_0_NS + "type", "GTestClass"),
                    new XElement(Xmlns.Core_1_0_NS + "property",
                        new XAttribute("name", "PropertyOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"),
                            new XAttribute(Xmlns.C_1_0_NS + "type", "int")))));

            var t = asm.GetType("Test.TestClass");
            t.Should().NotBeNull();
            t.IsValueType.Should().BeFalse();
            t.IsClass.Should().BeTrue();

            var p = t.GetProperty("PropertyOne");
            p.PropertyType.Should().Be(typeof(int));
        }

    }

}
