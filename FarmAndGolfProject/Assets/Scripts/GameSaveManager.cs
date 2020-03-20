using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaveManager : MonoBehaviour
{
    //暂时只做了"背包里有什么"的存储,物品数量的存储还没做
    public Inventory myInventory;
    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();//进行二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/inventory.txt");

        var json = JsonUtility.ToJson(myInventory);

        formatter.Serialize(file, json);

        file.Close();
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/game_SaveData/inventory.txt"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/inventory.txt", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myInventory);

            file.Close();
        }
    }
}
