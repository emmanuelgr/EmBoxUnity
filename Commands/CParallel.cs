using System;
using EmBoxUnity.Commands.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CParallel:BaseCommand, ICommandComposition{
	private List<ICommand> cmnds;
	private List<int> guids;
	private List<int> guidsInComplete;
	private List<int> guidsOutComplete;
	private int completeCmnds;

	public CParallel()
      : base( ){
		init();
	}

	public CParallel( params ICommand[] cmnds ): base( ){
		init();
		this.cmnds.AddRange( cmnds );
		for( int i = 0; i < cmnds.Length; i++ ){
			guids.Add( cmnds[ i ].GUID );
			guidsInComplete.Add( EmBox.GUID );
			guidsOutComplete.Add( EmBox.GUID );
		}
	}

	private void init(){
		cmnds = new List<ICommand>();
		guids = new List<int>();
		guidsInComplete = new List<int>();
		guidsOutComplete = new List<int>();
	}

	public void Add( ICommand cmnd ){
		cmnds.Add( cmnd );
		guids.Add( cmnd.GUID );
		guidsInComplete.Add( EmBox.GUID );
		guidsOutComplete.Add( EmBox.GUID );
	}

	protected override void DoIn(){
		completeCmnds = 0;
		for( int i = 0; i < cmnds.Count; i++ ){
			cmnds[ i ].AddOnInComplete( count, guidsInComplete[ i ] );
			cmnds[ i ].ExecuteIn();
		}
	}

	protected override void CancelIn(){
		for( int i = 0; i < cmnds.Count; i++ ){
			cmnds[ i ].RemOnInComplete( guidsInComplete[ i ] );
		}
	}
		
	protected override void DoOut(){
		completeCmnds = 0;
		for( int i = cmnds.Count -1; i >= 0; i-- ){
			cmnds[ i ].AddOnOutComplete( count, guidsOutComplete[ i ] );
			cmnds[ i ].ExecuteOut();
		}
	}

	protected override void CancelOut(){
		for( int i = 0; i < cmnds.Count; i++ ){
			cmnds[ i ].RemOnOutComplete( guidsOutComplete[ i ] );
		}
	}
		
	private void count( int guid ){
		if( State == States.ExecutingIn ){
			cmnds[ guidsInComplete.IndexOf( guid ) ].RemOnInComplete( guid );
		} else if( State == States.ExecutingOut ){
			cmnds[ guidsOutComplete.IndexOf( guid ) ].RemOnOutComplete( guid );
		}
		completeCmnds++;
//      Debug.Log("State " + State);
//      Debug.Log("completeCmnds " + completeCmnds);
//      Debug.Log("list.Count " + list.Count);
		if( completeCmnds == cmnds.Count ){
			if( State == States.ExecutingIn ){
				ExecuteInComplete();
			} else if( State == States.ExecutingOut ){
				ExecuteOutComplete();
			}
		}
	}
}
}

