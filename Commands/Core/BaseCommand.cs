using System;
using UnityEngine;

namespace EmBoxUnity.Commands.Core{
public class BaseCommand:ICommand{
	public States State { get; set; }

	public Action FnInComplete { get; set; }

	public Action FnOutComplete { get; set; }

	public bool Cancelable { get { return cancelable; } set { cancelable = value; } } // Wrapped it in a prop in order to avoid default bool value of false

	private bool cancelable = true;
	private bool needToDoAnIn = false;
	private bool needToDoAnOut = false;

	public BaseCommand(){
		State = States.ExecuteNone;
	}

	public void ExecuteIn(){
		switch( State ){
		case States.ExecutingIn:
				// busy... already executing In
			needToDoAnOut = false; // reset value in case there was a request to do an Out
			return;
			break;
		case States.ExecuteInComplete:
			if( FnInComplete != null )
				FnInComplete(); // all done but trigger fn() call again
			return;
			break;
		case States.ExecutingOut:
			if( !Cancelable ){
				needToDoAnIn = true;
				return;
			}
			break;
		}
		State = States.ExecutingIn;
		PreExecuteIn();
		DoIn();
	}

	public void ExecuteOut(){
		switch( State ){
		case	 States.ExecutingOut:
			return; // busy... already executing Out
			needToDoAnIn = false; // reset value in case there was a request to do an Out
			break;
		case	 States.ExecuteOutComplete:
			if( FnOutComplete != null )
				FnOutComplete(); // all done but trigger fn() call again
			return;
			break;
		case	 States.ExecuteNone:
			return ; // Will need to do an in before ... 
			break;
		case States.ExecutingIn:
			if( !Cancelable ){
				needToDoAnOut = true;
				return;
			}
			break;
		}
		State = States.ExecutingOut;
		PreExecuteOut();
		DoOut();
	}
		
	/// <summary>
	/// Allways call this Fn when DoIn has finished
	/// </summary>
	protected void ExecuteInComplete(){
		State = States.ExecuteInComplete;
		if( FnInComplete != null )
			FnInComplete();
		PostExecuteIn();
		if( needToDoAnOut ){
			needToDoAnIn = false;
			ExecuteOut();
		}
	}

	/// <summary>
	/// Allways call this Fn when DoOut has finished
	/// </summary>
	protected void ExecuteOutComplete(){
		State = States.ExecuteOutComplete;
		if( FnOutComplete != null )
			FnOutComplete();
		PostExecuteOut();
		if( needToDoAnIn ){
			needToDoAnOut = false;
			ExecuteIn();
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
}
}