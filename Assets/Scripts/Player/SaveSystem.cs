﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SavePlayer(PlayerData playerData) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerdata.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();

        Debug.Log("Data file saved in " + path);
    }

    public static PlayerData LoadData() {
        string path = Application.persistentDataPath + "/playerdata.txt";
        
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void InitiateDataFile() {
        string path = Application.persistentDataPath + "/playerdata.txt";

        if (!File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerData playerData = new PlayerData(0, 0, 0);

            formatter.Serialize(stream, playerData);
            stream.Close();
        }
    }
}