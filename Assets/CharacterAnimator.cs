using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

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
    public HeldItem LeftItem;
    [SerializeField]
    public HeldItem RightItem;
    Player Player => Player.MainPlayer;
    [SerializeField]
    List<GameObject> Limbs;
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
    private Vector2 ToMouse => (Utils.MouseWorld() - (Vector2)Head.transform.position);
    public void FlipAll(bool flipX)
    {
        Body.transform.localScale = new Vector3(flipX ? -1.0f : 1.0f, 1.0f, 1.0f);
    }
    public void PerformUpdate()
    {
        if (Player == null)
            return;
        FlipAll(Player.Direction == -1);
        RotateHeadToCursor();
        RotateArmToCursor(ref LeftArm, ref LeftArmRotationToCursor, LeftArmRelativePosition);
        RotateArmToCursor(ref RightArm, ref RightArmRotationToCursor, RightArmRelativePosition);
        LeftItem.item = Player.LeftHeldItem;
        RightItem.item = Player.RightHeldItem;
        LeftItem.transform.parent = LeftArm.transform;
        RightItem.transform.parent = RightArm.transform;
        Animate();
        LeftItem.ItemUpdate();
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
        //if (Player.Velocity.y < -0.0 && MathF.Abs(Player.Velocity.y) > 0.001f && MathF.Abs(Player.Velocity.x) < 0.001f)
        //    walkDirection = -1;
        float velocity = Player.Velocity.magnitude;
        float walkSpeedMultiplier = Mathf.Clamp(Math.Abs(velocity / 4f), 0, 1f);
        walkCounter += walkDirection * velocity * Mathf.Deg2Rad * walkSpeedMultiplier * WalkAnimSpeedMult;
        walkCounter = walkCounter.WrapAngle();
        //walkcounter *= walkSpeedMultiplier;
        Vector2 circularMotion = new Vector2(WalkAnimCircleMagnitude, 0).RotatedBy(-walkCounter) * walkSpeedMultiplier;
        circularMotion.y *= WalkAnimYMult;
        circularMotion.x *= WalkAnimXMult * walkSpeedMultiplier;
        float runningTilt = velocity * 0.0125f * walkSpeedMultiplier * Player.Direction;
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
            Arm.transform.localPosition = new Vector2(ArmPosition.x * Player.Direction, ArmPosition.y) + circularMotion * 0.1f;
            Arm.transform.localRotation = (circularMotion.x * 0.5f).ToQuaternion();
        }
        if (Player.Direction == side)
        {
            Arm.GetComponent<SpriteRenderer>().sortingOrder = -1;
            OppositeArm.GetComponent<SpriteRenderer>().sortingOrder = 5;
            if (HeldItem.item.ChangeHoldAnimation)
            {
                if ((Utils.MouseWorld() - (Vector2)Arm.transform.position).x * side > 0)
                {
                    Arm.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    HeldItem.GetComponent<SpriteRenderer>().sortingOrder = -2;
                }
                else
                {
                    Arm.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    HeldItem.GetComponent<SpriteRenderer>().sortingOrder = 6;
                }
            }
            OppositeHeldItem.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
    public void RotateHeadToCursor()
    {
        int direction = Player.Direction;
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
        toMouse.x = Mathf.Sign(toMouse.x) * 1; // * Player.Direction;
        if (Player.Direction == -1)
        {
            toMouse.x *= -1;
        }
        toMouse.y *= 0.4f;
        HeadRotationToCursor = (toMouse.ToRotation()).WrapAngle();
        Head.transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);
        Head.transform.rotation = (HeadRotationToCursor * Player.Direction).ToQuaternion();
    }
    public void RotateArmToCursor(ref GameObject Arm, ref float RotationToCursor, Vector2 ArmPosition)
    {
        int direction = Player.Direction;
        Vector2 toMouse = (Utils.MouseWorld() - (Vector2)Body.transform.position - ArmPosition);
        if (toMouse.x < 0)
        {
            direction *= -1;
        }
        else
        {
            direction *= 1;
        }
        toMouse = toMouse.normalized;
        if (Player.Direction == -1)
        {
            toMouse.x *= -1;
        }
        float rotation = Mathf.PI / 2 + toMouse.ToRotation();
        RotationToCursor = rotation.WrapAngle();
        Arm.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
}
