#include "stdafx.h"
#include "CryScriptInstance.h"

IMonoMethod *CCryScriptInstance::m_pGetScriptInstanceById = nullptr;

CCryScriptInstance::CCryScriptInstance(EMonoScriptFlags flags)
	: m_flags(flags)
	, m_scriptId(0)
{
	g_pScriptSystem->AddListener(this);
}

CCryScriptInstance::~CCryScriptInstance()
{
	if(g_pScriptSystem)
	{
		g_pScriptSystem->ReportScriptInstanceDestroyed(this, m_scriptId);
		g_pScriptSystem->RemoveListener(this);
	}
}

void CCryScriptInstance::Release(bool triggerGC)
{
	delete this;
}

void CCryScriptInstance::SetManagedObject(MonoObject *newObject, bool allowGC)
{
	CScriptObject::SetManagedObject(newObject, allowGC);

	IMonoObject *pScriptIdResult = *CScriptObject::GetPropertyValue("ScriptId");
	m_scriptId = pScriptIdResult->Unbox<int>();
	pScriptIdResult->Release();
}

void CCryScriptInstance::OnReloadStart()
{
	m_pClass = nullptr;
	m_pObject = nullptr;

	m_objectHandle = -1;
}

void CCryScriptInstance::OnReloadComplete()
{
	if(m_scriptId == 0)
		return;

	IMonoObject *pScriptManager = g_pScriptSystem->GetScriptManager();

	if(mono::object result = m_pGetScriptInstanceById->Call(pScriptManager->GetManagedObject(), m_scriptId, m_flags))
	{
		SetManagedObject((MonoObject *)result, true);
	}
	else
		MonoWarning("Failed to locate script instance %i after reload!", m_scriptId);
}

void CCryScriptInstance::CacheMethods()
{
	IMonoClass *pScriptManagerClass = g_pScriptSystem->GetScriptManager()->GetClass();

	m_pGetScriptInstanceById = pScriptManagerClass->GetMethod("GetScriptInstanceById", 2);
}