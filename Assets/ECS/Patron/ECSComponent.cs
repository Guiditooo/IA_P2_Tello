using System.Collections.Generic;
using System;
public abstract class ECSComponent
{
	private uint entityOwnerID = 0;

	public uint EntityOwnerID { get => entityOwnerID; set => entityOwnerID = value; }

	protected ECSComponent() { }

	public virtual void Dispose() { }


	Dictionary<Type, Dictionary<int, ECSComponent>> _components;
    _components[type][entityId]
}
