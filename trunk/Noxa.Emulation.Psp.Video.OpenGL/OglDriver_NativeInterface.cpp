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

VideoDisplayList* niFindList( int listId )
{
	return NULL;
}

int niEnqueueList( VideoDisplayList* list, bool immediate )
{
	return 0;
}

void niDequeueList( int listId )
{
}

void niSyncList( int listId )
{
}

void niSync()
{
}

void OglDriver::FillNativeInterface()
{
	VideoApi* ni = ( VideoApi* )_nativeInterface;

	ni->FindList = &niFindList;
	ni->EnqueueList = &niEnqueueList;
	ni->DequeueList = &niDequeueList;
	ni->SyncList = &niSyncList;
	ni->Sync = &niSync;
}
