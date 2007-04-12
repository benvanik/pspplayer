// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

/*
 A lot of this was taken from the C++/CLI type gcroot in gcroot.h
 I wanted something that disallowed ->, as for each -> access it requires a
 lot of overhead and I don't trust myself to not use it :)
 */

#define __GCHANDLE_TO_VOIDPTR(x) ((GCHandle::operator System::IntPtr(x)).ToPointer())
#define __VOIDPTR_TO_GCHANDLE(x) (GCHandle::operator GCHandle(System::IntPtr(x)))

namespace Noxa {
	namespace Emulation {
		namespace Psp {

			using namespace System::Runtime::InteropServices;

			template<class T>
			struct gcref
			{
				// always allocate a new handle during construction (see above)
				//
				[System::Diagnostics::DebuggerStepThroughAttribute]
				gcref()
				{
					_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(nullptr));
				}

				// this can't be T& here because & does not yet work on managed types
				// (T should be a pointer anyway).
				//
				gcref(T t)
				{
					_handle = __GCHANDLE_TO_VOIDPTR(GCHandle::Alloc(t));
				}

				gcref(const gcref& r)
				{
					// don't copy a handle, copy what it points to (see above)
					_handle = __GCHANDLE_TO_VOIDPTR(
									GCHandle::Alloc(
										__VOIDPTR_TO_GCHANDLE(r._handle).Target ));
				}

				// Since C++ objects and handles are allocated 1-to-1, we can 
				// free the handle when the object is destroyed
				//
				[System::Diagnostics::DebuggerStepThroughAttribute]
				~gcref()
				{
					GCHandle g = __VOIDPTR_TO_GCHANDLE(_handle);
					g.Free();
					_handle = 0; // should fail if reconstituted
				}

				[System::Diagnostics::DebuggerStepThroughAttribute]
				gcref& operator=(T t)
				{
					// no need to check for valid handle; was allocated in ctor
					__VOIDPTR_TO_GCHANDLE(_handle).Target = t;
					return *this;
				}

				gcref& operator=(const gcref &r)
				{
					// no need to check for valid handle; was allocated in ctor
					T t = (T)r;
					__VOIDPTR_TO_GCHANDLE(_handle).Target = t;
					return *this;
				}

				operator T() const
				{
					// gcroot is typesafe, so use static_cast
					return static_cast<T>( __VOIDPTR_TO_GCHANDLE(_handle).Target );
				}

			private:
				// Don't let anyone copy the handle value directly, or make a copy
				// by taking the address of this object and pointing to it from
				// somewhere else.  The root will be freed when the dtor of this
				// object gets called, and anyone pointing to it still will
				// cause serious harm to the Garbage Collector.
				//
				void* _handle;
			};

		}
	}
}

#undef __GCHANDLE_TO_VOIDPTR
#undef __VOIDPTR_TO_GCHANDLE
