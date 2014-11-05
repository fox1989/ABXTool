using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetBundleWindon :EditorWindow {

    private static AssetBundleWindon _window;
    public static int index = 0;
    public static bool isMultiple = false;
    public static float version = 0.0f; 
    string[] options = new string[] { "Android", "Windons", "IPhone" };

    
 

    [MenuItem("Assets/Open Assetsbundle Windon")]
    private static void Init()
    {
        _window = (AssetBundleWindon)EditorWindow.GetWindow(typeof(AssetBundleWindon));
    }


    void OnGUI()
    {

        index = EditorGUILayout.Popup("Choose the platform:", index, options, GUIStyle.none, null);

        isMultiple = EditorGUILayout.Toggle("isMultiple：", isMultiple);


        version = EditorGUILayout.FloatField("version:", version);

        if (GUILayout.Button("加密创建"))
        {
            switch (index)
            {
                case 0:
                    AssetBundleEditor.AssetBundleAndEncryption(BuildTarget.Android,isMultiple,version);
                    break;
                case 1:
                    AssetBundleEditor.AssetBundleAndEncryption(BuildTarget.StandaloneWindows, isMultiple, version);
                    break;
                case 2:
                    AssetBundleEditor.AssetBundleAndEncryption(BuildTarget.iPhone, isMultiple, version);
                    break;
            }
        }


        if (GUILayout.Button("测试"))
        {
            AssetBundleEditor.Test();
        }

    }
}
