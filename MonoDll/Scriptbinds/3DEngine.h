/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// 3DEngine scriptbind
//////////////////////////////////////////////////////////////////////////
// 13/01/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __SCRIPTBIND_3DENGINE__
#define __SCRIPTBIND_3DENGINE__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

#include "Renderer.h"

class CScriptbind_3DEngine : public IMonoScriptBind
{
public:
	CScriptbind_3DEngine();
	~CScriptbind_3DEngine() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "Engine"; }
	// ~IMonoScriptBind

	static float GetTerrainElevation(int x, int y, bool includeVoxels);

	static int GetTerrainSize();
	static int GetTerrainSectorSize();
};

#endif //__SCRIPTBIND_3DENGINE__