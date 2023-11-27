using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterAnimator : MonoBehaviour
{
    private static Vector2 LeftArmRelativePosition = new Vector2 (-4.5f, 2.5f);
    private static Vector2 RightArmRelativePosition = new Vector2(4.5f, 2.5f);
    private static Vector2 FrontLegRelativePosition = new Vector2(-2f, -4f);
    private static Vector2 BackLegRelativePosition = new Vector2(2f, -4f);
    private static Vector2 HeadRelativePosition = new Vector2(0, 6);
    private static Vector2 EyeRelativePosition = new Vector2(2, 5);
    private static Vector2 ShadowRelativePosition = new Vector2(0.5f, 5.5f);
    [SerializeField]
    public Entity Entity;
    [SerializeField]
    public HeldItem LeftItem;
    [SerializeField]
    public HeldItem RightItem;
    [SerializeField]
    List<GameObject> Limbs;
    private int Layer => Entity.LayerDefaultPosition;
    public void InitValues()
    {
        Body = Limbs[0];
        Head = Limbs[1];
        Eyes = Limbs[2];
        Shadow = Limbs[3];
        BackLeg = Limbs[4];
        FrontLeg = Limbs[5];
        RightArm = Limbs[6];
        LeftArm = Limbs[7];

        Body.GetComponent<SpriteRenderer>().sortingOrder = Layer;
        Head.GetComponent<SpriteRenderer>().sortingOrder = Layer + 1;
        Eyes.GetComponent<SpriteRenderer>().sortingOrder = Layer + 3;
        Shadow.GetComponent<SpriteRenderer>().sortingOrder = Layer + 2;
        BackLeg.GetComponent<SpriteRenderer>().sortingOrder = Layer - 3;
        FrontLeg.GetComponent<SpriteRenderer>().sortingOrder = Layer - 2;
        RightArm.GetComponent<SpriteRenderer>().sortingOrder = Layer - 1;
        LeftArm.GetComponent<SpriteRenderer>().sortingOrder = Layer + 4;
    }
    GameObject Body;
    GameObject Head;
    GameObject BackLeg;
    GameObject FrontLeg;
    GameObject RightArm;
    GameObject LeftArm;
    GameObject Shadow;
    GameObject Eyes;
    public void InitLimbs()
    {
        InitValues();
        foreach (GameObject l in Limbs)
        {
            l.transform.rotation = 0.0f.ToQuaternion();
            l.transform.localPosition = Vector3.zero;
        }
    }
    public void Start()
    {
        InitLimbs();
    }
    private float HeadRotationToCursor = 0f;
    private float LeftArmRotationToCursor = 0f;
    private float RightArmRotationToCursor = 0f;
    private Vector2 ToMouse => (Entity.LookTarget - (Vector2)Head.transform.position);
    public void FlipAll(bool flipX)
    {
        Body.transform.localScale = new Vector3(flipX ? -1.0f : 1.0f, 1.0f, 1.0f);
    }
    public void PerformUpdate()
    {
        if (Entity == null)
            return;
        FlipAll(Entity.Direction == -1);
        RotateHeadToCursor();
        RotateArmToCursor(ref LeftArm, ref LeftArmRotationToCursor, LeftArmRelativePosition);
        RotateArmToCursor(ref RightArm, ref RightArmRotationToCursor, RightArmRelativePosition);
        LeftItem.item = Entity.LeftHeldItem;
        RightItem.item = Entity.RightHeldItem;
        LeftItem.transform.parent = LeftArm.transform;
        RightItem.transform.parent = RightArm.transform;
        Animate();
        if(LeftItem.item != null)
            LeftItem.ItemUpdate();
        if(RightItem.item != null)
            RightItem.ItemUpdate();
    }
    private const float WalkAnimCircleMagnitude = 7f; //How large is the walk animation?
    private const float WalkAnimSpeedMult = 2.2f; //How fast is the walk animation?
    private const float WalkAnimYMult = 0.25f; //How should the size of animation be modified, Y value?
    private const float WalkAnimXMult = 0.15f; //How should the size of animation be modified, X value?
    private float walkCounter = 0; //Counter for animation progress
    public void Animate()
    {
        float walkDirection = 1f;
        //if (Entity.Velocity.y < -0.0 && MathF.Abs(Entity.Velocity.y) > 0.001f && MathF.Abs(Entity.Velocity.x) < 0.001f)
        //    walkDirection = -1;
        float velocity = Entity.Velocity.magnitude;
        float walkSpeedMultiplier = Mathf.Clamp(Math.Abs(velocity / 2f), 0, 1f);
        walkCounter += walkDirection * velocity * Mathf.Deg2Rad * walkSpeedMultiplier * WalkAnimSpeedMult;
        walkCounter = walkCounter.WrapAngle();
        //walkcounter *= walkSpeedMultiplier;
        Vector2 circularMotion = new Vector2(WalkAnimCircleMagnitude, 0).RotatedBy(-walkCounter) * walkSpeedMultiplier;
        circularMotion.y *= WalkAnimYMult;
        circularMotion.x *= WalkAnimXMult * walkSpeedMultiplier;
        float runningTilt = velocity * 0.0125f * walkSpeedMultiplier * Entity.Direction;
        float bobbingMotion = 0.5f + 0.5f * (float)MathF.Cos(walkCounter * 2);

        Head.transform.localPosition = new Vector3(HeadRelativePosition.x, HeadRelativePosition.y - bobbingMotion * walkSpeedMultiplier);
        Body.transform.localRotation = (-runningTilt).ToQuaternion();
        Eyes.transform.localPosition = EyeRelativePosition;
        Shadow.transform.localPosition = ShadowRelativePosition;

        AnimateHalf(circularMotion, runningTilt, -1);
        AnimateHalf(circularMotion, runningTilt, 1);
    }
    public void AnimateHalf(Vector2 circularMotion, float runningTilt, int side = -1)
    {
        GameObject Leg;
        GameObject Arm;
        GameObject OppositeArm;
        Vector2 ArmPosition;
        Vector2 LegPosition;
        HeldItem HeldItem;
        HeldItem OppositeHeldItem;
        if (side == -1)
        {
            Leg = BackLeg;
            LegPosition = BackLegRelativePosition;
            Arm = LeftArm;
            OppositeArm = RightArm;
            ArmPosition = LeftArmRelativePosition;
            HeldItem = LeftItem;
            OppositeHeldItem = RightItem;
        }
        else
        {
            Leg = FrontLeg;
            LegPosition = FrontLegRelativePosition;
            Arm = RightArm;
            OppositeArm = LeftArm;
            ArmPosition = RightArmRelativePosition;
            HeldItem = RightItem;
            OppositeHeldItem = LeftItem;
        }
        circularMotion *= side;
        if (circularMotion.y < 0)
        {
            circularMotion.y *= 0.1f;
        }
        Leg.transform.localPosition = LegPosition + circularMotion;
        Leg.transform.localRotation = (circularMotion.x * 0.3f + runningTilt * 0.25f).ToQuaternion();
        if (HeldItem.item.ChangeHoldAnimation)
        {
            Arm.transform.position = Body.transform.position + (Vector3)ArmPosition;
            Arm.transform.localRotation = (LeftArmRotationToCursor + Math.Abs(runningTilt)).ToQuaternion();
        }
        else
        {
            Arm.transform.localPosition = new Vector2(ArmPosition.x * Entity.Direction, ArmPosition.y) + circularMotion * 0.1f;
            Arm.transform.localRotation = (circularMotion.x * 0.5f).ToQuaternion();
        }
        if (Entity.Direction == side)
        {
            Arm.GetComponent<SpriteRenderer>().sortingOrder = Layer -1;
            OppositeArm.GetComponent<SpriteRenderer>().sortingOrder = Layer + 5;
            if (HeldItem.item.ChangeHoldAnimation)
            {
                if ((Entity.LookTarget - (Vector2)Arm.transform.position).x * side > 0)
                {
                    Arm.GetComponent<SpriteRenderer>().sortingOrder = Layer - 1;
                    HeldItem.GetComponent<SpriteRenderer>().sortingOrder = Layer - 2;
                }
                else
                {
                    Arm.GetComponent<SpriteRenderer>().sortingOrder = Layer + 7;
                    HeldItem.GetComponent<SpriteRenderer>().sortingOrder = Layer + 6;
                }
            }
            OppositeHeldItem.GetComponent<SpriteRenderer>().sortingOrder = Layer + 4;
        }
    }
    public void RotateHeadToCursor()
    {
        int direction = Entity.Direction;
        Vector2 toMouse = ToMouse;
        if (toMouse.x < 0)
        {
            direction *= -1;
        }
        else
        {
            direction *= 1;
        }
        toMouse = toMouse.normalized;
        toMouse.x = Mathf.Sign(toMouse.x) * 1; // * Entity.Direction;
        if (Entity.Direction == -1)
        {
            toMouse.x *= -1;
        }
        toMouse.y *= 0.4f;
        HeadRotationToCursor = (toMouse.ToRotation()).WrapAngle();
        Head.transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);
        Head.transform.rotation = (HeadRotationToCursor * Entity.Direction).ToQuaternion();
    }
    public void RotateArmToCursor(ref GameObject Arm, ref float RotationToCursor, Vector2 ArmPosition)
    {
        int direction = Entity.Direction;
        Vector2 toMouse = (Entity.LookTarget - (Vector2)Body.transform.position - ArmPosition);
        if (toMouse.x < 0)
        {
            direction *= -1;
        }
        else
        {
            direction *= 1;
        }
        toMouse = toMouse.normalized;
        if (Entity.Direction == -1)
        {
            toMouse.x *= -1;
        }
        float rotation = Mathf.PI / 2 + toMouse.ToRotation();
        RotationToCursor = rotation.WrapAngle();
        Arm.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
}
