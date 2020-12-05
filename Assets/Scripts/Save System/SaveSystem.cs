using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveShopData(ShopData shopDataCopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/shop.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        ShopData shopData = new ShopData(shopDataCopy);

        formatter.Serialize(stream, shopData);
        stream.Close();
    }

    public static ShopData LoadShopData()
    {
        string path = Application.persistentDataPath + "/shop.gm";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ShopData shopData = formatter.Deserialize(stream) as ShopData;
            stream.Close();

            return shopData;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            ShopData shopData = new ShopData();
            return shopData;
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