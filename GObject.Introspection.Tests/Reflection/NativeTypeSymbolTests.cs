using FluentAssertions;
using GObject.Introspection.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GObject.Introspection.Tests.Reflection
{

    [TestClass]
    public class NativeTypeSymbolTests
    {

        [TestMethod]
        public void CanParseTypeName()
        {
            var s = NativeTypeSymbol.Parse("void", t => LookupType(t));
            s.Should().BeAssignableTo<NativeTypeSymbol>();
            s.Name.Should().Be("void");
        }

        [TestMethod]
        public void CanParsePointerName()
        {
            var s = (NativePointerTypeSymbol)NativeTypeSymbol.Parse("void*", t => LookupType(t));
            s.Should().BeAssignableTo<NativePointerTypeSymbol>();
            s.Name.Should().Be("void*");
            s.Type.Name.Should().Be("void");
        }

        [TestMethod]
        public void CanParseConstTypeName()
        {
            var s = (NativeQualifiedTypeSymbol)NativeTypeSymbol.Parse("const int", t => LookupType(t));
            s.Should().BeAssignableTo<NativeQualifiedTypeSymbol>();
            s.Name.Should().Be("const int");
            s.Type.Name.Should().Be("int");
        }

        [TestMethod]
        public void CanParseVolatileTypeName()
        {
            var s = (NativeQualifiedTypeSymbol)NativeTypeSymbol.Parse("volatile int", t => LookupType(t));
            s.Should().BeAssignableTo<NativeQualifiedTypeSymbol>();
            s.Name.Should().Be("volatile int");
            s.Type.Name.Should().Be("int");
        }

        [TestMethod]
        public void CanParseConstVolatileTypeName()
        {
            var s = (NativeQualifiedTypeSymbol)NativeTypeSymbol.Parse("const volatile int", t => LookupType(t));
            s.Should().BeAssignableTo<NativeQualifiedTypeSymbol>();
            s.Name.Should().Be("const volatile int");
            s.Type.Name.Should().Be("int");
        }

        [TestMethod]
        public void CanParseConstPointerTypeName()
        {
            var s = (NativePointerTypeSymbol)NativeTypeSymbol.Parse("const int*", t => LookupType(t));
            s.Should().BeAssignableTo<NativePointerTypeSymbol>();
            s.Name.Should().Be("const int*");
            var t = (NativeQualifiedTypeSymbol)s.Type;
            t.Qualifier.Should().HaveFlag(NativeTypeSymbolQualifier.Const);
            t.Qualifier.Should().NotHaveFlag(NativeTypeSymbolQualifier.Volatile);
            var z = (TextNativeTypeSymbol)t.Type;
            z.Name.Should().Be("int");
        }

        class TextNativeTypeSymbol : NativeTypeSymbol
        {

            public TextNativeTypeSymbol(string name)
            {
                Name = name;
            }

            public override string Name { get; }

        }

        NativeTypeSymbol LookupType(string text)
        {
            return new TextNativeTypeSymbol(text);
        }

    }

}
