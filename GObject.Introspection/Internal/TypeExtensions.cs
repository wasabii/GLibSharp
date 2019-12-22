using System;
using System.Collections.Concurrent;
using System.Linq;

namespace GObject.Introspection.Internal
{

    /// <summary>
    /// Provides extensions for working with type instances.
    /// </summary>
    public static class TypeExtensions
    {

        static ConcurrentDictionary<Type, bool> blittableCache = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        /// Returns <c>true</c> if the referenced type is blittable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBlittable(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return blittableCache.GetOrAdd(type, GetIsBlittable);
        }

        static bool GetIsBlittable(Type type)
        {
            // classes are inherently non-blittable
            if (type.IsClass)
                return false;

            // arrays of blittable types are blittable
            if (type.IsArray)
                return IsBlittable(type.GetElementType());

            // otherwise base on type
            switch (type)
            {
                case Type t when t == typeof(byte):
                    return true;
                case Type t when t == typeof(sbyte):
                    return true;
                case Type t when t == typeof(short):
                    return true;
                case Type t when t == typeof(ushort):
                    return true;
                case Type t when t == typeof(int):
                    return true;
                case Type t when t == typeof(uint):
                    return true;
                case Type t when t == typeof(long):
                    return true;
                case Type t when t == typeof(ulong):
                    return true;
                case Type t when t == typeof(IntPtr):
                    return true;
                case Type t when t == typeof(UIntPtr):
                    return true;
                case Type t when t == typeof(float):
                    return true;
                case Type t when t == typeof(double):
                    return true;
                case Type t when t == typeof(byte):
                    return true;
                case Type t when t == typeof(byte):
                    return true;
            };

            // handle structs and other value types
            if (type.IsValueType && type.GetFields().All(i => IsBlittable(i.FieldType)))
                return true;

            return false;
        }

    }

}
