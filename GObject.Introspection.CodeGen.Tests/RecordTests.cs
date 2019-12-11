using System;
using System.Xml.Linq;

using FluentAssertions;

using GObject.Introspection.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.CodeGen.Tests
{

    [TestClass]
    public class RecordTests : RepositoryBuilderTestsBase
    {

        [TestMethod]
        public void Should_generate_valuetype()
        {
            var asm = Build(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldTwo"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldThree"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint")))));

            var t = asm.GetType("Test.TestRecord");
            t.Should().NotBeNull();
            t.IsValueType.Should().BeTrue();
            t.IsClass.Should().BeFalse();

            var f1 = t.GetField("FieldOne");
            f1.DeclaringType.Should().Be(t);
            f1.IsPublic.Should().BeTrue();
            f1.FieldType.Should().Be(typeof(uint));

            var f2 = t.GetField("FieldTwo");
            f2.DeclaringType.Should().Be(t);
            f2.IsPublic.Should().BeTrue();
            f2.FieldType.Should().Be(typeof(uint));

            var f3 = t.GetField("FieldThree");
            f3.DeclaringType.Should().Be(t);
            f3.IsPublic.Should().BeTrue();
            f3.FieldType.Should().Be(typeof(uint));

            typeof(IEquatable<>).MakeGenericType(t).IsAssignableFrom(t).Should().BeTrue();
        }

        [TestMethod]
        public void Should_generate_equals()
        {
            var asm = Build(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint")))));

            var t = asm.GetType("Test.TestRecord");
            var m = t.GetMethod("Equals", new[] { typeof(object) });
            m.DeclaringType.Should().Be(t);
            m.ReturnType.Should().Be(typeof(bool));
            m.GetParameters().Should().HaveCount(1);
        }

        [TestMethod]
        public void Should_generate_typed_equals()
        {
            var asm = Build(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint")))));

            var t = asm.GetType("Test.TestRecord");
            var m = t.GetMethod("Equals", new[] { t });
            m.DeclaringType.Should().Be(t);
            m.ReturnType.Should().Be(typeof(bool));
            m.GetParameters().Should().HaveCount(1);
        }

        [TestMethod]
        public void Should_generate_gethashcode()
        {
            var asm = Build(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "guint")))));

            var t = asm.GetType("Test.TestRecord");
            var m = t.GetMethod("GetHashCode");
            m.DeclaringType.Should().Be(t);
            m.ReturnType.Should().Be(typeof(int));
            m.GetParameters().Should().HaveCount(0);
        }

        [TestMethod]
        public void Should_generate_callback_field()
        {
            var asm = Build(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "callback",
                            new XAttribute("name", "field_cb")))));

            var t = asm.GetType("Test.TestRecord");
            t.Should().NotBeNull();
            t.IsValueType.Should().BeTrue();
            t.IsClass.Should().BeFalse();

            var f1 = t.GetField("FieldOne");
            f1.DeclaringType.Should().Be(t);
            f1.IsPublic.Should().BeTrue();
        }

    }

}
