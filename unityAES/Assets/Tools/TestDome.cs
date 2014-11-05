using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class TestDome : MonoBehaviour
{
   

    //打印信息
    public string info = "";
    // Use this for initialization
    void Start()
    {

        AssetBundleDownload download = new AssetBundleDownload("test1", "http://192.168.50.24");
        download.OnFinishDownload += DownldFinish;
        download.StartDownload(this);
        AssetBundleDownload download1 = new AssetBundleDownload("test2", "http://192.168.50.24");
        download1.StartDownload(this);
    }

    /// <summary>
    /// 载入完成
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="success"></param>
    /// <param name="info"></param>
    private void Loaded(string fileName, bool success, string info)
    {
        print("fileName:" + fileName + "   resName:" +success.ToString()+ "   version:" + info);
        //info += resName;
    }

    public AssetBundle assetBundel = null;
    // Update is called once per frame
    void Update()
    {
    }



    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "打印"))
        {
            info += "path:" + Application.persistentDataPath;

            Player[] list = XmlUtils.AESLoad<Player[]>("myPlay");
            foreach (Player p in list)
            {
                info += ("name:" + p.name + " level: " + p.level + "  att1:" + p.att1);
            }

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
            XmlUtils.AESSave("myPlay", list);
           

        }

        if (GUI.Button(new Rect(200, 0, 100, 50), "创建"))
        {
            float x = Random.Range(-4f, 4f);
            float y = Random.Range(-4f, 4f);
            float z = Random.Range(-4f, 4f);


            Instantiate((GameObject)ABS.GetRes("Capsule"), new Vector3(x, y, z), Quaternion.identity);

        }

        if (GUI.Button(new Rect(300, 0, 100, 50), "打印asset"))
        {
           info += "path:" + Application.streamingAssetsPath;

            Player[] list = XmlUtils.AESLoadByStreamingAssetsPath<Player[]>("player");
            foreach (Player p in list)
            {
                info += ("name:" + p.name + " level: " + p.level + "  att1:" + p.att1);
            }

        }

        GUI.Label(new Rect(100, 50, 100, 700), info);
    }




    public void DownldFinish(string v, int i)
    {
        print(v +"   "+ i);
        ABS.debug = false; 
        ABS.OnLoaded += Loaded;
        ABS.StartLoadRes("test1", this);
      
    }
}
