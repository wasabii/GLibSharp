using System;
using System.Xml.Linq;

using FluentAssertions;

using GObject.Introspection.Library.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.Tests.CodeGen.Syntax
{

    [TestClass]
    public class RecordTests : TestBase
    {

        [TestMethod]
        public void Should_generate_valuetype()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldOne"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "unsigned int"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldTwo"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "unsigned int"))),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "fieldThree"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "unsigned int")))));

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
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int")))));

            var t = asm.GetType("Test.TestRecord");

            var m1 = t.GetMethod("Equals", new[] { typeof(object) });
            m1.DeclaringType.Should().Be(t);
            m1.ReturnType.Should().Be(typeof(bool));
            m1.GetParameters().Should().HaveCount(1);
            m1.GetParameters()[0].ParameterType.Should().Be(typeof(object));
        }

        [TestMethod]
        public void Should_generate_typed_equals()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int")))));

            var t = asm.GetType("Test.TestRecord");
            var m = t.GetMethod("Equals", new[] { t });
            m.DeclaringType.Should().Be(t);
            m.ReturnType.Should().Be(typeof(bool));
            m.GetParameters().Should().HaveCount(1);
            m.GetParameters()[0].ParameterType.Should().Be(t);
        }

        [TestMethod]
        public void Should_generate_gethashcode()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int")))));

            var t = asm.GetType("Test.TestRecord");
            var m = t.GetMethod("GetHashCode");
            m.DeclaringType.Should().Be(t);
            m.ReturnType.Should().Be(typeof(int));
            m.GetParameters().Should().HaveCount(0);
        }

        [TestMethod]
        public void Should_generate_callback_field()
        {
            var asm = ExportNamespace(
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

        [TestMethod]
        public void Should_pass_non_pointer_by_value()
        {
            var asm = ExportNamespace(
                new XElement(Xmlns.Core_1_0_NS + "record",
                    new XAttribute("name", "TestRecord"),
                    new XAttribute(Xmlns.C_1_0_NS + "type", "GTestRecord"),
                    new XElement(Xmlns.Core_1_0_NS + "field",
                        new XAttribute("name", "field"),
                        new XElement(Xmlns.Core_1_0_NS + "type",
                            new XAttribute("name", "int"))),
                    new XElement(Xmlns.Core_1_0_NS + "function",
                        new XAttribute("name", "func"),
                        new XElement(Xmlns.Core_1_0_NS + "parameters",
                            new XElement(Xmlns.Core_1_0_NS + "parameter",
                                new XAttribute("name", "arg"),
                                new XElement(Xmlns.Core_1_0_NS + "type",
                                    new XAttribute("name", "TestRecord"),
                                    new XAttribute(Xmlns.C_1_0_NS + "type", "GTestRecord")))))));

            var t = asm.GetType("Test.TestRecord");
            t.Should().NotBeNull();
            t.IsValueType.Should().BeTrue();
            t.IsClass.Should().BeFalse();

            var f1 = t.GetMethod("Func");
            f1.DeclaringType.Should().Be(t);
            f1.IsPublic.Should().BeTrue();
            f1.GetParameters().Should().HaveCount(1);
            f1.GetParameters()[0].Name.Should().Be("arg");
            f1.GetParameters()[0].ParameterType.Should().Be(t);
        }

    }

}
