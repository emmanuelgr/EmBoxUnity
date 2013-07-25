using System;

namespace EmBoxUnity.Commands.Core{
public interface ICommand{
	void ExecuteIn();

	void ExecuteOut();

	void Toggle();
 
	Action FnInComplete{ get; set; }

	Action FnOutComplete{ get; set; }

	States State{ get; set; }

	bool Cancelable { get; set; }
  
}
}

