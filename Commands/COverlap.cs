using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class COverlap:BaseCommand, ICommandComposition{
	public float DelayIn = 0.3f;
	public float DelayOut = 0.1f;
	private List<ICommand> list;
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
		list.AddRange( cmnds );
	}

	private void init(){
		list = new List<ICommand>();
		cursor = -1;
	}

	public void Add( ICommand cmnd ){
		list.Add( cmnd );
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
		EmBox.CallLater( DoNext, DelayIn );
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
		EmBox.CallLater( DoNext, DelayOut );
	}

	private void DoNext(){
		switch( State ){
		case States.ExecutingIn:
			if( cursor == list.Count - 1 ){
				return;
			}
			cursor++;
			list[ cursor ].ExecuteIn();
			EmBox.CallLater( DoNext, DelayIn );
			break;
		case States.ExecutingOut:
			if( cursor == 0 ){
				return;
			}
			cursor--;
			list[ cursor ].ExecuteOut();
			EmBox.CallLater( DoNext, DelayOut );
			break;
		}
	}
		
	private void resetFnCounts(){
		completeCmnds = 0;
		for( int i = 0; i < list.Count; i++ ){
			list[ i ].FnInComplete = null;
			list[ i ].FnOutComplete = null;
		}
	}

	private void count(){
		completeCmnds++;
//      Debug.Log("State " + State);
//      Debug.Log("completeCmnds " + completeCmnds);
//      Debug.Log("list.Count " + list.Count);
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

