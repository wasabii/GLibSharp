using System;

namespace GLib.Interop
{

    /// <summary>
    /// Describes a type as possessing a native handle to unmanaged memory.
    /// </summary>
    public interface INativeHandle
    {

        /// <summary>
        /// Gets the native handle value.
        /// </summary>
        IntPtr Handle { get; }

    }

}
