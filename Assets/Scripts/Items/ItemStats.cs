using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemStats", order = 1)]
public class ItemStats : ScriptableObject
{
    /// <summary>
    /// PLEASE READ THIS:
    /// 
    /// Since this is a scriptable object for storing data, all of the fields need to be public
    /// Otherwise, other classes would not be able to access the information inside
    /// 
    /// If it was a property, these values wouldn't be serializable
    /// 
    /// And it does NOT make sense to make 9 getter methods just so the fields can *be private*.
    /// It would be very arbitrary to make them private.
    /// 
    /// All if the below functions are automatically serialized due to being public and part of a serialized object
    /// </summary>
    public bool ChangeHoldAnimation = true;
    public bool HoldClick = true;
    public float UseCooldown = 20;
    public float RotationOffset = -Mathf.PI / 2;
    public float Scale = 0.75f;
    public int Damage = 5;
    public float ShotVelocity = 10.5f;
    public float Width = -1;
    public float Height = -1;
    public float DeaccelerationRate = 0.94f;
    public int Cost = 10;
}
