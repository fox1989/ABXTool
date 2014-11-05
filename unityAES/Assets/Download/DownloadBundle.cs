using UnityEngine;
using System.Collections;
using System.IO;

public class DownloadBundle : MonoBehaviour {
	public delegate void EventHandler(DownloadBundle loader);
	public event EventHandler OnDownLoadFinish;

	public string bundleName;

	void Start(){
		StartCoroutine(Download());
	}


	public IEnumerator Download(){
	
		print ("http://192.168.50.43:8888/apacheShare/file/unity"+"/"+bundleName);
		WWW www=new WWW("http://192.168.50.43:8888/apacheShare/file/unity"+"/"+bundleName);
		yield return www;



		if(www.isDone){
			if(File.Exists(Application.persistentDataPath+"/"+bundleName)){
				File.Delete(Application.persistentDataPath+"/"+bundleName);
		} 
		
		
			//if(!File.Exists(Application.persistentDataPath+"/"+bundleName)){
				File.WriteAllBytes(Application.persistentDataPath+"/"+bundleName,www.bytes);
		//}
			if(OnDownLoadFinish!=null){
				OnDownLoadFinish(this);
			}


			Destroy(this);
		}
	}
}
