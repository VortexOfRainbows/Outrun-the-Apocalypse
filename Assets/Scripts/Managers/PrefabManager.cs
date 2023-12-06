using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField]
    private GameObjectDictionary _prefab;
    [SerializeField]
    private ScriptedObjectDictionary _scriptableObject;
    /// <summary>
    /// Returns the prefab with the given index in the prefab array
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static GameObject GetPrefab(int index)
    {
        return instance._prefab.items[index]._gameObject;
    }
    /// <summary>
    /// Returns the prefab with the given dictionary key (which will usually be named after the prefab)
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static GameObject GetPrefab(string prefabName)
    {
        return Prefab.GetValueOrDefault(prefabName);
    }
    public static ScriptableObject GetScriptableObject(int index)
    {
        return instance._scriptableObject.items[index]._scriptedObject;
    }
    public static ScriptableObject GetScriptableObject(string objName)
    {
        return ScriptedObject.GetValueOrDefault(objName);
    }

    public static Dictionary<string, GameObject> Prefab;
    public static Dictionary<string, ScriptableObject> ScriptedObject;
    public static PrefabManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Prefab = _prefab.ToDictionary();
        ScriptedObject = _scriptableObject.ToDictionary();
    }
    private void Update()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        if (Prefab == null)
            Prefab = _prefab.ToDictionary();
        if (ScriptedObject == null)
            ScriptedObject = _scriptableObject.ToDictionary();
    }
}
