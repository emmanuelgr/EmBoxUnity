using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CFunctionCompleteBefore:BaseCommand{
	public delegate void Action();
	public Action FnIn;
	public Action FnOut;
    
	public CFunctionCompleteBefore( Action FnIn, Action FnOut )
    : base( ){
		this.FnIn = FnIn;
		this.FnOut = FnOut;
	}
    
	protected override void DoIn(){
		ExecuteInComplete();
		if( FnIn != null ){
			FnIn();
		}
	}
    
	protected override void DoOut(){
		ExecuteOutComplete();
		if( FnOut != null ){
			FnOut();
		}
	}
    
}
}