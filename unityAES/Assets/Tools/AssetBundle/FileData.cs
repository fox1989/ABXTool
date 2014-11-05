using UnityEngine;
using System.Collections;

public class FileData  {
    public FileData()
    { }

    public FileData(int id,string name)
    {
        this.id = id;
        this.fileName = name;
    }

    public int id;
    public string fileName;
}
