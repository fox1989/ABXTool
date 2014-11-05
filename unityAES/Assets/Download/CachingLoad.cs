using System;
using UnityEngine;
using System.Collections;

public class CachingLoad : MonoBehaviour {

	public delegate void EventHandler(GameObject go);
	public event EventHandler OnLoadFinish;

	public string BundleURL;
	public string AssetName;
	public int version;

	public bool isOnlyLoadToCache=false;
	
	void Start() {
		StartCoroutine (DownloadAndCache());
	}
	
	IEnumerator DownloadAndCache (){
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;
		
		// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		using(WWW www = WWW.LoadFromCacheOrDownload (BundleURL, version)){
			yield return www;
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
			AssetBundle bundle = www.assetBundle;
			GameObject go=null;

			if(!isOnlyLoadToCache){
			
				if (AssetName == ""){
				go=Instantiate(bundle.mainAsset) as GameObject;
				}else{
					go=Instantiate(bundle.Load(AssetName)) as GameObject;
				}
			}
			
			 bundle.Unload(false);

			if(OnLoadFinish!=null){
				OnLoadFinish(go);
				}
			

			Destroy(this);
		}
	}
}