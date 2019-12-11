// glib/IOChannel.cs : IOChannel API wrapper
//
// Author: Mike Kestner  <mkestner@novell.com>
//
// Copyright (c) 2007 Novell, Inc.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of version 2 of the Lesser GNU General 
// Public License as published by the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.


namespace GLibSharp
{

    using System;
    using GLib;

    public partial class IOFuncWrapper
    {

        public IOFuncNative NativeDelegate;

        public IOFuncWrapper(IOFunc managed)
        {
            this.managed = managed;
            NativeDelegate = new IOFuncNative(NativeCallback);
        }

        bool NativeCallback(IntPtr source, int condition, IntPtr data)
        {
            try
            {
                return managed(IOChannel.FromHandle(source), (IOCondition)condition);
            }
            catch (Exception e)
            {
                ExceptionManager.RaiseUnhandledException(e, false);
                return false;
            }
        }
    }
}
