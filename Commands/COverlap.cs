using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class COverlap:BaseCommand, ICommandComposition{
	public float DelayIn = 0.3f;
	public float DelayOut = 0.1f;
	private List<ICommand> list;
	private List<int> guidsIns;
	private List<int> guidsOut;
	private int cursor;
	private float explode;
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
			list[ i ] =cmnds[i];
			guidsIns[ i ] = EmBox.GUID;
			guidsOut[ i ] = EmBox.GUID;
		}
	}

	private void init(){
		list = new List<ICommand>();
		guidsIns = new List<int>();
		guidsOut = new List<int>();
		cursor = -1;
	}

	public void Add( ICommand cmnd ){
		list.Add( cmnd );
		guidsIns.Add( EmBox.GUID );
		guidsOut.Add( EmBox.GUID );
	}
		
	protected override void DoIn(){
		resetFnCounts();
		if( list.Count == 0 ){
			ExecuteInComplete();
			return;
		}
		if( cursor == -1 ){
			cursor = 0;
		}
		cmndsToComplete = list.Count - cursor;
		for( int i = 0; i < cmndsToComplete; i++ ){
			list[ i ].FnInComplete = count;
		}
		list[ cursor ].ExecuteIn();
				if( cursor +1 <= list.Count-1){
		EmBox.CallLater( DoNext, DelayIn, guidsIns[ cursor + 1 ] );}
	}

	protected override void DoOut(){
		resetFnCounts();
		if( list.Count == 0 ){
			ExecuteOutComplete();
			return;
		}
		if( cursor == -1 ){
			cursor = list.Count - 1;
		}
		cmndsToComplete = cursor + 1;
		for( int i = cursor; i >= 0; i-- ){
			list[ i ].FnOutComplete = count;
		}
		list[ cursor ].ExecuteOut();
					if( cursor -1 >= 0){
		EmBox.CallLater( DoNext, DelayOut, guidsOut[ cursor - 1 ] );}
	}

	private void DoNext(){
		switch( State ){
		case States.ExecutingIn:
			if( cursor == list.Count - 1 ){
				return;
			}
			cursor++;
			list[ cursor ].ExecuteIn();
				if( cursor +1 <= list.Count-1){
			EmBox.CallLater( DoNext, DelayIn, guidsIns[ cursor + 1 ] );}
			break;
		case States.ExecutingOut:
			if( cursor == 0 ){
				return;
			}
			cursor--;
			list[ cursor ].ExecuteOut();
					if( cursor -1 >= 0){
			EmBox.CallLater( DoNext, DelayOut, guidsOut[ cursor - 1 ] );}
			break;
		}
	}
		
	private void resetFnCounts(){
		completeCmnds = 0;
		for( int i = 0; i < list.Count; i++ ){
			list[ i ].FnInComplete = null;
			list[ i ].FnOutComplete = null;
			EmBox.CallLaterCancel( guidsIns[ i ] );
			EmBox.CallLaterCancel( guidsOut[ i ] );
		}
	}

	private void count(){
		completeCmnds++;
//      Debug.Log( "State " + State +" completed  " + completeCmnds + "/" + cmndsToComplete + " list.Count:" + list.Count);
		if( completeCmnds == cmndsToComplete ){
			cursor = -1;
			if( State == States.ExecutingIn ){
				ExecuteInComplete();
			} else if( State == States.ExecutingOut ){
				ExecuteOutComplete();
			}
		}
	}
}
}

