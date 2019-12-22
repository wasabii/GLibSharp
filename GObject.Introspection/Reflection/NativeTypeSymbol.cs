using System;

namespace GObject.Introspection.Reflection
{

    /// <summary>
    /// Describes a reference to an introspected native type.
    /// </summary>
    public abstract class NativeTypeSymbol
    {

        /// <summary>
        /// Parses the specified native type declaration into a <see cref="NativeTypeSymbol"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="resolve"></param>
        /// <returns></returns>
        public static NativeTypeSymbol Parse(string text, Func<string, NativeTypeSymbol> resolve)
        {
            var qualifier = NativeTypeSymbolQualifier.None;
            var pointerCount = 0;
            var typeName = (string)null;

            // TODO really bad C-Type parsing
            foreach (var word in text.Split(' '))
            {
                switch (word)
                {
                    case "const":
                        qualifier |= NativeTypeSymbolQualifier.Const;
                        break;
                    case "volatile":
                        qualifier |= NativeTypeSymbolQualifier.Volatile;
                        break;
                    default:
                        var w = word;
                        while (w.EndsWith("*"))
                        {
                            pointerCount++;
                            w = word.Remove(word.Length - 1);
                        }

                        // remaining chars are the type name
                        typeName = w;
                        break;
                }
            }

            if (typeName == null)
                throw new FormatException($"Unable to parse type name from '{text}'.");

            var type = resolve(typeName);
            if (type == null)
                throw new InvalidOperationException($"Could not resolve native type name '{typeName}'.");

            // apply any modifiers
            if (qualifier != NativeTypeSymbolQualifier.None)
                type = new NativeQualifiedTypeSymbol(type, qualifier);

            // apply succession of pointer modifiers
            while (pointerCount-- > 0)
                type = new NativePointerTypeSymbol(type);

            return type;
        }

        /// <summary>
        /// Gets the native type name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Returns <c>true</c> if the type symbol references an array.
        /// </summary>
        public virtual bool IsArray => false;

        /// <summary>
        /// Gets a string representation of the type symbol.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

    }

}
