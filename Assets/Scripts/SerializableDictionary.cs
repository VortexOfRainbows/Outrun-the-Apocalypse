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
[Serializable]
public class NamedScriptedObject
{
    [SerializeField]
    public string _string;
    [SerializeField]
    public ScriptableObject _scriptedObject;
}
[Serializable]
public class ScriptedObjectDictionary
{
    [SerializeField]
    public NamedScriptedObject[] items;
    public Dictionary<string, ScriptableObject> ToDictionary()
    {
        Dictionary<string, ScriptableObject> dictionary = new Dictionary<string, ScriptableObject>();
        foreach (var item in items)
        {
            dictionary.Add(item._string, item._scriptedObject);
        }
        return dictionary;
    }
}