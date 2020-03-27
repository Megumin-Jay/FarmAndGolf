using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;//即Input和Output,代表文件的输入和输出
using System.Runtime.Serialization.Formatters.Binary;//将任何数据转化为二进制的文件

public class GameSaveManager : MonoBehaviour
{
    //暂时只做了"背包里有什么"的存储,物品数量的存储还没做
    public Inventory myInventory;//先生成我们要存储的变量

    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);//直接输出这个保存文件的文件夹的路径
        if (!Directory.Exists(Application.persistentDataPath + "/game_SaveData"))//如果没有文件夹,则生成一个
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_SaveData");
        }

        BinaryFormatter formatter = new BinaryFormatter();//进行二进制转化

        FileStream file = File.Create(Application.persistentDataPath + "/game_SaveData/inventory.txt");//创建一个文件,把数据存储进去

        //如果用来存储scriptableObject之外的类,就不需要json了,直接序列化反序列化即可
        var json = JsonUtility.ToJson(myInventory);//将物体变成json,用overwrite重新写回到scriptableObject

        formatter.Serialize(file, json);//把上面的json用二进制的方法写到文件夹当中

        file.Close();//相当于保存,不然文档是在闪存中,关机即消失
    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();//进行二进制转化

        if (File.Exists(Application.persistentDataPath + "/game_SaveData/inventory.txt"))//先判断文件是否存在 
        {
            //打开文件
            FileStream file = File.Open(Application.persistentDataPath + "/game_SaveData/inventory.txt", FileMode.Open);

            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myInventory);//反序列化之后覆盖重写

            file.Close();//关闭!别忘了这个!
        }
    }
}
