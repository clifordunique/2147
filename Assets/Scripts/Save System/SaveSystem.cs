using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveResourceData(ResourceData shopDataCopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/resource.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        ResourceData resourceData = new ResourceData(shopDataCopy);

        formatter.Serialize(stream, resourceData);
        stream.Close();
    }

    public static ResourceData LoadResourceData()
    {
        string path = Application.persistentDataPath + "/resource.gm";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ResourceData resourceData = formatter.Deserialize(stream) as ResourceData;
            stream.Close();

            return resourceData;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            ResourceData resourceData = new ResourceData();
            return resourceData;
        }
    }

    /*public static void LevelSave(LevelData levelDatacopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/leveldat.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData levelData = new LevelData(levelDatacopy);

        formatter.Serialize(stream, levelData);
        stream.Close();
    }

    public static LevelData LevelLoad()
    {
        string path = Application.persistentDataPath + "/leveldat.gm";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData levelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return levelData;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            LevelData levelData = new LevelData();
            return levelData;
        }
    }*/
}