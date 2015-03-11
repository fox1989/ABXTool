using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class AssetBundleDownload 
{
    public delegate void EventHandler(string value, int num);
    public EventHandler OnStartDownload;
    public EventHandler OnFinishDownload;
    public EventHandler OnDownloadChange;
    public EventHandler OnError;

    //public
    public string xmlName = "";
    public string serverPath = "";
    public int total;

    //private
    private int tempNum;
    private MonoBehaviour b;
    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="xmlName"></param>
    /// <param name="serverPath"></param>
    public AssetBundleDownload(string xmlName, string serverPath)
    {
        this.xmlName = xmlName;
        this.serverPath = serverPath;
    }


   public void StartDownload(MonoBehaviour b)
    {
        this.b = b;
        Debug.Log("start-----download");
        b.StartCoroutine(GetVersion());
    }


    /// <summary>
    /// 对比版本号
    /// </summary>
    /// <returns></returns>
    IEnumerator GetVersion()
    {
        WWW www = new WWW(serverPath + "/" + xmlName + ".xml");
        yield return www;
        Debug.Log("start-----GetVersion");

        //如果出错
        if (www.error != null)
        {
            if (OnError != null)
            {
                OnError(www.error, -1);
            }
            b.StopAllCoroutines();//关闭协程 
        }


        using (MemoryStream ms = new MemoryStream(www.bytes))
        {
            bool serverIsMultiple = false;
            bool localIsMultiple = false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ms);
            XmlElement versionElem = xmlDoc.DocumentElement;
            //得到服务器版号
            string serverVersion = versionElem.GetAttribute("xmlns:v");
            serverIsMultiple = Convert.ToBoolean(versionElem.GetAttribute("xmlns:multiple"));
            //返回到0项
            ms.Position = 0;
          
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<FileData>));
            List<FileData> serverList = (List<FileData>)xmlSerializer.Deserialize(ms);
            string localVersion = "";
          
            List<FileData> localList = new List<FileData>();
            if (!Directory.Exists(Application.persistentDataPath + "/Data"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Data");
            }


            string strPathtest = Application.persistentDataPath + "/Data/" + xmlName + ".xml";
            if (File.Exists(Application.persistentDataPath + "/Data/" + xmlName + ".xml"))
            {
                localList = XmlUtils.Load<List<FileData>>("Data/"+xmlName, out localVersion, out localIsMultiple);
            }
            bool flag = false;//是否下载
            if (localList.Count > 0)
            {
                if (!localVersion.Equals(serverVersion))//下载服务器的资源
                {
                    DownloadNewDeleteOld(serverList, serverVersion, serverIsMultiple);
                }
                else//对比文件数是否相同
                {
                    
                    if (localIsMultiple)//是否是多文件
                    {
                        foreach (FileData d in localList)
                        {
                            if (!File.Exists(Application.persistentDataPath + "/Data/" + d.fileName + ".assetbundle"))
                            {
                                flag = true;
                                DownloadNewDeleteOld(serverList, serverVersion, serverIsMultiple);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!File.Exists(Application.persistentDataPath + "/Data/" + xmlName + ".assetbundle"))
                        {
                            flag = true;
                            DownloadNewDeleteOld(serverList, serverVersion, serverIsMultiple);
                        }
                    }
                }
            }
            else//下载服务器的资源
            {
                flag = true;
                DownloadNewDeleteOld(serverList, serverVersion, serverIsMultiple);
            }
            if (!flag)
            {
                if (OnFinishDownload != null)
                {
                    OnFinishDownload("本地资源完整，不用下载", 0);
                }
            }
        }
    }



    void DownloadNewDeleteOld(List<FileData> list,string version,bool isMultiple)
    {
        Debug.Log("start-----DownloadNewDeleteOld");

        string localPath = Application.persistentDataPath + "/Data";
        //删除本地xml
        if (File.Exists(localPath + "/" + xmlName + ".xml"))
        {
            File.Delete(localPath + "/" + xmlName + ".xml");
        }
        //删除文件
        foreach (FileData d in list)
        {
            if (File.Exists(localPath + "/" + d.fileName + ".assetbundle"))
            {
                File.Delete(localPath + "/" + d.fileName + ".assetbundle");
            }
        }

        //写入xml
        XmlUtils.SaveByPath(localPath + "/" + xmlName, list, version, isMultiple);
        total = list.Count;
        if (OnStartDownload != null)
        {
            OnStartDownload("开始下载~~", list.Count);
        }
        OnDownloadChange += Finish;
        if (isMultiple)
        {
            foreach (FileData fd in list)
            {
                b.StartCoroutine(DownloadFile(fd.fileName));
            }
        }
        else
        {
            b.StartCoroutine(DownloadFile(xmlName));
        }



    }
    IEnumerator DownloadFile(string fileName)
    {
        Debug.Log("start-----DownloadFile:" + fileName);

        string assetBundleName = fileName + ".assetbundle";
        WWW www = new WWW(serverPath + "/" + assetBundleName);

        yield return www;

        //如果出错
        if (www.error != null)
        {
            if (OnError != null)
            {
                OnError(www.error, -1);
            }
            b.StopAllCoroutines();//关闭协程 
        }

        File.WriteAllBytes(Application.persistentDataPath + "/Data/" + assetBundleName, www.bytes);

        if (OnDownloadChange != null)
        {
            OnDownloadChange(fileName, 1);
        }

    }

   private static object o = new object();


    void Finish(string value, int i)
    {
        Debug.Log(value);
        lock (o)
        {
            tempNum++;
        }
        if (tempNum == total)
        {
            if (OnFinishDownload != null)
            {
                OnFinishDownload("已经下载完", total);
            }
        }
    }





}
