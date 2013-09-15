using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CScriptEnable:BaseCommand{
	private Behaviour behaviour;

	public CScriptEnable( Behaviour behaviour, bool initValue=false ): base( ){
		this.behaviour = behaviour;
		behaviour.enabled = initValue;
	}

	protected override void DoIn(){
		behaviour.enabled = true;
		ExecuteInComplete();
	}

	protected override void DoOut(){
		behaviour.enabled = false;
		ExecuteOutComplete();
	}
}
}

