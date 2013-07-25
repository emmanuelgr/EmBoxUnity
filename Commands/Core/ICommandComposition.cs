using System;
using EmBoxUnity.Commands;

namespace EmBoxUnity.Commands.Core{
public interface ICommandComposition:ICommand{
	void Add( ICommand cmnd );
}
}

