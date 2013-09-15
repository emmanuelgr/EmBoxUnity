using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using System.Collections;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CDelay:BaseCommand{
	public float DelayIn;
	public float DelayOut;
	private int inGUID ,outGUID;
    
	public CDelay( float DelayIn, float DelayOut )
    : base( ){
		this.DelayIn = DelayIn;
		this.DelayOut = DelayOut;
		init();
	}
    
	private void init(){
		inGUID = EmBox.GUID;
		outGUID = EmBox.GUID;
//		Debug.Log( "in: " + inGUID + " put: " + outGUID );
	}
    
	protected override void DoIn(){
		if( DelayIn <= 0 ){
			ExecuteInComplete();
		} else{
			EmBox.CallLater( ExecuteInComplete, DelayIn, inGUID );
		}
	}

	protected override void CancelIn(){
		EmBox.CallLaterCancel( outGUID );
	}
    
	protected override void DoOut(){
		if( DelayOut <= 0 ){
			ExecuteOutComplete();
		} else{
			EmBox.CallLater( ExecuteOutComplete, DelayOut, outGUID );
		}
	}

	protected override void CancelOut(){
		EmBox.CallLaterCancel( inGUID );
	}

   
}
}