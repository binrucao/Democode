// use to calculate the coordinates translate from each other
// gps -> unity
// unity -> gps

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateCoordinates : MonoBehaviour {
	public static TranslateCoordinates _trans;
	float unityOrig_x, unityOrig_z;
	float gpsOrig_lat, gpsOrig_lon;
	float unityNew_x, unityNew_z;
	float gpsNew_lat, gpsNew_lon;

	float rotationMatrix11, rotationMatrix12, rotationMatrix21, rotationMatrix22;

	static float constant, scale;

	Vector3 UnityLocation;
	Vector2 GPSLocation;


	// Use this for initialization
	void Start () {
		initialTranslate ();
	}

	void initialTranslate()
	{
		unityOrig_x = 1482.4f;
		unityOrig_z = 191f;

		gpsOrig_lat = 42.3658521162533f;
		gpsOrig_lon = -71.0608604550362f;

		rotationMatrix11 = Mathf.Cos (125 * Mathf.Deg2Rad);
		rotationMatrix12 = Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix21 = - Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix22 = Mathf.Cos (125 * Mathf.Deg2Rad);

		constant = Mathf.Cos (gpsOrig_lat * Mathf.Deg2Rad);
		scale = 7.658f;
	}

	public Vector3 getUnityLocation(float lat, float lon, float depth)
	{

		unityOrig_x = 1482.4f;
		unityOrig_z = 191f;

		gpsOrig_lat = 42.3658521162533f;
		gpsOrig_lon = -71.0608604550362f;

		rotationMatrix11 = Mathf.Cos (125 * Mathf.Deg2Rad);
		rotationMatrix12 = Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix21 = - Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix22 = Mathf.Cos (125 * Mathf.Deg2Rad);

		constant = Mathf.Cos (gpsOrig_lat * Mathf.Deg2Rad);
		scale = 7.658f;

		var dis_lat = (lat - gpsOrig_lat) * 111300;
		var dis_lon = (lon - gpsOrig_lon) * 111300 * constant;
//		Debug.Log ("The distance of lat and lon are: " + dis_lat + "    " + dis_lon);

		var dis_x = rotationMatrix11 * dis_lon + rotationMatrix12 * dis_lat;
		var dis_z = rotationMatrix21 * dis_lon + rotationMatrix22 * dis_lat;
//		Debug.Log ("The distance of x and z are: " + dis_x + "    " + dis_z);

		unityNew_x = unityOrig_x + dis_x * scale;
		unityNew_z = unityOrig_z + dis_z * scale;
		Debug.Log ("The NEW coordinates of x and z are: " + unityNew_x + "    " + unityNew_z);

		UnityLocation = new Vector3 (unityNew_x, depth, unityNew_z);

		return UnityLocation;

	}

	public Vector2 getGPSLocation(float x, float z)
	{

		unityOrig_x = 1482.4f;
		unityOrig_z = 191f;

		gpsOrig_lat = 42.3658521162533f;
		gpsOrig_lon = -71.0608604550362f;

		rotationMatrix11 = Mathf.Cos (125 * Mathf.Deg2Rad);
		rotationMatrix12 = Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix21 = - Mathf.Sin (125 * Mathf.Deg2Rad);
		rotationMatrix22 = Mathf.Cos (125 * Mathf.Deg2Rad);

		constant = Mathf.Cos (gpsOrig_lat * Mathf.Deg2Rad);
		scale = 7.658f;

		var dis_x = (x - unityOrig_x) / scale;
		var dis_z = (z - unityOrig_z) / scale;
//		Debug.Log ("The distance of x and z are: " + dis_x + "    " + dis_z);

		var dis_lon = rotationMatrix11 * dis_x + rotationMatrix21 * dis_z;
		var dis_lat = rotationMatrix12 * dis_x + rotationMatrix22 * dis_z;
//		Debug.Log ("The distance of lat and lon are: " + dis_lat + "    " + dis_lon);

		gpsNew_lat = gpsOrig_lat + dis_lat / 111300;
		gpsNew_lon = gpsOrig_lon + dis_lon / 111300 / constant;
		Debug.Log ("The NEW coordinates of lat and lon are: " + gpsNew_lat + "    " + gpsNew_lon);

		GPSLocation = new Vector2 (gpsNew_lat, gpsNew_lon);
		return GPSLocation;

	}
		
}
