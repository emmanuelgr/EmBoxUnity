using System;
using EmBoxUnity.Commands.Core;
using EmBoxUnity.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EmBoxUnity.Commands{
public class CInterpolate:BaseCommand{
		
	private float interpolated ,ratio ,timer;
	public float fromIn ,toIn ,fromOut ,toOut ,timeIn ,timeOut;
	public delegate float Fn( float start, float end, float val );

	public Fn inFn ,outFn;

	public CInterpolate( float fromIn, float pose, float toOut, float timeIn, float timeOut, Fn inFn=null, Fn outFn=null ): base( ){
		this.fromIn = interpolated = fromIn;
		this.toIn = pose;
		this.fromOut = pose;
		this.toOut = toOut;
		this.timeIn = timeIn;
		this.timeOut = timeOut;
		this.inFn = inFn == null ? Interpolate.easeInOutCubic : inFn;
		this.outFn = outFn == null ? Interpolate.easeInOutCubic : outFn;
		timer = 0f;
	}

	public CInterpolate( float fromIn, float toIn, float fromOut, float toOut, float timeIn, float timeOut, Fn inFn=null, Fn outFn=null ): base( ){
		this.fromIn = interpolated = fromIn;
		this.toIn = toIn;
		this.fromOut = fromOut;
		this.toOut = toOut;
		this.timeIn = timeIn;
		this.timeOut = timeOut;
		this.inFn = inFn == null ? Interpolate.easeInOutCubic : inFn;
		this.outFn = outFn == null ? Interpolate.easeInOutCubic : outFn;
		timer = 0f;
	}

	protected override void DoIn(){
		EmBox.UPDATE.Add( update );
	}

	protected override void CancelIn(){
		EmBox.UPDATE.Remove( update );
		timer = timeOut * ( 1 - ratio );
	}

	protected override void DoOut(){
		EmBox.UPDATE.Add( update );
	}

	protected override void CancelOut(){
		EmBox.UPDATE.Remove( update );
		timer = timeIn * ( 1 - ratio );
	}

	private void update(){
		timer += Time.deltaTime;
		switch( State ){
		case States.ExecutingIn:
			ratio = timer / timeIn;
			ratio = ratio > 1f ? 1f : ratio;
			interpolated = inFn( fromIn, toIn, ratio );
			if( ratio == 1f ){
				timer = 0f;
				ExecuteInComplete();
			}
			break;
		case States.ExecutingOut:
			ratio = timer / timeOut;
			ratio = ratio > 1f ? 1f : ratio;
			interpolated = outFn( fromOut, toOut, ratio );
			if( ratio == 1f ){
				timer = 0f;
				ExecuteOutComplete();
			}
			break;
		case States.ExecuteInComplete:
		case States.ExecuteOutComplete:
			EmBox.UPDATE.Remove( update );
			timer = 0f;
			break;
		}
	}
		
	public float Value{
		get{
			return interpolated;
		}
	}
}
}

