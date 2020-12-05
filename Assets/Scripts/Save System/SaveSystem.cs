using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveResourceData(ResourceData resourceDataCopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/resourcedata.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        //ResourceData resourceData = new ResourceData(shopDataCopy);

        formatter.Serialize(stream, resourceDataCopy);
        stream.Close();
    }

    public static ResourceData LoadResourceData()
    {
        string path = Application.persistentDataPath + "/resourcedata.gm";
        
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

    public static void SavePlayerData(PlayerData playerDataCopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerDataCopy);
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerdata.gm";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            PlayerData playerData = new PlayerData();
            return playerData;
        }
    }

    public static void SaveCampaignProgressData(CampaignProgressData campaignProgressDataCopy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/campaignprogressdata.gm";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, campaignProgressDataCopy);
        stream.Close();
    }

    public static CampaignProgressData LoadCampaignProgressData()
    {
        string path = Application.persistentDataPath + "/campaignprogressdata.gm";
        
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CampaignProgressData campaignProgressData = formatter.Deserialize(stream) as CampaignProgressData;
            stream.Close();

            return campaignProgressData;
        }
        else
        {
            Debug.LogError("Save File Not Found in " + path);
            CampaignProgressData campaignProgressData = new CampaignProgressData();
            return campaignProgressData;
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