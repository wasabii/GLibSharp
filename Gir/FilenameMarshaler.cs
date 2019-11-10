using System;
using System.Runtime.InteropServices;

namespace Gir
{

    public class FilenameMarshaler : ICustomMarshaler
    {

        static FilenameMarshaler marshaler;

        /// <summary>
        /// Gets a new instance of the marshaller.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static ICustomMarshaler GetInstance(string cookie)
        {
            if (marshaler == null)
                marshaler = new FilenameMarshaler();

            return marshaler;
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            throw new NotImplementedException();
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

    }

}
