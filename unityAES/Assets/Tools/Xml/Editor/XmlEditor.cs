using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class XmlEditor
{
    /// <summary>
    /// 加密Xml文件到新的路径
    /// </summary>
    [MenuItem("Assets/Xml Encryption for New Path")]
    static void ExportAssetBundleEachfile2PathForIphone()
    {
        string assetsPath = Application.streamingAssetsPath;
        //如果没有文件夹，就新建一个
        if (!Directory.Exists(assetsPath))
        {
            Directory.CreateDirectory(assetsPath);
        }

        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (selection.Length <= 0)
        {
            EditorUtility.DisplayDialog("提示", "没有选择文件", "确定");
            return;
        }

        Selection.objects = selection;
        string path = "";
        List<string> pathList = new List<string>();
        foreach (Object o in selection)
        {
            path = AssetDatabase.GetAssetPath(o).Replace("Assets", Application.dataPath);
            pathList.Add(path);
           Debug.Log(path);
        }
        string savePath = EditorUtility.OpenFolderPanel("保存路径", Application.streamingAssetsPath, "");
       
        //加密文件
        foreach (string p in pathList)
        {
            XmlUtils.EncryptionXML(p, savePath);
        }
        System.GC.Collect();
    }
}
