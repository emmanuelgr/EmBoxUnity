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
	
#if UNITY_ANDROID
	private static AndroidJavaObject activity;
	private static AndroidJavaObject windowManager;
	private static AndroidJavaObject display;
	private static AndroidJavaObject configuration;
	private static AndroidJavaObject resourcesAndroid;
	public enum AndroidRotations{
		ROTATION_0 = 0x00000000,
		ROTATION_90 = 0x00000001,
		ROTATION_180 = 0x00000002,
		ROTATION_270 = 0x00000003
	}
	public enum AndroidOrientations{
		ORIENTATION_UNDEFINED = 0x00000000,
		ORIENTATION_PORTRAIT = 0x00000001,
		ORIENTATION_LANDSCAPE = 0x00000002,
		ORIENTATION_SQUARE = 0x00000003
	}
	public static AndroidJavaObject Activity{
		get{
			if (activity == null) {
				activity = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ).GetStatic<AndroidJavaObject>( "currentActivity" );
			}
			return activity;
		}
	}
	public static AndroidJavaObject WindowManager{
		get{
			if (windowManager == null) {
				windowManager = Activity.Call<AndroidJavaObject>("getSystemService","window");
			}
			return windowManager;
		}
	}
	public static AndroidJavaObject Display{
		get{
			if (display == null) {
				display = WindowManager.Call<AndroidJavaObject>("getDefaultDisplay");
			}
			return display;
		}
	}
	public static AndroidRotations AndroidRotation{
		get{
			return (AndroidRotations)Display.Call<int>("getRotation");
		}
	}
	public static AndroidJavaObject ResourcesAndroid{
		get{
			if (resourcesAndroid == null) {
				resourcesAndroid = Activity.Call<AndroidJavaObject>("getResources");
			}
			return resourcesAndroid;
		}
	}
	public static AndroidJavaObject Configuration{
		get{
			if (configuration == null) {
				configuration = ResourcesAndroid.Call<AndroidJavaObject>("getConfiguration");
			}
			return configuration;
		}
	}
	public static AndroidOrientations AndroidOrientation{
		get{
			return (AndroidOrientations)Configuration.Get<int>("orientation");
		}
	}
#endif
	/// <summary>
	/// The ScaleFactor.
	/// </summary>
	public static float SF =1;
	public static float minSF =0.5f;
	public static float maxSF =1.25f;
	public static float DPIRatio;
	public static float RealEstateRatio;
	public static bool ShowMesseges = false;
	private static float WRatio;
	private static float HRatio;
	private static float designWidth;
	private static float designHeight;
	private static AMono aMono;
	public static List<Action> UPDATE = new List<Action>();
	// Message sysytem
	private static string debugMsg = "";
	private Vector2 scrollPosition;
	private static Constraint scrollerRect = new Constraint( "0.3", "0.3", Ninegrid.Types.MiddleLeft, Ninegrid.Types.MiddleLeft, 0, 0 );
	
	public void Init( float designWidth, float designHeight, float dpi=260 ){
		Debug.Log( "<<<<<<<<<<--------EMBOX-------->>>>>>>>>>" );
		EmBox.designWidth = designWidth;
		EmBox.designHeight = designHeight;
				
		aMono = Camera.mainCamera.gameObject.AddComponent<AMono>();

    #if UNITY_EDITOR || UNITY_STANDALONE
        DPIRatio = 72f/dpi;
		#else
			DPIRatio = Screen.dpi/dpi;
		#endif
	}
	void Update(){
		
		WRatio = (float)Screen.width / ( Screen.width > Screen.height ? designWidth : designHeight );
		HRatio = (float)Screen.height / ( Screen.width > Screen.height ? designHeight : designWidth );
		RealEstateRatio = Math.Min( WRatio, HRatio );
//		RealEstateRatio = ( WRatio + HRatio )/2;
		SF = Math.Min( RealEstateRatio, DPIRatio );
		SF = Mathf.Clamp( SF, minSF, maxSF);
		
//		Debug.Log( "l: " + EmBox.UPDATE.Count );
		for (int i = 0; i < UPDATE.Count; i++) {
			UPDATE[i]();
		}
	}	
	public static void CallLater( Action act, float time ){
		aMono.CallLater( act, time );
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
				debugGUIStyle.fontSize = (int)( 30 * EmBox.SF );
			}
			return debugGUIStyle;
		}
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
public class AMono : MonoBehaviour{

	public void CallLater( Action act, float time ){
		StartCoroutine( calllaterCoRoutine( act, time ) );
	}

	IEnumerator calllaterCoRoutine( Action act, float time ){
		yield return new WaitForSeconds(time);
		act();
	}
}