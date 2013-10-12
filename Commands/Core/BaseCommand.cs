//case States.ExecuteError:
//case States.ExecuteNone:
//case States.ExecutingIn:
//case States.ExecuteInComplete:
//case States.ExecutingOut:
//case States.ExecuteOutComplete:

// NOTES-----------------------------------------------------
// Need to check cycle case of cancelable!!!!!!!!!!!
//---------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;

namespace EmBoxUnity.Commands.Core{
public class BaseCommand:ICommand{
		
	private int guid;
	private List<int> guidsIn = new List< int>();
	private List<Action< int>> actionsIn = new List< Action<int>>();
	private List<int> guidsOut = new List< int>();
	private List<Action<int>> actionsOut = new List< Action<int>>();
	private static int anInt;
		
	public int GUID { get { return guid; } }

	public States State { get; set; }

	public BaseCommand(){
		guid = EmBox.GUID;
		State = States.ExecuteNone;
	}

	public virtual void ExecuteIn(){
		switch( State ){
		case States.ExecutingIn:
				// busy... already executing In
			break;
		case States.ExecuteInComplete:
//			// all done but trigger fn() call again
				 ExecuteInComplete();
			break;
		case States.ExecutingOut:
			CancelOut();
			seqIn();
			break;
		case States.ExecuteError:
		case States.ExecuteNone:
		case States.ExecuteOutComplete:
			seqIn();
			break;
		}
	}

	private void seqIn(){
		State = States.ExecutingIn;
		PreExecuteIn();
		DoIn();
	}

	public virtual void ExecuteOut(){
		switch( State ){
		case	 States.ExecutingOut:
			// busy... already executing Out
			break;
		case	 States.ExecuteOutComplete:
// all done but trigger fn() call again
				ExecuteOutComplete();
			break;
		case	 States.ExecuteNone:
			 // Will need to do an in before ... 
			break;
		case States.ExecutingIn:
			CancelIn();
			seqOut();
			break;
		case States.ExecuteError:
		case States.ExecuteInComplete:
			seqOut();
			break;
		}
	}

	private void seqOut(){
		State = States.ExecutingOut;
		PreExecuteOut();
		DoOut();
	}
	/// <summary>
	/// Allways call this Fn when DoIn has finished
	/// </summary>
	protected void ExecuteInComplete(){
		State = States.ExecuteInComplete;
		PostExecuteIn();
		for( int i = 0; i < actionsIn.Count; i++ ){
			actionsIn[ i ]( guidsIn[ i ] );
		}
	}

	/// <summary>
	/// Allways call this Fn when DoOut has finished
	/// </summary>
	protected void ExecuteOutComplete(){
		State = States.ExecuteOutComplete;
		PostExecuteOut();
		for( int i = 0; i < actionsOut.Count; i++ ){
			actionsOut[ i ]( guidsOut[ i ] );
		}
	}
	/// <summary>
	/// Triggered before DoIn()
	/// </summary>
	protected virtual void PreExecuteIn(){
	}
	/// <summary>
	///  Override and call ExecuteInComplete() when done
	/// </summary>
	protected virtual void DoIn(){
	}

	protected virtual void CancelIn(){
	}
	/// <summary>
	/// Triggered after ExecuteInComplete()
	/// </summary>
	protected virtual void PostExecuteIn(){
	}


	/// <summary>
	/// Triggered before DoOut()
	/// </summary>
	protected virtual void PreExecuteOut(){
	}
	/// <summary>
	/// Override and call ExecuteOutComplete() when done
	/// </summary>
	protected virtual void DoOut(){
	}

	protected virtual void CancelOut(){
	}
	/// <summary>
	/// Triggered after ExecuteOutComplete()
	/// </summary>
	protected virtual void PostExecuteOut(){
	}

	public void Toggle(){
		switch( State ){
		case States.ExecutingIn:
		case States.ExecuteInComplete:
			ExecuteOut();
			break;
		case States.ExecutingOut:
		case States.ExecuteOutComplete:
		case States.ExecuteNone:
			ExecuteIn();
			break;
		}
	}

	public void AddOnInComplete( Action<int> act, int guid ){
		anInt = guidsIn.IndexOf( guid );
		if( anInt >= 0 ){
		} else{
			guidsIn.Add( guid );
			actionsIn.Add( act );
		}
	}

	public void RemOnInComplete( int guid ){
		anInt = guidsIn.IndexOf( guid );
		if( anInt >= 0 ){
			guidsIn.RemoveAt( anInt );
			actionsIn.RemoveAt( anInt );
		}
	}

	public void AddOnOutComplete( Action<int> act, int guid ){
		anInt = guidsOut.IndexOf( guid );
		if( anInt >= 0 ){
		} else{
			guidsOut.Add( guid );
			actionsOut.Add( act );
		}
	}

	public void RemOnOutComplete( int guid ){
		anInt = guidsIn.IndexOf( guid );
		if( anInt >= 0 ){
			guidsOut.RemoveAt( anInt );
			actionsOut.RemoveAt( anInt );
		}
	}

}
}