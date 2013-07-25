using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CAddComponent:BaseCommand{
	private Component compon;
	private string type;
	private GameObject gameObj;

	public CAddComponent( string type, GameObject gameObj ): base( ){
		this.type = type;
		this.gameObj = gameObj;
	}

	protected override void DoIn(){
//		if( gameObj != null || type != null ){
			compon = gameObj.AddComponent( type );
//		}
		ExecuteInComplete();
	}

	protected override void DoOut(){
		Component.Destroy( compon );
		ExecuteOutComplete();
	}
}
}

