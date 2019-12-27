using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GLib.Interop
{

    /// <summary>
    /// Maintains an object instance until signaled by GLib.
    /// </summary>
    class ToggleRef : IDisposable
    {

        /// <summary>
        /// Signature invoked by the toggle ref notification callback.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="handle"></param>
        /// <param name="is_last_ref"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ToggleNotifyHandler(IntPtr data, IntPtr handle, bool is_last_ref);

        static void RefToggled(IntPtr data, IntPtr handle, bool is_last_ref)
        {
            try
            {
                var gch = (GCHandle)data;
                var tref = (ToggleRef)gch.Target;
                tref.Toggle(is_last_ref);
            }
            catch (Exception e)
            {
                ExceptionManager.RaiseUnhandledException(e, false);
            }
        }

        /// <summary>
        /// Delegate to be invoked when notified.
        /// </summary>
        static ToggleNotifyHandler toggleNotifyCallback = RefToggled;
        static List<ToggleRef> pendingDestroys = new List<ToggleRef>();
        static bool idle_queued;

        IntPtr handle;
        object target;
        GCHandle thnd;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target"></param>
        public ToggleRef(object target)
        {
            // associate handle with object if object is wrapper for native
            if (target is INativeHandle n)
                handle = n.Handle;

            handle = target.Handle;
            thnd = GCHandle.Alloc(this);
            this.target = target;
            g_object_add_toggle_ref(target.Handle, toggleNotifyCallback, (IntPtr)thnd);
            g_object_unref(target.Handle);
        }

        /// <summary>
        /// Gets the handle of the held object.
        /// </summary>
        public IntPtr Handle => handle;

        /// <summary>
        /// Gets the held object.
        /// </summary>
        public object Target => target;

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            lock (pendingDestroys)
                pendingDestroys.Remove(this);

            Free();
        }

        /// <summary>
        /// Frees the instance.
        /// </summary>
        void Free()
        {
            if (hardened)
                g_object_unref(handle);
            else
                g_object_remove_toggle_ref(handle, toggleNotifyCallback, (IntPtr)thnd);
            target = null;
            thnd.Free();
        }

        internal void Harden()
        {
            // Added for the benefit of GnomeProgram.  It releases a final ref in
            // an atexit handler which causes toggle ref notifications to occur after 
            // our delegates are gone, so we need a mechanism to override the 
            // notifications.  This method effectively leaks all objects which invoke it, 
            // but since it is only used by Gnome.Program, which is a singleton object 
            // with program duration persistence, who cares.

            g_object_ref(handle);
            g_object_remove_toggle_ref(handle, toggleNotifyCallback, (IntPtr)thnd);
            if (target is WeakReference)
                target = (target as WeakReference).Target;
            hardened = true;
        }

        void Toggle(bool is_last_ref)
        {
            if (is_last_ref && target is GLib.Object)
                target = new WeakReference(target);
            else if (!is_last_ref && target is WeakReference)
            {
                WeakReference weak = target as WeakReference;
                target = weak.Target;
            }
        }

        public void QueueUnref()
        {
            lock (pendingDestroys)
            {
                pendingDestroys.Add(this);
                if (!idle_queued)
                {
                    Timeout.Add(50, new TimeoutHandler(PerformQueuedUnrefs));
                    idle_queued = true;
                }
            }
        }

        static bool PerformQueuedUnrefs()
        {
            ToggleRef[] references;

            lock (pendingDestroys)
            {
                references = new ToggleRef[pendingDestroys.Count];
                pendingDestroys.CopyTo(references, 0);
                pendingDestroys.Clear();
                idle_queued = false;
            }

            foreach (ToggleRef r in references)
                r.Free();

            return false;
        }

        [DllImport(Global.GObjectNativeDll, CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_add_toggle_ref(IntPtr raw, ToggleNotifyHandler notify_cb, IntPtr data);

        [DllImport(Global.GObjectNativeDll, CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_remove_toggle_ref(IntPtr raw, ToggleNotifyHandler notify_cb, IntPtr data);

        [DllImport(Global.GObjectNativeDll, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_object_ref(IntPtr raw);

        [DllImport(Global.GObjectNativeDll, CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_unref(IntPtr raw);

    }
}
