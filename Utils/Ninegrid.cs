using System.Collections;
using UnityEngine;

namespace EmBoxUnity.Utils{
public class Ninegrid{
	public enum Types{
		TopLeft,
		TopMiddle,
		TopRight,
		MiddleLeft,
		MiddleMiddle,
		MiddleRight,
		BottomLeft,
		BottomMiddle,
		BottomRight
	}

	/// <summary>
	/// Returns a Rect that will offset according to texture, screen sizes and desired 9grid location
	/// </summary>
	/// <param name="t">T.</param>
	/// <param name="textureNgrid">Texture ngrid.</param>
	/// <param name="screenNgrid">Screen ngrid.</param>
	/// <param name="scale">Scale ratio in order to accodomodate different dpis</param>
	/// <param name="offsetX">Offset x.</param>
	/// <param name="offsetY">Offset y.</param>
	public static Rect Rect( float w, float h, Types textureNgrid, Types screenNgrid, int offsetX, int offsetY ){
		Point texturePoint = regPoint( textureNgrid );
		Point screenPoint = regPoint( screenNgrid );
		Rect rect = new Rect( 0, 0, w, h );
		rect.x = Screen.width * screenPoint.x;
		rect.y = Screen.height * screenPoint.y;
		rect.x -= w * texturePoint.x;
		rect.y -= h * texturePoint.y;
		rect.x += offsetX;
		rect.y += offsetY;
		return rect;
	}

	public static Point regPoint( Types ng ){
		Point p = new Point();

		switch( ng ){
		case Ninegrid.Types.TopLeft:
		p.x = 0f;
		p.y = 0f;
		break;
		case Ninegrid.Types.TopMiddle:
		p.x = 0.5f;
		p.y = 0f;
		break;
		case Ninegrid.Types.TopRight:
		p.x = 1f;
		p.y = 0f;
		break;
		case Ninegrid.Types.MiddleLeft:
		p.x = 0f;
		p.y = 0.5f;
		break;
		case Ninegrid.Types.MiddleMiddle:
		p.x = 0.5f;
		p.y = 0.5f;
		break;
		case Ninegrid.Types.MiddleRight:
		p.x = 1f;
		p.y = 0.5f;
		break;
		case Ninegrid.Types.BottomLeft:
		p.x = 0f;
		p.y = 1f;
		break;
		case Ninegrid.Types.BottomMiddle:
		p.x = 0.5f;
		p.y = 1f;
		break;
		case Ninegrid.Types.BottomRight:
		p.x = 1f;
		p.y = 1f;
		break;
		}
		return p;
	}

}
}
