using UnityEngine;
using System.Collections;

public class ServerVersionLoader : VersionLoader {

	// Use this for initialization
	void Start () {
		path ="http://192.168.50.43:8888/apacheShare/file/unity";
		base.Start ();
	}

}
