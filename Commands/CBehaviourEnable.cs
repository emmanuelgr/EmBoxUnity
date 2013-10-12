using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CBehaviourEnable:BaseCommand{
	private Behaviour behaviour;
	private bool waitForInverseValueBeforeIn;
	private bool waitForInverseValueBeforeOut;

	public CBehaviourEnable( Behaviour behaviour, bool initValue=false, bool waitForInverseValueBeforeIn=false, bool waitForInverseValueBeforeOut=false ): base( ){
		this.behaviour = behaviour;
		this.waitForInverseValueBeforeIn = waitForInverseValueBeforeIn;
		this.waitForInverseValueBeforeOut = waitForInverseValueBeforeOut;
		behaviour.enabled = initValue;
	}

	protected override void DoIn(){
		if( waitForInverseValueBeforeIn ){
			if( behaviour.enabled == true ){
				EmBox.UPDATE.Add( checkIn );
			} else{
				behaviour.enabled = true;
				ExecuteInComplete();
			}
		} else{
			behaviour.enabled = true;
			ExecuteInComplete();
		}
	}

	protected override void DoOut(){
		if( waitForInverseValueBeforeOut ){
			if( behaviour.enabled == false ){
				EmBox.UPDATE.Add( checkOut );
			} else{
				behaviour.enabled = false;
				ExecuteOutComplete();
			}
		} else{
			behaviour.enabled = false;
			ExecuteOutComplete();
		}
	}
		
	private void checkIn(){
		if( !behaviour.enabled ){
			behaviour.enabled = true;
			ExecuteInComplete();
		}
	}
				
	private void checkOut(){
		if( behaviour.enabled ){
			behaviour.enabled = false;
			ExecuteOutComplete();
		}
	}
}
}

