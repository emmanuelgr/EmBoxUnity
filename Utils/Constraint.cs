using System;
using UnityEngine;
using EmBoxUnity.Geometry;

namespace EmBoxUnity.Utils{
public class Constraint{
	public float OffsetX = 0f ,OffsetY = 0f ,Scale = 1f;
	public Rectangle ParentRectangle;
	public object widthObj ,heightObj ,nineGridOffsetXObj ,nineGridOffsetYObj;
	private Ninegrid.Types textureNgrid ,referenceNgrid;
	private Point textureUV ,referenceUV;
	private bool scaleHor = true , scaleVer = true;
	private Rectangle rectangle;
	private float aFloat;
		
	/// <summary>
	/// Constraints texture`s rect to a ninegrid system within a Command pattern. Need to call update() on unity's onGui() callback
	/// </summary>
	/// <param name="width">The width if 0 will be Screen width.</param>
	/// <param name="height">The height if 0 will be Screen height.</param>
	/// <param name="textureNgrid">Texture ngrid.</param>
	/// <param name="referenceNgrid">Reference ngrid.</param>
	/// <param name="scale">Scale.</param>
	/// <param name="offsetX">Offset x.</param>
	/// <param name="offsetY">Offset y.</param>
	/// <param name="pivotScale">Global Scaling amount</param>
	/// <param name="parentRectangle">References the Screen if not assigned or the passed Rectangle y.</param>
	public Constraint( object width, object height, Ninegrid.Types textureNgrid, Ninegrid.Types referenceNgrid, object nineGridOffsetX, object nineGridOffsetY, bool scaleHor , bool scaleVer, Rectangle parentRectangle ){
		this.widthObj = width;
		this.heightObj = height;
		this.textureNgrid = textureNgrid;
		this.referenceNgrid = referenceNgrid;
		this.nineGridOffsetXObj = nineGridOffsetX;
		this.nineGridOffsetYObj = nineGridOffsetY;
		this.scaleHor = scaleHor;
		this.scaleVer = scaleVer;
		this.ParentRectangle = parentRectangle;
		init();
	}

	public Constraint( object width, object height, Ninegrid.Types textureNgrid, Ninegrid.Types referenceNgrid, object nineGridOffsetX, object nineGridOffsetY, bool scaleHor , bool scaleVer ){
		this.widthObj = width;
		this.heightObj = height;
		this.textureNgrid = textureNgrid;
		this.referenceNgrid = referenceNgrid;
		this.nineGridOffsetXObj = nineGridOffsetX;
		this.nineGridOffsetYObj = nineGridOffsetY;
		this.scaleHor = scaleHor;
		this.scaleVer = scaleVer;
		init();
	}

	public Constraint( object width, object height, Ninegrid.Types textureNgrid, Ninegrid.Types referenceNgrid, object nineGridOffsetX, object nineGridOffsetY ){
		this.widthObj = width;
		this.heightObj = height;
		this.textureNgrid = textureNgrid;
		this.referenceNgrid = referenceNgrid;
		this.nineGridOffsetXObj = nineGridOffsetX;
		this.nineGridOffsetYObj = nineGridOffsetY;
		init();
	}

	public void init(){
		rectangle = new Rectangle();
		textureUV = Ninegrid.regPoint( textureNgrid );
		referenceUV = Ninegrid.regPoint( referenceNgrid );
	}

	private float getFloat( object o ){
		if( o is string ){
			aFloat = float.Parse( (string)o );
		} else if( o is int ){
			aFloat = (float)(int)o;	
		} else if( o is float ){
			aFloat = (float)o;	
		}
		return aFloat;
	}

	public float Width{
		get{
			aFloat = getFloat( widthObj );	
			if( widthObj is string ){
				if( ParentRectangle != null ){
					aFloat = ParentRectangle.width * aFloat;
				} else{
					aFloat = Screen.width * aFloat;
				}
			}
			return aFloat * (scaleHor?EmBox.SF:1f) * Scale;
		}
	}

	public float Height{
		get{
			aFloat = getFloat( heightObj );
			if( heightObj is string ){
				if( ParentRectangle != null ){
					aFloat = ParentRectangle.height * aFloat;
				} else{
					aFloat = Screen.height * aFloat;
				}
			}
			return aFloat * (scaleVer?EmBox.SF:1f) * Scale;
		}
	}

	public float NineGridOffsetX{
		get{
			aFloat = getFloat( nineGridOffsetXObj );	
			if( aFloat == 0 )return 0;
			if( nineGridOffsetXObj is string ){
				if( ParentRectangle != null ){
					aFloat = ParentRectangle.width * aFloat;
				} else{
					aFloat = Screen.width * aFloat;
				}
				return aFloat;
			}
			return aFloat * (scaleHor?EmBox.SF:1f);
		}
	}

	public float NineGridOffsetY{
		get{
			aFloat = getFloat( nineGridOffsetYObj );	
			if( aFloat == 0 )return 0;
			if( nineGridOffsetYObj is string ){
				if( ParentRectangle != null ){
					aFloat = ParentRectangle.height * aFloat;
				} else{
					aFloat = Screen.height * aFloat;
				}
				return aFloat;
			}
			return aFloat * (scaleVer?EmBox.SF:1f);
		}
	}
		
	public Rectangle Rectangle{
		get{
			rectangle.width = Width;
			rectangle.height = Height;
			rectangle.x = rectangle.width * -textureUV.x;
			rectangle.y = rectangle.height * -textureUV.y;
				
			if( ParentRectangle != null ){
				rectangle.x += ParentRectangle.width * referenceUV.x;
				rectangle.y += ParentRectangle.height * referenceUV.y;
				rectangle.x += ParentRectangle.x;
				rectangle.y += ParentRectangle.y;
			} else{
				rectangle.x += Screen.width * referenceUV.x;
				rectangle.y += Screen.height * referenceUV.y;
			}
			rectangle.x += NineGridOffsetX;
			rectangle.y += NineGridOffsetY;
			rectangle.x += OffsetX;
			rectangle.y += OffsetY;
			return rectangle;
		}
	}

	public Rect Rect{
		get{
			return Rectangle.Rect;
		}
	}
		
	public override string ToString(){
		return "X: " + rectangle.x + " Y: " + rectangle.y + " Width: " + rectangle.width + " Height: " + rectangle.height ; 
	}
}
}

