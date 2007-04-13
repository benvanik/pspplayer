// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#pragma once

#include "NoxaShared.h"
#include "KHandle.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::IO;
using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Bios;
using namespace Noxa::Emulation::Psp::Media;

namespace Noxa {
	namespace Emulation {
		namespace Psp {
			namespace Bios {

				class KDevice;

				enum SpecialFileHandle
				{
					HSTDIN	= 0,
					HSTDOUT	= 1,
					HSTDERR	= 2,
				};

				class KFile : public KHandle
				{
				public:
					KDevice*			Device;
					gcref<IMediaItem^>	Item;

					int					FolderOffset;
					gcref<Stream^>		BoundStream;

					bool				IsOpen;
					bool				CanWrite;
					bool				CanSeek;

					bool				IsStd;				// true if stdin/out/err

				public:
					KFile( KDevice* device, IMediaItem^ item );
					KFile( KDevice* device, IMediaItem^ item, Stream^ stream, bool readOnly );
					KFile( int specialFileHandle );
					~KFile();

				public:
					// Only use if IsStd == true
					virtual void Write( const byte* buffer, int count ){}
					virtual void Write( const char* string ){}
					virtual void Write( String^ string ){}
				};

				class KStdFile : public KFile
				{
				public:
					int			StdType;

				private:
					byte*		_buffer;
					int			_bufferIndex;

				public:
					KStdFile( int stdType );
					~KStdFile();

					virtual void Write( const byte* buffer, int count );
					virtual void Write( const char* string );
					virtual void Write( String^ string );
				};

			}
		}
	}
}
