using UnityEditor.MemoryProfiler;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb;
    public ItemData LeftHeldItem;
    public ItemData RightHeldItem;
    public int Direction = 1;
    public int LastDirection = 1;
    public Vector2 PrevVelocity = Vector2.zero;
    public Vector2 Velocity = Vector2.zero;
    public virtual int LayerDefaultPosition => 0;
    public virtual float ArmDegreesOffset => 0f;
    public Vector2 LookTarget;
}