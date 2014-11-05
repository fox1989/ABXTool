using UnityEngine;
using System.Collections;
using System.IO;
using System;


public class CheckVersion : MonoBehaviour {
	public delegate void EventHandler(bool same);
	public event EventHandler OnCheckVersionFinish;

	LocalVersionLoader localVersionLoader;
	ServerVersionLoader serverVersionLoader;
	// Use this for initialization
	void Start () {
		StartServerVersionLoad ();
	}
	
	void StartServerVersionLoad(){
		serverVersionLoader = gameObject.AddComponent<ServerVersionLoader> ();
		serverVersionLoader.OnLoadFinishHandler += OnServerVersionLoadFinish;
	}
	
	void OnServerVersionLoadFinish(VersionLoader loader){
		serverVersionLoader.OnLoadFinishHandler -= OnServerVersionLoadFinish;
		StartLocalVersionLoad ();
	}
	
	void StartLocalVersionLoad(){
		localVersionLoader = gameObject.AddComponent<LocalVersionLoader> ();
		localVersionLoader.OnLoadFinishHandler += OnLocalVersionLoadFinish;
	}
	
	void OnLocalVersionLoadFinish(VersionLoader loader){
		localVersionLoader.OnLoadFinishHandler -= OnLocalVersionLoadFinish;
		bool same = localVersionLoader.version.Equals(serverVersionLoader.version);
		if (OnCheckVersionFinish != null) {
			OnCheckVersionFinish(same);		
		}
	}
}
