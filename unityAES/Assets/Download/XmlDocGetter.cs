using System;
using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlDocGetter: MonoBehaviour {
	
	public delegate void EventHandler(XmlDocument xmlDoc);
	public event EventHandler OnLoadFinish;
	
	public string fileName;

	void Start() {

		StartCoroutine (Load());
	}
	
	IEnumerator Load (){
		print (fileName + ".xml");

				WWW www = new WWW (fileName + ".xml");
				yield return www;

				if (www.isDone) {

						XmlDocument xmlDoc = new XmlDocument ();
						xmlDoc.LoadXml (XmlSaver.UTF8ByteArrayToString (www.bytes).Trim ());

						if (OnLoadFinish != null) {
								OnLoadFinish (xmlDoc);
						}
						Destroy (this);
				}
		}
}
