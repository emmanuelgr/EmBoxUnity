using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CSerial:BaseCommand, ICommandComposition{
	private List<ICommand> cmnds;
	private List<int> guids;
	private List<int> guidsInComplete;
	private List<int> guidsOutComplete;
	private int cursor = 0;

	private void init(){
		cmnds = new List<ICommand>();
		guids = new List<int>();
		guidsInComplete = new List<int>();
		guidsOutComplete = new List<int>();
	}

	public CSerial()
      : base( ){
		init();
	}

	public CSerial( params ICommand[] cmnds ): base( ){
		init();
		this.cmnds.AddRange( cmnds );
		for( int i = 0; i < cmnds.Length; i++ ){
			guids.Add( cmnds[ i ].GUID );
			guidsInComplete.Add( EmBox.GUID );
			guidsOutComplete.Add( EmBox.GUID );
		}
	}

	public void Add( ICommand cmnd ){
		cmnds.Add( cmnd );
		guids.Add( cmnd.GUID );
		guidsInComplete.Add( EmBox.GUID );
		guidsOutComplete.Add( EmBox.GUID );
	}

	protected override void DoIn(){
		if( cmnds.Count == 0 ){
			ExecuteInComplete();
			return;
		}
		cmnds[ cursor ].AddOnInComplete( subInCom, guidsInComplete[ cursor ] );
		cmnds[ cursor ].ExecuteIn();
	}

	protected override void CancelIn(){
		cmnds[ cursor ].RemOnInComplete( guidsInComplete[ cursor ] );
	}

	protected override void DoOut(){
		if( cmnds.Count == 0 ){
			ExecuteOutComplete();
			return;
		}
		cmnds[ cursor ].AddOnOutComplete( subOutCom, guidsOutComplete[ cursor ] );
		cmnds[ cursor ].ExecuteOut();
	}

	protected override void CancelOut(){
		cmnds[ cursor ].RemOnOutComplete( guidsOutComplete[ cursor ] );
	}

	private void subInCom( int guid ){
		int index = guidsInComplete.IndexOf( guid );
		cmnds[ index ].RemOnInComplete( guid );
		if( cursor == cmnds.Count - 1 ){
			ExecuteInComplete();
		} else{
			cursor++;
			cmnds[ cursor ].AddOnInComplete( subInCom, guidsInComplete[ cursor ] );
			cmnds[ cursor ].ExecuteIn();
		}
	}

	private void subOutCom( int guid ){
		int index = guidsOutComplete.IndexOf( guid );
		cmnds[ index ].RemOnOutComplete( guid );
		if( cursor == 0 ){
			ExecuteOutComplete();
		} else{
			cursor--;
			cmnds[ cursor ].AddOnOutComplete( subOutCom, guidsOutComplete[ cursor ] );
			cmnds[ cursor ].ExecuteOut();
		}
	}
}
}

