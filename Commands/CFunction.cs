using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CFunction:BaseCommand{
	public delegate void Action();

	public Action FnIn;
	public Action FnOut;
    
	public CFunction( Action FnIn, Action FnOut )
    : base( ){
		this.FnIn = FnIn;
		this.FnOut = FnOut;
	}
    
	protected override void DoIn(){
		if( FnIn != null ){
			FnIn();
		}
		ExecuteInComplete();
	}
    
	protected override void DoOut(){
		if( FnOut != null ){
			FnOut();
		}
		ExecuteOutComplete();
	}
    
}
}