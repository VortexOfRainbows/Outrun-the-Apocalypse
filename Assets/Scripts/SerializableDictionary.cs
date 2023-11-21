using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NamedGameObject
{
    [SerializeField]
    public string _string;
    [SerializeField]
    public GameObject _gameObject;
}
[Serializable]
public class GameObjectDictionary
{
    [SerializeField]
    public NamedGameObject[] items;
    public Dictionary<string, GameObject> ToDictionary()
    {
        Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
        foreach (var item in items)
        {
            dictionary.Add(item._string, item._gameObject);
        }
        return dictionary;
    }
}