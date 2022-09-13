using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    //Reference to GameManager
    GameManager Game;

    string json;

    public SaveData UnlockedList = new SaveData();
    PowerCardData cards = new PowerCardData(); //Saving this saves a json with the card values cool for storage

    public void Awake()
    { 
        //Assign GameManager script reference
        Game = GetComponent<GameManager>();

        if (System.IO.File.Exists(Application.persistentDataPath + "/CardData.json"))
        {
            LoadData();
        }else
        {
            json = JsonUtility.ToJson(UnlockedList, true);
            File.WriteAllText(Application.persistentDataPath + "/CardData.json", json);
        }
    }

    //Save current Save Data to JSON file
    public void SaveData()
    {
        json = JsonUtility.ToJson(Game.SaveData, true);
        File.WriteAllText(Application.persistentDataPath + "/CardData.json", json);
        Debug.Log("Saved");
    }

    //Load data from JSON file
    public void LoadData()
    {
        SaveData LoadedList = JsonUtility.FromJson<SaveData>(File.ReadAllText(Application.persistentDataPath + "/CardData.json"));
        //Copy loaded data to save list.
        Game.SaveData = LoadedList;

        Debug.Log("Loaded");
    }
}