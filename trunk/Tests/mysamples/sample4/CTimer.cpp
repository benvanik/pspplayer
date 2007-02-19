#include "CTimer.h"

CTimer::CTimer( void )
{
	
}

void CTimer::Init( )
{
	sceRtcGetCurrentTick( &timeLastAsk );
	tickResolution = sceRtcGetTickResolution();

}

double CTimer::GetDeltaTime( void )
{
	sceRtcGetCurrentTick( &timeNow );
	double dt = ( timeNow - timeLastAsk ) / ((float) tickResolution/1000 );
	timeLastAsk = timeNow;

	return dt;
}
