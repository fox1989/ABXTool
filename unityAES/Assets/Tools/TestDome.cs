using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class TestDome : MonoBehaviour
{


    //打印信息
    public string info = "";
    // Use this for initialization
    void Start()
    {

        AssetBundleDownload download = new AssetBundleDownload("test1", "http://192.168.50.26");
        download.OnFinishDownload += DownldFinish;
        download.OnError += Error;
        download.StartDownload(this);
       // AssetBundleDownload download1 = new AssetBundleDownload("test2", "http://192.168.50.26");
     // download1.StartDownload(this);
        info = "ddd";
        //  List<FileData> list = XmlUtils.Load<List<FileData>>("test1");
    }

    /// <summary>
    /// 载入完成
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="success"></param>
    /// <param name="info"></param>
    private void Loaded(string fileName, bool success, string infov)
    {
        info += ("fileName:" + fileName + "   resName:" + success.ToString() + "   version:" + infov);
        print("fileName:" + fileName + "   resName:" + success.ToString() + "   version:" + infov);
        //info += resName;
    }

    public AssetBundle assetBundel = null;
    // Update is called once per frame
    void Update()
    {
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Ping"))
        {
            Ping ping = new Ping("192.168.50.26");
        while(!ping.isDone)
        {}
            info += ping.time+ping.isDone.ToString();
        
            //info += "path:" + Application.persistentDataPath;

            //Player[] list = XmlUtils.AESLoad<Player[]>("myPlay");
            //foreach (Player p in list)
            //{
            //    info += ("name:" + p.name + " level: " + p.level + "  att1:" + p.att1);
            //}

        }


        if (GUI.Button(new Rect(100, 0, 100, 50), "写入"))
        {
            Player[] list = new Player[10];
            for (int i = 0; i < 10; i++)
            {
                Player p = new Player();
                p.id = i;
                p.name = "name";
                p.level = i * 2;
                p.att1 = i;
                p.att2 = i;
                p.hp = i;
                list[i] = p;

            }
            XmlUtils.Save("myPlay", list);


        }

        if (GUI.Button(new Rect(200, 0, 100, 50), "创建"))
        {
            float x = Random.Range(-4f, 4f);
            float y = Random.Range(-4f, 4f);
            float z = Random.Range(-4f, 4f);


            Instantiate((GameObject)ABS.GetRes("Capsule"), new Vector3(x, y, z), Quaternion.identity);

        }

        if (GUI.Button(new Rect(300, 0, 100, 50), "打印AESasset"))
        {
            info += "path:" + Application.streamingAssetsPath;

            Player[] list = XmlUtils.AESLoadByStreamingAssetsPath<Player[]>("player");
            foreach (Player p in list)
            {
                info += ("name:" + p.name + " level: " + p.level + "  att1:" + p.att1);
            }

        }

        if (GUI.Button(new Rect(400, 0, 100, 50), "打印asset"))
        {
            //info += "path:" + Application.streamingAssetsPath;

            // List<FileData> list = XmlUtils.LoadByStreamingAssetsPath<List<FileData>>("test1");
            // XmlUtils.path = Application.streamingAssetsPath + "/";
            //List<FileData> list = XmlUtils.Load<List<FileData>>("test1");
            //foreach (FileData p in list)
            //{
            //    info += ("name:" +p.fileName+ ",,");
            //}


            //FileStream fs = new FileStream(filePath, FileMode.Open);
            //StreamReader m_streamReader = new StreamReader(fs, Encoding.UTF8);
            //string str = m_streamReader.ReadToEnd();
            //if (fileStringsDic.ContainsKey(filePath))
            //    fileStringsDic[filePath] = str;
            //else
            //    fileStringsDic.Add(filePath, str);
            //m_streamReader.Close();
            //m_streamReader.Dispose();


            //info += list[1].fileName;
            info += ("startTime:" + System.DateTime.Now.ToString() + ":" + System.DateTime.Now.Millisecond.ToString());
            //Object o = Resources.Load("test1");
            //TextAsset t = o as TextAsset;
            //XmlSerializer xmls = new XmlSerializer(typeof(List<FileData>));
            //using (MemoryStream ms = new MemoryStream(t.bytes))
            //{
            //    List<FileData> list = (List<FileData>)xmls.Deserialize(ms);
            //    info += list[0].fileName;
            //}
            Player[] list = XmlUtils.AESLoadByStreamingAssetsPath<Player[]>("player");
            info += list[2].name;

            info += ("endTime:" + System.DateTime.Now.ToString() + ":" + System.DateTime.Now.Millisecond.ToString());

        }

        GUI.Label(new Rect(100, 50, 100, 700), info);
    }




    public void DownldFinish(string v, int i)
    {
        info += v + "   " + i;
        print(v + "   " + i);
        ABS.debug = false;
        ABS.OnLoaded += Loaded;
        ABS.StartLoadRes("test1", this);

    }

    public void Error(string value, int num)
    {
        info += value;
    }
}
