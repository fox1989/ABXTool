using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Threading;
using System.Collections.Generic;

public class AssetBundleEditor
{

    //在Unity编辑器中添加菜单-----资源打包，加密二进制文件

    public static void AssetBundleAndEncryption(BuildTarget buildTarget, bool flag, float version)
    {
        // 打开保存面板，获得用户选择的路径
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
        string configName = Path.GetFileName(path);
        configName = configName.Substring(0, configName.LastIndexOf("."));

        if (path.Length != 0)
        {
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包为一个文件 
            if (!flag)
            {
                // 选择的要保存的对象
                if (selection.Length <= 0)
                {
                    EditorUtility.DisplayDialog("提示", "没有选择文件", "确定");
                    return;
                }

                //第一次打包
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, buildTarget);

                Selection.objects = selection;
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

                byte[] newBuff = GetBytesByFileStream(fs);
                fs.Close();
                File.Delete(path);
                string BinPath = path.Substring(0, path.LastIndexOf('.')) + ".bytes";
                FileStream cfs = new FileStream(BinPath, FileMode.Create);
                cfs.Write(newBuff, 0, newBuff.Length);
                newBuff = null;
                cfs.Close();
                 string localPath = BinPath.Substring(BinPath.IndexOf("Assets"));
                //重新打包
                Debug.Log(localPath);
                //刷新资源
                AssetDatabase.Refresh();
                Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                BuildPipeline.BuildAssetBundle(t, null, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, buildTarget);
                File.Delete(BinPath);
            }
            else//打包为多个文件 
            {
                path = path.Substring(0, path.LastIndexOf("/")) + "/";
                foreach (Object o in selection)
                {
                    string tempPath = path + o.name + ".assetbundle";
                    BuildPipeline.BuildAssetBundle(o, null, tempPath, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, buildTarget);
                    FileStream fs = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite);

                    byte[] newBuff = GetBytesByFileStream(fs);

                    fs.Close();
                    File.Delete(tempPath);
                    string BinPath = tempPath.Substring(0, tempPath.LastIndexOf('.')) + ".bytes";
                    FileStream cfs = new FileStream(BinPath, FileMode.Create);
                    cfs.Write(newBuff, 0, newBuff.Length);
                    newBuff = null;
                    cfs.Close();
                    //重新打包
                    string localPath = BinPath.Substring(BinPath.IndexOf("Assets"));
                    AssetDatabase.Refresh();
                    Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                    BuildPipeline.BuildAssetBundle(t, null, tempPath, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, buildTarget);
                    File.Delete(BinPath);
                }
            }

            int id = 0;
            List<FileData> list = new List<FileData>();
            foreach (Object o in selection)
            {
                list.Add(new FileData(id, o.name));
                id++;
            }
            XmlUtils.SaveByPath(path.Substring(0, path.LastIndexOf("/")) + "/" + configName, list, version.ToString(), flag);
        }

    }

    /// <summary>
    /// 得到bytes 加密
    /// </summary>
    /// <param name="fs">文件流</param>
    /// <returns>byte[]</returns>
    private static byte[] GetBytesByFileStream(FileStream fs)
    {
        //读取字节流
        int numBytesToRead = (int)fs.Length;
        int numBytesRead = 0;
        byte[] readByte = new byte[fs.Length];
        //读取字节
        while (numBytesToRead > 0)
        {
            // Read may return anything from 0 to numBytesToRead.
            int n = fs.Read(readByte, numBytesRead, numBytesToRead);

            // Break when the end of the file is reached.
            if (n == 0)
                break;

            numBytesRead += n;
            numBytesToRead -= n;
        }


        //加密
        byte[] newBuff = AES.AESEncrypt(readByte);
        return newBuff;
    }


  /// <summary>
  /// 测试
  /// </summary>
    public static void Test()
    {
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "assetbundle");
        string configName = Path.GetFileName(path);

        if (path.Length != 0)
        {  
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            //打包为一个文件 
          
                // 选择的要保存的对象
                if (selection.Length <= 0)
                {
                    EditorUtility.DisplayDialog("提示", "没有选择文件", "确定");
                    return;
                }

                //第一次打包
                BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);

                Selection.objects = selection;
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

                byte[] newBuff = GetBytesByFileStream(fs);
                fs.Close();
                File.Delete(path);
                string BinPath = path.Substring(0, path.LastIndexOf('.')) + ".bytes";
                FileStream cfs = new FileStream(BinPath, FileMode.Create);
                cfs.Write(newBuff, 0, newBuff.Length);
                newBuff = null;
                cfs.Close();
                 string localPath = BinPath.Substring(BinPath.IndexOf("Assets"));
                //重新打包
                Debug.Log(localPath);
                //刷新资源
                AssetDatabase.Refresh();
                Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                BuildPipeline.BuildAssetBundle(t, null, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
                File.Delete(BinPath);

        }
    }
}
