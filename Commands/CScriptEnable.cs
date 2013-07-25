using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CScriptEnable:BaseCommand{
	private MonoBehaviour mb;

	public CScriptEnable( MonoBehaviour mb, bool initValue=false ): base( ){
		this.mb = mb;
		mb.enabled = initValue;
	}

	protected override void DoIn(){
//			Debug.Log(">: " + mb.GetType());
		mb.enabled = true;
		ExecuteInComplete();
	}

	protected override void DoOut(){
//			Debug.Log("<: " + mb.GetType());
		mb.enabled = false;
		ExecuteOutComplete();
	}
}
}

