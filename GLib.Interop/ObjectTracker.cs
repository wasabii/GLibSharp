using System;
using System.Collections.Concurrent;

namespace GLib.Interop
{

    /// <summary>
    /// Tracks managed objects related to native objects.
    /// </summary>
    public static class ObjectTracker
    {

        readonly static ConcurrentDictionary<IntPtr, ToggleRef> toggles = new ConcurrentDictionary<IntPtr, ToggleRef>();
        readonly static ConcurrentDictionary<object, IntPtr> references = new ConcurrentDictionary<object, IntPtr>();

        ToggleRef GetToggleRef(INativeHandle handle)
        {
            return toggles.GetOrAdd(handle.Handle, _ => new ToggleRef(handle));
        }

    }

}
