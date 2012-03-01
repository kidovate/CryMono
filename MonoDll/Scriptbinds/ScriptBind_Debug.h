///////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Scriptbind to hold various debugging methods
//////////////////////////////////////////////////////////////////////////
// 01/03/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#ifndef __SCRIPTBIND_DEBUG__
#define __SCRIPTBIND_DEBUG__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

struct IPersistantDebug;

class CScriptBind_Debug : public IMonoScriptBind
{
public:
	CScriptBind_Debug();
	~CScriptBind_Debug() {}

protected:

	// Externals

	// IPersistantDebug
	static void AddPersistantSphere(Vec3 pos, float radius, ColorF color, float timeout);
	static void AddDirection(Vec3 pos, float radius, Vec3 dir, ColorF color, float timeout);
	static void AddPersistantText2D(mono::string text, float size, ColorF color, float timeout);
	// ~IPersistantDebug

	// ~Externals

	static IPersistantDebug *GetIPersistantDebug();

	// IMonoScriptBind
	virtual const char *GetNamespace() override { return "CryEngine.Utils"; }
	virtual const char *GetClassName() override { return "Debug"; }
	// ~IMonoScriptBind
};

#endif