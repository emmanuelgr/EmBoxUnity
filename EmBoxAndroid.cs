using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using EmBoxUnity.Commands.Core;
using EmBoxUnity.Utils;

public class EmBoxAndroid:MonoBehaviour{
	
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
}


