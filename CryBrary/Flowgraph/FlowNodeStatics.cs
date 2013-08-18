using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Initialization;

using CryEngine.Native;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	public abstract partial class FlowNode : CryScriptInstance
	{
		internal static void Register(string typeName)
		{
			NativeFlowNodeMethods.RegisterNode(typeName);
		}

		internal static void InternalRemove(UInt16 nodeId, UInt32 graphId)
		{
			ScriptManager.Instance.RemoveInstances(ScriptType.FlowNode, instance =>
			{
				var node = instance as FlowNode;
				if (node != null && node.NodeId == nodeId && node.GraphId == graphId)
				{
					node.OnRemove();

					return true;
				}

				return false;
			});
		}
	}
}
