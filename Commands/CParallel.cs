using System;
using EmBoxUnity.Commands.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CParallel:BaseCommand, ICommandComposition{
	private List<ICommand> list;
	private int completeCmnds;

	public CParallel()
      : base( ){
		init();
	}

	public CParallel( params ICommand[] cmnds ): base( ){
		init();
		list.AddRange( cmnds );
	}

	private void init(){
		list = new List<ICommand>();
	}

	public void Add( ICommand cmnd ){
		list.Add( cmnd );
	}

	protected override void DoIn(){
		completeCmnds = 0;
		for( int i = 0; i < list.Count; i++ ){
			list[ i ].FnInComplete = count;
			list[ i ].ExecuteIn();
		}
	}

	protected override void DoOut(){
		completeCmnds = 0;
		for( int i = list.Count -1; i >= 0; i-- ){
			list[ i ].FnOutComplete = count;
			list[ i ].ExecuteOut();
		}
	}

	private void count(){
		completeCmnds++;
//      Debug.Log("State " + State);
//      Debug.Log("completeCmnds " + completeCmnds);
//      Debug.Log("list.Count " + list.Count);
		if( completeCmnds == list.Count ){
			if( State == States.ExecutingIn ){
				ExecuteInComplete();
			} else if( State == States.ExecutingOut ){
				ExecuteOutComplete();
			}
		}
	}
}
}

