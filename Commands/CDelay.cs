using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using System.Collections;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CDelay:BaseCommand{
	public float DelayIn;
	public float DelayOut;
	private float explode;
    
	public CDelay( float DelayIn, float DelayOut )
    : base( ){
		this.DelayIn = DelayIn;
		this.DelayOut = DelayOut;
		init();
	}
    
	private void init(){
	}
    
	protected override void DoIn(){
//    explode = Time.time + DelayIn;
		EmBox.CallLater( Update, DelayIn );
	}
    
	protected override void DoOut(){
//    explode = Time.time + DelayOut;
		EmBox.CallLater( Update, DelayOut );
	}
    
	void Update(){
		switch( State ){
		case States.ExecutingIn:
//          if( explode <= Time.time ) ExecuteInComplete();
		ExecuteInComplete();
		break;
		case States.ExecutingOut:
//          if( explode <= Time.time )  ExecuteOutComplete();
		ExecuteOutComplete();
		break;
		}
	}
   
}
}