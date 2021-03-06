using System;
using System.Reflection;
using System.Xml.Linq;

using FluentAssertions;

using GObject.Introspection.Library.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.CodeGen.Syntax.Tests
{

    [TestClass]
    public class EnumerationTests : RepositoryBuilderTestsBase
    {

        [TestMethod]
        public void Should_generate_enum()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "enumeration",
                    new XAttribute("name", "TestEnum"),
                    new XElement(Xmlns.Core_1_0_NS + "member",
                        new XAttribute("name", "fieldOne"),
                        new XAttribute("value", "1")),
                    new XElement(Xmlns.Core_1_0_NS + "member",
                        new XAttribute("name", "fieldTwo"),
                        new XAttribute("value", "2")),
                    new XElement(Xmlns.Core_1_0_NS + "member",
                        new XAttribute("name", "fieldThree"),
                        new XAttribute("value", "3"))));

            var t = asm.GetType("Test.TestEnum");
            t.Should().NotBeNull();
            t.IsEnum.Should().BeTrue();
            t.GetCustomAttribute<FlagsAttribute>().Should().BeNull();

            var v1 = Enum.ToObject(t, 1);
            var v2 = Enum.ToObject(t, 2);
            var v3 = Enum.ToObject(t, 3);

            v1.Should().NotBeNull();
            v2.Should().NotBeNull();
            v3.Should().NotBeNull();

            Enum.GetName(t, v1).Should().Be("FieldOne");
            Enum.GetName(t, v2).Should().Be("FieldTwo");
            Enum.GetName(t, v3).Should().Be("FieldThree");
        }

    }

}
