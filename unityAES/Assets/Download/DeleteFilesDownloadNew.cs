using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DeleteFilesDownloadNew : MonoBehaviour {
	public delegate void EventHandler(System.Object obj);
	public event EventHandler OnDownloadAllFinish;


	ServerVersionLoader serverVersionLoader;

	int finished=0;
	// Use this for initialization
	void Start () {
			DirectoryInfo dirInfo = new DirectoryInfo (Application.persistentDataPath);  
			FileInfo[] files = dirInfo.GetFiles ();  
			foreach (FileInfo file in files) {
						file.Delete ();
				}		
			StartServerVersionLoad();
	}

	void StartServerVersionLoad(){
		serverVersionLoader = gameObject.AddComponent<ServerVersionLoader> ();
		serverVersionLoader.OnLoadFinishHandler += OnServerVersionLoadFinish;
	}
	
	void OnServerVersionLoadFinish(VersionLoader loader){
		serverVersionLoader.OnLoadFinishHandler -= OnServerVersionLoadFinish;
		StartDownloadBunle ();
	}

	void StartDownloadBunle(){
		for (int i=0; i<serverVersionLoader.files.Count; i++) {
			DownloadBundle downloadBundle = gameObject.AddComponent<DownloadBundle> ();
			string fileName = "";
			serverVersionLoader.files.TryGetValue(i,out fileName);
			downloadBundle.bundleName = fileName+".unity3d";
			downloadBundle.OnDownLoadFinish += this.OnDownLoadFinish;	
		}


	}

	void OnDownLoadFinish(DownloadBundle loader){
		loader.OnDownLoadFinish -= this.OnDownLoadFinish;
		finished++;
		if (finished== serverVersionLoader.files.Count) {
			serverVersionLoader.xmlDoc.Save(Application.persistentDataPath+"/AssetXML.xml");
			if(OnDownloadAllFinish!=null){
				OnDownloadAllFinish(null);
			}
			Destroy(this);
		}
	}
}
