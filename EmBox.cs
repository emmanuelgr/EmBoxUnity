using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using EmBoxUnity.Commands.Core;
using EmBoxUnity.Utils;

public class EmBox:MonoBehaviour{
	
	public enum Layers{
		InYourFace = 1,
		Front = 10,
		Middle = 20,
		Background = 30
	};
	
	/// <summary>
	/// The ScaleFactor.
	/// </summary>
	public static float SF = 1;
	public static float minSF = 0.25f;
	public static float maxSF = 1.25f;
	public static float DPIRatio;
	public static float RealEstateRatio;
	public static bool ShowMesseges = false;
	public static bool ScreenChanged = true;
	private static float WRatio;
	private static float HRatio;
	private static float designWidth;
	private static float designHeight;
	private static int lastWidth;
	private static int lastHeight;
	public static List<Action> UPDATE = new List<Action>();
	private static GameObject scripts;
	private static GameObject cam;
	private static GUIStyle gUIStyleDefault;
	private static int guid = 1;
	private static int guid2 = 1;
	// Callater
	private static List<int> guids = new List< int>();
	private static List<Action> actions = new List< Action>();
	private static List<float> times = new List< float>();
	private static int anInt;
	private static Action anAction;
	
	/// <summary>
	/// Gets an global unique identifier.
	/// </summary>
	public static int GUID{
		get{ return guid++; 	}
	}	
	/// <summary>
	/// Gets an global unique identifier.
	/// </summary>
	public static int GUID2{
		get{ return guid2++; 	}
	}
	
	// Message sysytem
	private static string debugMsg = "";
	private Vector2 scrollPosition;
	private static Constraint scrollerRect = new Constraint( "0.5", "0.5", Ninegrid.Types.MiddleLeft, Ninegrid.Types.MiddleLeft, 0, 0 );
	
	public void Init( float designWidth, float designHeight, float dpi=260 ){
		Debug.Log( "<<<<<<<<<<--------EMBOX-------->>>>>>>>>>" );
		EmBox.designWidth = designWidth;
		EmBox.designHeight = designHeight;
				
    #if UNITY_EDITOR || UNITY_STANDALONE
        DPIRatio = 72f/dpi;
		#else
		DPIRatio = Screen.dpi / dpi;
		#endif
	}
	
	void Update(){
		#region  Screen change detection
		if( Screen.width != lastWidth || Screen.height != lastHeight ){
			ScreenChanged = true;
			lastWidth = Screen.width;
			lastHeight = Screen.height;
			calcSF();
		} else{
			ScreenChanged = false;
		}
		#endregion
		
		#region fns to call every Update
//		Debug.Log( "l: " + EmBox.UPDATE.Count );
		for( int i = 0; i < UPDATE.Count; i++ ){
			UPDATE[ i ]();
		}
		#endregion
	
		#region CallLater
		for( int k=times.Count - 1; k > -1; k-- ){
			if( times[ k ] <= Time.time ){
				anAction = actions[ k ];
				guids.RemoveAt( k );
				times.RemoveAt( k );
				actions.RemoveAt( k );
				anAction();
			}
		}
		#endregion
	}
	
	
	private void calcSF(){
		WRatio = (float)Screen.width / ( Screen.width > Screen.height ? designWidth : designHeight );
		HRatio = (float)Screen.height / ( Screen.width > Screen.height ? designHeight : designWidth );
		RealEstateRatio = Math.Min( WRatio, HRatio );
//		RealEstateRatio = ( WRatio + HRatio )/2;
		SF = Math.Min( RealEstateRatio, DPIRatio );
		SF = Mathf.Clamp( SF, minSF, maxSF );
	}
	
	public static void CallLaterCancel( int guid ){
		anInt = guids.IndexOf( guid );
		if( anInt >= 0 ){
			guids.RemoveAt( anInt );
			times.RemoveAt( anInt );
			actions.RemoveAt( anInt );
		}
	}

	public static void CallLater( Action act, float time, int guid ){
		anInt = guids.IndexOf( guid );
		if( anInt >= 0 ){
			times[ anInt ] = time + Time.time;
		} else{
			guids.Add( guid );
			actions.Add( act );
			times.Add( time + Time.time );
		}
	}

	public static GameObject Cam{
		get{
			if( cam == null ){
				cam = GameObject.Find( "Main Camera" );
				if( cam == null ){
					Debug.LogError( "cant find camera with name:Main Camera" );
				}
			}
			return cam;
		}
	}
	
	public static GameObject Scripts{
		get{
			if( scripts == null ){
				scripts = GameObject.Find( "Scripts" );
				if( scripts == null ){
					scripts = new GameObject( "Scripts" );
				}
			}
			return scripts;
		}
	}
	
	public static GUIStyle GUIStyleDefault{
		get{
			if( gUIStyleDefault == null ){
//				gUIStyleDefault = new GUIStyle();
				gUIStyleDefault = GUIStyle.none;
				
			}
			return gUIStyleDefault;
		}
	}
	
	private static GUIStyle debugGUIStyle;

	public  static GUIStyle DebugGUIStyle{
		get{
			if( debugGUIStyle == null ){
				debugGUIStyle = new GUIStyle();
				debugGUIStyle.normal.textColor = new Color( 0.75f, 0.75f, 0.75f ); 
				Texture2D txtr = new Texture2D( 1, 1 );
				Color32[] clrs = new Color32[1];
				clrs[ 0 ] = new Color32( 0, 0, 0, (int)( 255 * 0.75 ) );
				txtr.SetPixels32( clrs );
				txtr.Apply( false );
				debugGUIStyle.normal.background = txtr;
				debugGUIStyle.fontSize = (int)( 6f/72f * Screen.dpi);
			}
			return debugGUIStyle;
		}
	}
	
	public static void ClearMessages(){
		debugMsg = "";
	}

	public static void Message( string msg ){
		debugMsg += msg + "\n";
	}

	void OnGUI(){
		if( !ShowMesseges )
			return;
		Rect contentRect = new Rect( 0, 0, Screen.width * 0.5f, debugMsg.Split( '\n' ).Length * DebugGUIStyle.lineHeight );
		scrollPosition = GUI.BeginScrollView( scrollerRect.Rect, scrollPosition, contentRect );
//		GUI.TextArea( contentRect, debugMsg, DebugGUIStyle );
		GUI.Label( contentRect, debugMsg, DebugGUIStyle );
		GUI.EndScrollView();
//		Debug.Log( debugMsg);
	}
}


