using System;
using UnityEngine;
using EmBoxUnity.Commands.Core;

namespace EmBoxUnity.Commands{
public class CBool:BaseCommand{
		private bool _value;
		public  bool Value{
			get { return _value;}
		}

	public CBool(  bool initValue=false ): base( ){
		_value = initValue;
	}

	protected override void DoIn(){
		_value = true;
		ExecuteInComplete();
	}

	protected override void DoOut(){
		_value = false;
		ExecuteOutComplete();
	}
}
}

