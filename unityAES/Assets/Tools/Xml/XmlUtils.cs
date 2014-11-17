using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;




/// <summary>
/// xml实体映射工具
/// </summary>
public class XmlUtils
{
    /// <summary>
    /// 默认路径
    /// </summary>
    ///    //不同平台下persistentDataPath的路径是不同的，这里需要注意一下。
    private static string defalutPath =
#if UNITY_ANDROID   //安卓
 UnityEngine.Application.persistentDataPath + "/";

#elif UNITY_IPHONE  //iPhone
        UnityEngine.Application.persistentDataPath + "/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
 UnityEngine.Application.persistentDataPath + "/";
#else
        string.Empty;
#endif
    public static string DefalutPath
    {
        get { return XmlUtils.defalutPath; }

    }

    public static string path = DefalutPath;




    /// <summary>
    /// 模型保存为xml
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="obj">模型对象</param>
    public static void Save(string fileName, Object obj)
    {
        FileStream fs = null;
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(fs, obj);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }
    }

    /// <summary>
    /// 给一个路径保存
    /// </summary>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public static void SaveByPath(string mypath, Object obj, string version, bool isMultiple)
    {
        FileStream fs = null;
        try
        {

            fs = new FileStream(mypath + ".xml", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlSerializerNamespaces xmlNameSpaces = new XmlSerializerNamespaces();
            xmlNameSpaces.Add("v", version);
            xmlNameSpaces.Add("multiple", isMultiple.ToString());
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(fs, obj, xmlNameSpaces);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }
    }





    /// <summary>
    /// 在一个文件下追加
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="obj">模型对象</param>
    public static void Addend(string fileName, Object obj)
    {
        FileStream fs = null;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        try
        {
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(fs, obj);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }
    }


    /// <summary>
    /// xml解析为模型
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    /// <param name="filename">文件名</param>
    /// <returns></returns>
    public static T Load<T>(string fileName)
    {
        FileStream fs = null;
        try
        {
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(fs);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }
    }


    /// <summary>
    /// xml解析为模型
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    /// <param name="filename">文件名</param>
    /// <param name="version">版本号</param>
    /// <returns></returns>
    public static T Load<T>(string fileName, out string version, out bool isMultiple)
    {
        FileStream fs = null;
        try
        {
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fs);
            XmlElement versionElem = xmlDoc.DocumentElement;
            //得到服务器版号
            version = versionElem.GetAttribute("xmlns:v");
            isMultiple = Convert.ToBoolean(versionElem.GetAttribute("xmlns:multiple"));
            //返回
            fs.Position = 0;

            return (T)serializer.Deserialize(fs);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();

        }
    }



    /// <summary>
    /// 加密序列化
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="obj">对像</param>
    public static void AESSave(string fileName, Object obj)
    {
        FileStream fs = null;
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);

                byte[] arr = ms.ToArray();
                Encoding.UTF8.GetString(arr);
                byte[] AESArr = AES.AESEncrypt(arr);
                fs.Write(AESArr, 0, AESArr.Length);
                ms.Close();
            }
            fs.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }

    }



    /// <summary>
    /// 加密序列化
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="obj">对像</param>
    /// <param name="version">版本吗</param>
    public static void AESSave(string fileName, Object obj, string version)
    {
        FileStream fs = null;
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlSerializerNamespaces xmlNameSpaces = new XmlSerializerNamespaces();
                xmlNameSpaces.Add("v", version);
                serializer.Serialize(ms, obj, xmlNameSpaces);
                byte[] arr = ms.ToArray();

                Encoding.UTF8.GetString(arr);
                byte[] AESArr = AES.AESEncrypt(arr);
                fs.Write(AESArr, 0, AESArr.Length);
                ms.Close();
            }
            fs.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();
        }

    }
    /// <summary>
    /// 解密反序列化
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static T AESLoad<T>(string fileName)
    {
        FileStream fs = null;
        try
        {
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            int numBytesToRead = (int)fs.Length;
            int numBytesRead = 0;
            byte[] readArr = new byte[fs.Length];
            //读取字节
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fs.Read(readArr, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }


            byte[] reArr = AES.AESDecrypt(readArr);
            using (MemoryStream ms = new MemoryStream(reArr))
            {
                return (T)serializer.Deserialize(ms);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();

        }
    }



    /// <summary>
    /// 解密反序列化
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static T AESLoad<T>(string fileName, out string version, out bool isMultiple)
    {
        FileStream fs = null;
        try
        {
            fileName = fileName + ".xml";
            fs = new FileStream(path + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            int numBytesToRead = (int)fs.Length;
            int numBytesRead = 0;
            byte[] readArr = new byte[fs.Length];
            //读取字节
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = fs.Read(readArr, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }


            byte[] reArr = AES.AESDecrypt(readArr);
            using (MemoryStream ms = new MemoryStream(reArr))
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                XmlElement versionElem = xmlDoc.DocumentElement;
                //得到版号
                version = versionElem.GetAttribute("xmlns:v");
                isMultiple = Convert.ToBoolean(versionElem.GetAttribute("xmlns:multiple"));
                //返回
                ms.Position = 0;
                return (T)serializer.Deserialize(ms);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (fs != null) fs.Close();

        }
    }

    /// <summary>
    /// 从StreamingAssetsPath加载加密xml
    /// </summary>
    /// <typeparam name="T">模型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static T AESLoadByStreamingAssetsPath<T>(string fileName)
    {
        string streamingAssetsPath = "";
        streamingAssetsPath=
#if UNITY_EDITOR
 "file:///" + UnityEngine.Application.dataPath + "/StreamingAssets" + "/";
#elif UNITY_IPHONE
          UnityEngine.Application.dataPath +"/Raw"+"/";
#elif UNITY_ANDROID
        UnityEngine.Application.streamingAssetsPath+"/";
#endif

        streamingAssetsPath += (fileName + ".xml");

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            UnityEngine.WWW www = new UnityEngine.WWW(streamingAssetsPath);
            while (!www.isDone)
            {
            }

            byte[] readArr = www.bytes;
            byte[] reArr = AES.AESDecrypt(readArr);
            using (MemoryStream ms = new MemoryStream(reArr))
            {
                return (T)serializer.Deserialize(ms);

            }
        }
        catch (Exception ex)
        {
            throw ex;

        }

    }

    /// <summary>
    /// 从StreamingAssetsPath加载加密xml
    /// </summary>
    /// <typeparam name="T">模型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <returns></returns>
    public static T LoadByStreamingAssetsPath<T>(string fileName)
    {
        string streamingAssetsPath = "";
        streamingAssetsPath =
#if UNITY_EDITOR
 "file:///" + UnityEngine.Application.dataPath + "/StreamingAssets" + "/";
#elif UNITY_IPHONE
          UnityEngine.Application.dataPath +"/Raw"+"/";
#elif UNITY_ANDROID
        UnityEngine.Application.streamingAssetsPath+"/";
#endif

        streamingAssetsPath += (fileName + ".xml");

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            UnityEngine.WWW www = new UnityEngine.WWW(streamingAssetsPath);
            while (!www.isDone)
            {
            }

            byte[] readArr = www.bytes;
            //byte[] reArr = AES.AESDecrypt(readArr);
            using (MemoryStream ms = new MemoryStream(readArr))
            {
                return (T)serializer.Deserialize(ms);

            }
        }
        catch (Exception ex)
        {
            throw ex;

        }

    }


    /// <summary>
    /// 加密xml文件
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="savePath">保存路径</param>
    public static void EncryptionXML(string path, string savePath)
    {

        FileStream saveFs = null;
        FileStream readFs = null;
        try
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string fileName = Path.GetFileName(path);
            UnityEngine.Debug.Log("savePath" + savePath);
            saveFs = new FileStream(savePath + "/" + fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            readFs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //读取字节流
            int numBytesToRead = (int)readFs.Length;
            int numBytesRead = 0;
            byte[] readByte = new byte[readFs.Length];
            //读取字节
            while (numBytesToRead > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                int n = readFs.Read(readByte, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }
            //加密
            byte[] writeByte = AES.AESEncrypt(readByte);
            //写文件
            saveFs.Write(writeByte, 0, writeByte.Length);
            readFs.Close();
            saveFs.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (saveFs != null) saveFs.Close();
            if (readFs != null) readFs.Close();
        }
    }


}