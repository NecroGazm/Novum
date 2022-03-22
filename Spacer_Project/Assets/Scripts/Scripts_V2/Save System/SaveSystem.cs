using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveHighScore (TempHSData dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.SV";

        FileStream stream = new FileStream(path, FileMode.Create);

        HighScoreData data = new HighScoreData(dataToSave);
        data.highscores = dataToSave.highscores;

        formatter.Serialize(stream, data);

        stream.Close();

    }

    public static HighScoreData LoadScore()
    {
        string path = Application.persistentDataPath + "/player.SV";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HighScoreData data = formatter.Deserialize(stream) as HighScoreData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found");
            return null;
        }
    }

    public static void ClearSaveData()
    {
        string path = Application.persistentDataPath + "/player.SV";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

}
