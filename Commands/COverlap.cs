using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class COverlap:BaseCommand, ICommandComposition{
	public float DelayIn = 0.3f;
	public float DelayOut = 0.1f;
	private List<ICommand> cmnds;
	private List<int> guids;
	private List<int> guidsIn;//unique ids for CallLlater fn()
	private List<int> guidsOut;//unique ids for CallLlater fn()
	private List<int> guidsInComplete;
	private List<int> guidsOutComplete;
	private int cursor;
	private int completeCmnds;
	private int cmndsToComplete;
		
	public COverlap()
  : base( ){
		init();
	}

	public COverlap( params ICommand[] cmnds )
  : base( ){
		init();
		for( int i = 0; i < cmnds.Length; i++ ){
			this.cmnds[ i ] = cmnds[ i ];
			guids.Add( cmnds[ i ].GUID );
			guidsIn.Add( EmBox.GUID );
			guidsOut.Add( EmBox.GUID );
			guidsInComplete.Add( EmBox.GUID );
			guidsOutComplete.Add( EmBox.GUID );
		}
	}

	private void init(){
		cmnds = new List<ICommand>();
		guids = new List<int>();
		guidsIn = new List<int>();
		guidsOut = new List<int>();
		guidsInComplete = new List<int>();
		guidsOutComplete = new List<int>();
		cursor = 0;
	}

	public void Add( ICommand cmnd ){
//			if(cmnd==null)return;
		cmnds.Add( cmnd );
		guids.Add( cmnd.GUID );
		guidsIn.Add( EmBox.GUID );
		guidsOut.Add( EmBox.GUID );
		guidsInComplete.Add( EmBox.GUID );
		guidsOutComplete.Add( EmBox.GUID );
	}
		
	protected override void DoIn(){
		if( cmnds.Count == 0 ){
			ExecuteInComplete();
			return;
		}
		cmndsToComplete = cmnds.Count - cursor;
		cmnds[ cursor ].AddOnInComplete( countIn, guidsInComplete[ cursor ] );
		cmnds[ cursor ].ExecuteIn();
		if( cursor + 1 <= cmnds.Count - 1 ){
			EmBox.CallLater( DoNext, DelayIn, guidsIn[ cursor + 1 ] );
		}
	}

	protected override void CancelIn(){
		completeCmnds = 0;
		for( int i = 0; i < cmnds.Count; i++ ){
			cmnds[ i ].RemOnInComplete( guidsInComplete[ i ] );
			EmBox.CallLaterCancel( guidsIn[ i ] );
		}
	}
		
	protected override void DoOut(){
		if( cmnds.Count == 0 ){
			ExecuteOutComplete();
			return;
		}
		cmndsToComplete = cursor + 1;
		cmnds[ cursor ].AddOnOutComplete( countOut, guidsOutComplete[ cursor ] );
		cmnds[ cursor ].ExecuteOut();
		if( cursor - 1 >= 0 ){
			EmBox.CallLater( DoNext, DelayOut, guidsOut[ cursor - 1 ] );
		}
	}

	protected override void CancelOut(){
		completeCmnds = 0;
		for( int i = 0; i < cmnds.Count; i++ ){
			cmnds[ i ].RemOnOutComplete( guidsOutComplete[ i ] );
			EmBox.CallLaterCancel( guidsOut[ i ] );
		}
	}

	private void DoNext(){
		switch( State ){
		case States.ExecutingIn:
			if( cursor == cmnds.Count - 1 ){
				return;
			}
			cursor++;
			cmnds[ cursor ].AddOnInComplete( countIn, guidsInComplete[ cursor ] );
			cmnds[ cursor ].ExecuteIn();
			if( cursor + 1 <= cmnds.Count - 1 ){
				EmBox.CallLater( DoNext, DelayIn, guidsIn[ cursor + 1 ] );
			}
			break;
		case States.ExecutingOut:
			if( cursor == 0 ){
				return;
			}
			cursor--;
			cmnds[ cursor ].AddOnOutComplete( countOut, guidsOutComplete[ cursor ] );
			cmnds[ cursor ].ExecuteOut();
			if( cursor - 1 >= 0 ){
				EmBox.CallLater( DoNext, DelayOut, guidsOut[ cursor - 1 ] );
			}
			break;
		}
	}
		
	private void countIn( int guid ){
		cmnds[ guidsInComplete.IndexOf( guid ) ].RemOnInComplete( guid );
		completeCmnds++;
//		Debug.Log( " completed  " + completeCmnds + "/" + cmndsToComplete + " cmnds.Count:" + cmnds.Count );
		if( completeCmnds == cmndsToComplete ){
			completeCmnds = 0;
			ExecuteInComplete();
		}
	}

	private void countOut( int guid ){
		cmnds[ guidsOutComplete.IndexOf( guid ) ].RemOnOutComplete( guid );
		completeCmnds++;
//		Debug.Log( " completed  " + completeCmnds + "/" + cmndsToComplete + " cmnds.Count:" + cmnds.Count );
		if( completeCmnds == cmndsToComplete ){
			completeCmnds = 0;
			ExecuteOutComplete();
		}
	}
}
}

