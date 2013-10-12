using System;

namespace EmBoxUnity.Commands.Core{
public interface ICommand{
	int GUID { get; }

	void ExecuteIn();

	void ExecuteOut();

	void Toggle();
 
	void AddOnInComplete( Action<int> act, int guid );

	void RemOnInComplete( int guid );

	void AddOnOutComplete( Action<int> act, int guid );

	void RemOnOutComplete( int guid );

	States State{ get; set; }

}
}

