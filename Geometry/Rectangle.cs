using System;
using UnityEngine;

namespace EmBoxUnity.Geometry{
public class Rectangle{
	public float x ,y ,width ,height;

	public Rectangle(){
		x = 0;
		y = 0;
		width = 0;
		height = 0;
	}

	public Rectangle( float x, float y, float width, float height ){
		x = x;
		y = y;
		width = width;
		height = height;
	}

	public Rectangle( Rect rect ){
		x = rect.x;
		y = rect.y;
		width = rect.width;
		height = rect.height;
	}
		
	public Rectangle SetValues( Rect rect ){
		x = rect.x;
		y = rect.y;
		width = rect.width;
		height = rect.height;
		return this;
	}
		
	public Rect Rect{
		get{
			return new Rect( x, y, width, height );
		}
	}
		
	public override string ToString(){
		return "X: " + x + " Y: " + y + " Width: " + width + " Height: " + height ; 
	}
}
}


