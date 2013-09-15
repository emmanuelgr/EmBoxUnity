using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CSerial:BaseCommand, ICommandComposition{
	private List<ICommand> list;
	private int cursor = 0;

	private void init(){
		list = new List<ICommand>();
	}

	public CSerial()
      : base( ){
		init();
	}

	public CSerial( params ICommand[] cmnds ): base( ){
		init();
		list.AddRange( cmnds );
		for( int i = 0; i < cmnds.Length; i++ ){
//			( cmnds[ i ] as ICommand ).FnInComplete = subInCom;
//			( cmnds[ i ] as ICommand ).FnOutComplete = subOutCom;
		}
	}

	public void Add( ICommand cmnd ){
		list.Add( cmnd );
//		cmnd.FnInComplete = subInCom;
//		cmnd.FnOutComplete = subOutCom;
	}

	protected override void DoIn(){
		if( list.Count == 0 ){
			ExecuteInComplete();
			return;
		}
		list[ cursor ].FnInComplete = subInCom;
		list[ cursor ].ExecuteIn();
	}

		protected override void CancelIn(){
		list[ cursor ].FnInComplete = null;
		list[ cursor ].FnOutComplete = subOutCom;
		list[ cursor ].ExecuteOut();
	}

	protected override void DoOut(){
		if( list.Count == 0 ){
			ExecuteOutComplete();
			return;
		}
		list[ cursor ].FnOutComplete = subOutCom;
		list[ cursor ].ExecuteOut();
	}

		protected override void CancelOut(){
		list[ cursor ].FnOutComplete = null;
		list[ cursor ].FnInComplete = subInCom;
		list[ cursor ].ExecuteIn();
	}

	private void subInCom(){
		list[ cursor ].FnInComplete = null;
		if( cursor == list.Count - 1 ){
			ExecuteInComplete();
		} else{
			cursor++;
			list[ cursor ].FnInComplete = subInCom;
			list[ cursor ].ExecuteIn();
		}
	}

	private void subOutCom(){
		list[ cursor ].FnOutComplete = null;
		if( cursor == 0 ){
			ExecuteOutComplete();
		} else{
			cursor--;
			list[ cursor ].FnOutComplete = subOutCom;
			list[ cursor ].ExecuteOut();
		}
	}
}
}

