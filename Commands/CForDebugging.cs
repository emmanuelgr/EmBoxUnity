using System;
using System.Collections.Generic;
using EmBoxUnity.Commands.Core;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CForDebugging:CSerial{
		
		public override void ExecuteIn(){
			Debug.Log("iiiiin");
			base.ExecuteIn();
		}
		
		public override void ExecuteOut(){
			Debug.Log("out");
			base.ExecuteOut();
		}
}
}