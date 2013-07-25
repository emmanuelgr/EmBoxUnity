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
		if( list.Count == 0 ){
			ExecuteInComplete();
			return;
		}
		if( cursor == -1 ){
			cursor = 0;
			list[ cursor ].ExecuteIn();
		}
		EmBox.CallLater( DoNext, DelayIn );
	}

	protected override void DoOut(){
		if( list.Count == 0 ){
			ExecuteOutComplete();
			return;
		}
		if( cursor == -1 ){
			cursor = list.Count - 1;
			list[ cursor ].ExecuteOut();
		}
		EmBox.CallLater( DoNext, DelayOut );
	}

	private void DoNext(){
		switch( State ){
		case States.ExecutingIn:
			cursor++;
			if( cursor > list.Count-1 ){
				cursor = -1;
				ExecuteInComplete();
				return;
			}
			list[ cursor ].ExecuteIn();
//          explode = Time.time + DelayIn;
			EmBox.CallLater( DoNext, DelayIn );
			break;
		case States.ExecutingOut:
			cursor--;
			if( cursor < 0 ){
				cursor = -1;
				ExecuteOutComplete();
				return;
			}
			list[ cursor ].ExecuteOut();
//          explode = Time.time + DelayOut;
			EmBox.CallLater( DoNext, DelayOut );
			break;
		}
	}

//    void Update() {
//      Debug.Log("explode <= Time.time"+(explode <= Time.time).ToString());
//          if( explode <= Time.time ) DoNext();
//    }

}
}

