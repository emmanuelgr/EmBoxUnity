using System;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CDebugLog :BaseCommand{
	public string StringIn;
	public string StringOut;
      
	public CDebugLog( string stringIn ):base( ){
		StringIn = StringOut = stringIn;
	}

	public CDebugLog( string stringIn, string stringOut ):base( ){
		StringIn = stringIn;
		StringOut = stringOut;
	}
      
	protected override void DoIn(){
		base.DoIn();
		if( StringIn != null )
			Debug.Log( StringIn );
		ExecuteInComplete();
        
	}

	protected override void DoOut(){
		base.DoOut();
		if( StringOut != null )
			Debug.Log( StringOut );
		ExecuteOutComplete();
	}

}
}

