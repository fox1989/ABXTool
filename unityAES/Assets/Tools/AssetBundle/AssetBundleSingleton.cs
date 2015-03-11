using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;

public class ABS:MonoBehaviour
{
    public delegate void LoadedEvent(string fileName, bool success, string info);
    public static LoadedEvent OnLoaded;
    public static List<AssetBundle> assetBundleList = new List<AssetBundle>();
    /// <summary>
    /// 文件名列表
    /// </summary>
    public static List<FileData> fileDataList = null;

    // public AssetBundle assetBundle;


    public int total = 0;
    public int tempNum = 0;


    /// <summary>
    /// 是否是开发模式
    /// </summary>
    public static bool debug = true;

    /// <summary>
    /// 开始加载资源
    /// </summary>
    /// <returns></returns>
    public static void StartLoadRes(string fileName, MonoBehaviour b)
    {
        ABS abs = new ABS();

        string version = "0";
        bool isMultiple = false;
        List<FileData> list = XmlUtils.Load<List<FileData>>("Data/" + fileName, out version, out isMultiple);

        //加入文件名列表 
        foreach (FileData fd in list)
        {

            FileData fd1 = fileDataList.Where(a => a.fileName == fd.fileName).FirstOrDefault();
            if (fd1 != null)
            {
                fileDataList.Remove(fd1);
            }
            fileDataList.Add(fd);
        }


        if (list.Count <= 0)
        {
            if (OnLoaded != null)
            {
                OnLoaded(fileName, false, "加载失败，没有找到配置文件");
            }
        }
        else
        {
            if (isMultiple)//多文件加载 
            {
                abs.total = list.Count;
                foreach (FileData d in list)
                {
                    b.StartCoroutine(abs.LoadRes(d.fileName));
                }
            }
            else
            {
                b.StartCoroutine(abs.LoadRes(fileName));
            }
        }


    }



    /// <summary>
    /// 一个资源一个包
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetRes(string resName)
    {
        if (debug)
        {
            return Resources.Load(resName);
        }
        else
        {
            AssetBundle assetBundle = ABS.GetAssetBundleByName(resName);
            if (assetBundle != null)
            {
                return assetBundle.Load(resName);
            }
            return null;
        }
    }

    /// <summary>
    /// 对于多个资源打包在同一个包中
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public static UnityEngine.Object GetRes(string fileName, string resName)
    {
        if (debug)
        {
            return Resources.Load(resName);
        }
        else
        {
            AssetBundle assetBundle = ABS.GetAssetBundleByName(fileName);
            if (assetBundle == null)
            {
                return assetBundle.Load(resName);
            }
            return null;
        }
    }


    private static object o = new object();

    public IEnumerator LoadRes(string fileName)
    {


        if (!debug)
        {
            string path =
#if UNITY_EDITOR
 "file:///" + UnityEngine.Application.persistentDataPath;
#elif UNITY_IPHONE
              "file:///"+ Application.persistentDataPath;
#elif UNITY_ANDROID
               "file:///"+ Application.persistentDataPath;
   
#endif
            path += "/Data/";
            path += fileName;

            WWW www = new WWW(path + ".assetbundle");
            // UnityEngine.Application.persistentDataPath
            yield return www;
            TextAsset txt = www.assetBundle.Load(fileName, typeof(TextAsset)) as TextAsset;
            byte[] data = txt.bytes;
            byte[] newdata = AES.AESDecrypt(data);
            // StartCoroutine(LoadBundle(newdata)); 
            //创建资源
            AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory(newdata);
            yield return acr;
            AssetBundle assetBundle = acr.assetBundle;
            assetBundle.name = fileName;
            ABS.assetBundleList.Add(assetBundle);
            //www.assetBundle.Unload(false);
            //加载完成事件
            lock (o)//加锁
            {
                tempNum++;
            }

            if (tempNum == total)
            {
                if (OnLoaded != null)
                {
                    OnLoaded(fileName, true, "加载成功了");
                }
            }
        }
    }
    /// <summary>
    /// 同步加载 ------????
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static AssetBundle SynLoadRes(string fileName)
    {
        string path =
#if UNITY_EDITOR
 "file:///" + UnityEngine.Application.persistentDataPath;
#elif UNITY_IPHONE
                          "file:///"+ Application.persistentDataPath;
#elif UNITY_ANDROID
                           "file:///"+ Application.persistentDataPath;
   
#endif

        path += "/Data/";
        path += fileName;
      

        WWW www = new WWW(path + ".assetbundle");
       // TextAsset txt = www.assetBundle.Load(fileName, typeof(TextAsset)) as TextAsset;
       // byte[] data = txt.bytes;
      //  byte[] newArr = AES.AESDecrypt(data);
       /// AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory(newArr);
       // AssetBundle assetBundle = acr.assetBundle;
       // assetBundle.name = fileName;
       // ABS.assetBundleList.Add(assetBundle);

       // return assetBundle;
        return www.assetBundle;
    }


    /// <summary>
    /// 通过名字找到AssetBundle
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static AssetBundle GetAssetBundleByName(string name)
    {
        AssetBundle reAssetBundle = null;
        //缓存中找
        foreach (AssetBundle a in assetBundleList)
        {
            if (a.name == name)
            {
                reAssetBundle = a;
            }
        }
        return null;
    }




}
