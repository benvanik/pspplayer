// ----------------------------------------------------------------------------
// PSP Player Emulation Suite
// Copyright (C) 2006 Ben Vanik (noxa)
// Licensed under the LGPL - see License.txt in the project root for details
// ----------------------------------------------------------------------------

#include "StdAfx.h"
#include "OglDriver.h"
#include "VideoApi.h"
#include <string>

using namespace Noxa::Emulation::Psp;
using namespace Noxa::Emulation::Psp::Video;
using namespace Noxa::Emulation::Psp::Video::Native;

DisplayList^ OglDriver::FindDisplayList( int displayListId )
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;

	VideoDisplayList* ndl = ni->FindList( displayListId );
	if( ndl == NULL )
		return nullptr;

	// TODO: convert to managed display list

	return nullptr;
}

bool OglDriver::Enqueue( DisplayList^ displayList, bool immediate )
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;
	
	// TODO: convert to native display list
	VideoDisplayList* ndl = NULL;
	int ret = ni->EnqueueList( ndl, immediate );
	if( ret >= 0 )
	{
		// TODO: store back in display list
		displayList->ID = ndl->ID;
		return true;
	}
	else
		return false;
}

void OglDriver::Abort( int displayListId )
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;
	ni->DequeueList( displayListId );
}

void OglDriver::Sync( DisplayList^ displayList )
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;
	ni->SyncList( displayList->ID, SYNC_LIST_DONE );
}

void OglDriver::Sync()
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;
	ni->Sync( SYNC_LIST_DONE );
}