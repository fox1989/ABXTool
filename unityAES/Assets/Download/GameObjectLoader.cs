using System;
using UnityEngine;
using System.Collections;

public class GameObjectLoader : MonoBehaviour {
	
	public delegate void EventHandler(GameObject go,GameObjectLoader loader);
	public event EventHandler OnLoadFinish;

	public string fileName;
	GameObject go;

	
	void Start() {
		go=null;
		LoadPrefabAsGameObject(ref go,fileName);
		if(go!=null){
			if (OnLoadFinish != null) {
				OnLoadFinish (go,this);
			}
			Destroy (this);
			return;
		}else{
			StartCoroutine (Download());
		}
	}
	
	IEnumerator Download (){
	
				WWW www = new WWW ("file://"+Application.persistentDataPath + "/" + fileName+".unity3d");
				yield return www;
			

				if (www.isDone) {
						AssetBundle bundle = www.assetBundle;
						go = Instantiate (bundle.mainAsset) as GameObject;
			            go.name=fileName;
						go.SetActive (false);
						bundle.Unload (false);
			
						if (OnLoadFinish != null) {
								OnLoadFinish (go,this);
						}
			
			
						Destroy (this);
				}
		}

	public static void LoadPrefabAsGameObject(ref GameObject myGameObject,string prefabName){
		string resPath="Prefab/"+prefabName;
		System.Object obj = Resources.Load (resPath);
		if (obj != null) {
			GameObject tempObj = obj as GameObject;
			myGameObject = (GameObject)Instantiate (tempObj);
			myGameObject.name = prefabName;
			myGameObject.SetActive (false);
		}
	}
}
