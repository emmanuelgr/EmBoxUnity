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
			break;
		case States.ExecuteInComplete:
//			if( FnInComplete != null )
//				FnInComplete(); // all done but trigger fn() call again
			break;
		case States.ExecutingOut:
			if( !Cancelable ){
				needToDoAnIn = true;
			} else{
				CancelOut();
				seqIn();
			}
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

	public void ExecuteOut(){
		switch( State ){
		case	 States.ExecutingOut:
			// busy... already executing Out
			needToDoAnIn = false; // reset value in case there was a request to do an Out
			break;
		case	 States.ExecuteOutComplete:
//			if( FnOutComplete != null )
//				FnOutComplete(); // all done but trigger fn() call again
			break;
		case	 States.ExecuteNone:
			 // Will need to do an in before ... 
			break;
		case States.ExecutingIn:
			if( !Cancelable ){
				needToDoAnOut = true;
			} else{
				CancelIn();
				seqOut();
			}
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
		if( FnInComplete != null )
			FnInComplete();
		if( needToDoAnOut ){
			needToDoAnOut = false;
			ExecuteOut();
		}
	}

	/// <summary>
	/// Allways call this Fn when DoOut has finished
	/// </summary>
	protected void ExecuteOutComplete(){
		State = States.ExecuteOutComplete;
		PostExecuteOut();
		if( FnOutComplete != null )
			FnOutComplete();
		if( needToDoAnIn ){
			needToDoAnIn = false;
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
}
}