using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class VersionLoader : MonoBehaviour {
	public delegate void EventHandler(VersionLoader loader);
	public event EventHandler OnLoadFinishHandler;


	XmlDocGetter docGetter;
	protected string path;

	public XmlDocument xmlDoc;
	public string version;
	public Dictionary<int,string>files;
	// Use this for initialization
	protected void Start () {
	    files=new Dictionary<int, string> ();
		docGetter = gameObject.AddComponent<XmlDocGetter> ();
		docGetter.fileName =path+ "/AssetXML";
		docGetter.OnLoadFinish+=this.OnXmlLoadFinish;
	}
	
	void OnXmlLoadFinish(XmlDocument xmlDoc){
		docGetter.OnLoadFinish+=this.OnXmlLoadFinish;

		XmlElement versionElem = xmlDoc.DocumentElement;
		string v = versionElem.GetAttribute("v");
		XmlNode listNode = versionElem.GetElementsByTagName("list")[0]; 
		XmlNodeList fileNodes = ((XmlElement)listNode).GetElementsByTagName ("file");

		this.xmlDoc= xmlDoc;
		this.version = v;
		
		foreach (XmlNode f in fileNodes) {
			string id=((XmlElement)f).GetAttribute("id");
			string name=((XmlElement)f).GetAttribute("name");
			files.Add(Convert.ToInt32(id),name);
		}

		if (OnLoadFinishHandler != null) {
			OnLoadFinishHandler(this);	
		}
	}
}
