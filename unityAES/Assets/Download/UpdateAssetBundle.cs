using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class UpdateAssetBundle : MonoBehaviour {
	public delegate void EventHandler(System.Object obj);
	public event EventHandler OnUpdateAssetBundleFinish;


	DeleteFilesDownloadNew dfdn;
	CheckVersion chkv;
	// Use this for initialization
	void Start () {
	//	print (Application.persistentDataPath);
	//	//return;

        print(Application.persistentDataPath);
		if (!File.Exists (Application.persistentDataPath + "/AssetXML.xml")) {
			StartDeleteFilesDownloadNew();
			} else {
			StartCheckVersion();
		}

	}


	void StartDeleteFilesDownloadNew(){
		dfdn=gameObject.AddComponent<DeleteFilesDownloadNew>();
		dfdn.OnDownloadAllFinish+=OnDownloadAllFinish;
	}

	void OnDownloadAllFinish(System.Object obj){
		dfdn.OnDownloadAllFinish-=OnDownloadAllFinish;
		NoticeUpdateOver();	
	}


	void StartCheckVersion(){
		chkv=gameObject.AddComponent<CheckVersion>();
		chkv.OnCheckVersionFinish+=OnCheckVersionOver;
	}

	void OnCheckVersionOver(bool same){
		chkv.OnCheckVersionFinish-=OnCheckVersionOver;
		if (!same) {
		StartDeleteFilesDownloadNew ();	
			} else {
			NoticeUpdateOver();	
		}
	}

	void NoticeUpdateOver(){
		if (OnUpdateAssetBundleFinish != null) {
			OnUpdateAssetBundleFinish(null);		
		}
	}
}
