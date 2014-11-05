using UnityEngine;
using System.Collections;
using System.Text;
using UnityEditor;
using System.IO;

public class NewBehaviourScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public AssetBundle assetBundel = null;
    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.A))
        {
            string text = "test";
            byte[] aesByte = Encoding.UTF8.GetBytes(text);
            print("加密前：" + text);
            aesByte = AES.AESEncrypt(aesByte);
            print("加密后：" + Encoding.UTF8.GetString(aesByte));
            aesByte = AES.AESDecrypt(aesByte);
            print("解密后：" + Encoding.UTF8.GetString(aesByte));

        }


        if (Input.GetKeyDown(KeyCode.B))
        {

            print(Application.dataPath);
          
            if (assetBundel!= null)
            {
                print(assetBundel.Load("Cube").name);
            }
            else
            {
                StartCoroutine(LoadRes());
            }
        }
    }


    //在Unity编辑器中添加菜单
    [MenuItem("Assets/Build AssetBundle And Encryption From Selection")]
    static void ExportResourceRGB2()
    {
        // 打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);

            Selection.objects = selection;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            byte[] buff = new byte[fs.Length];
            fs.Read(buff, 0, (int)fs.Length);
            byte[] newBuff = AES.AESEncrypt(buff);
            fs.Close();
            File.Delete(path);
            string BinPath = path.Substring(0, path.LastIndexOf('.')) + ".bytes";
            FileStream cfs = new FileStream(BinPath, FileMode.Create);
            cfs.Write(buff, 0, newBuff.Length);
            buff = null;
            cfs.Close();
        }
    }


    //在Unity编辑器中添加菜单
    /// <summary>
    /// 加密assetBundle
    /// </summary>
    [MenuItem("Assets/Build  AssetBundle From Selection")]
    static void AssetBundleEncryption()
    {
         string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");

        if (path.Length != 0)
        {
            // 选择的要保存的对象
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows);
        }
    }



    /// <summary>
    /// 导入
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadRes()
    {
            WWW www = WWW.LoadFromCacheOrDownload("file:///" + Application.dataPath + "/Resources/" + "test.assetbundle", 1);
            yield return www;
        
            TextAsset txt = www.assetBundle.Load("2222", typeof(TextAsset)) as TextAsset;
            byte[] data = txt.bytes;
            byte[] newdata = AES.AESDecrypt(data);
            StartCoroutine(LoadBundle(newdata));
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="decryptedData"></param>
    /// <returns></returns>
    IEnumerator LoadBundle(byte[] decryptedData)
    {
        AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory(decryptedData);
        yield return acr;
        assetBundel = acr.assetBundle;
        Instantiate(assetBundel.Load("Cube"));
    }

}
