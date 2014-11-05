using UnityEngine;
using System.Collections;

public class LocalVersionLoader : VersionLoader {


	void Start () {
		path = "file:///"+Application.persistentDataPath;
        print(path);
     //   path =  Application.persistentDataPath;
		base.Start ();
	}
	

}
