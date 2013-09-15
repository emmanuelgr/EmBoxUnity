using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CGameObjEnable:BaseCommand{
	private GameObject go;

	public CGameObjEnable( GameObject go, bool initValue=false ): base( ){
		this.go = go;
		go.SetActive( initValue );
	}

	protected override void DoIn(){
			go.SetActive( true );
		ExecuteInComplete();
	}

	protected override void DoOut(){
			go.SetActive( false );
		ExecuteOutComplete();
	}
}
}

