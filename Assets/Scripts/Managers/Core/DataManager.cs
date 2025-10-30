using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}


public class DataManager 
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();

    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, key, Value>(string path) where Loader : ILoader<key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");

        if (textAsset == null)
        {
            Debug.LogError($"[LoadJson] Resource not found at: Data/{path}");
            return default;
        }

        Debug.Log($"[LoadJson] Raw JSON:\n{textAsset.text}");

        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    //JSON{}:Class 등 오브젝트 , [] : List, Array 등 배열
}
