using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    private HeldItem LeftItem;
    [SerializeField]
    private HeldItem RightItem;
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
        RotateLeftArmToCursor();
        RotateRightArmToCursor();
        LeftItem.item = Player.LeftHeldItem;
        RightItem.item = Player.RightHeldItem;
        LeftItem.transform.parent = LeftArm.transform;
        RightItem.transform.parent = RightArm.transform;
        Animate();
        LeftItem.ItemUpdate();
        RightItem.ItemUpdate();
    }
    float walkSpeedMultiplier = 0.0f;
    float walkcounter = 0;
    public void Animate()
    {
        float walkDirection = 1f;
        //if (Player.Velocity.y < -0.0 && MathF.Abs(Player.Velocity.y) > 0.001f && MathF.Abs(Player.Velocity.x) < 0.001f)
        //    walkDirection = -1;
        float velocity = Player.Velocity.magnitude;
        walkSpeedMultiplier = Mathf.Clamp(Math.Abs(velocity / 4f), 0, 1f);
        walkcounter += walkDirection * velocity * Mathf.Deg2Rad * walkSpeedMultiplier * 2.2f;
        walkcounter = walkcounter.WrapAngle();
        //walkcounter *= walkSpeedMultiplier;
        Vector2 circularMotion = new Vector2(7f, 0).RotatedBy(-walkcounter) * walkSpeedMultiplier;
        circularMotion.y *= 0.25f;
        circularMotion.x *= 0.15f * walkSpeedMultiplier;
        Vector2 inverseCM = -circularMotion;
        if (circularMotion.y < 0)
        {
            circularMotion.y *= 0.1f;
        }
        if (inverseCM.y < 0)
        {
            inverseCM.y *= 0.1f;
        }
        float runningTilt = velocity * 0.0125f * walkSpeedMultiplier * Player.Direction;
        BackLeg.transform.localPosition = new Vector2(2f, -4f) + circularMotion;
        BackLeg.transform.localRotation = (circularMotion.x * 0.3f + runningTilt * 0.25f).ToQuaternion();
        FrontLeg.transform.localPosition = new Vector2(-2f, -4f) + inverseCM;
        FrontLeg.transform.localRotation = (inverseCM.x * 0.3f + runningTilt * 0.25f).ToQuaternion();

        RightArm.transform.localPosition = new Vector2(4.5f * Player.Direction, 2.5f) + inverseCM * 0.1f;
        RightArm.transform.localRotation = (inverseCM.x * 0.5f).ToQuaternion();
        float bobbingMotion = 0.5f + 0.5f * (float)MathF.Cos(walkcounter * 2);

        Head.transform.localPosition = new Vector3(0, 6 - bobbingMotion * walkSpeedMultiplier);
        Body.transform.localRotation = (-runningTilt).ToQuaternion();
        Eyes.transform.localPosition = new Vector3(2, 5, 0);
        Shadow.transform.localPosition = new Vector3(0.5f, 5.5f, 0);


        if (LeftItem.item.ChangeHoldAnimation)
        {
            LeftArm.transform.position = Body.transform.position + new Vector3(-4.5f, 2.5f);
            LeftArm.transform.localRotation = (LeftArmRotationToCursor + Math.Abs(runningTilt)).ToQuaternion();
        }
        else
        {
            LeftArm.transform.localPosition = new Vector2(-4.5f * Player.Direction, 2.5f) + circularMotion * 0.1f;
            LeftArm.transform.localRotation = (circularMotion.x * 0.5f).ToQuaternion();
        }

        if (RightItem.item.ChangeHoldAnimation)
        {
            RightArm.transform.position = Body.transform.position + new Vector3(4.5f, 2.5f);
            RightArm.transform.localRotation = (RightArmRotationToCursor + Math.Abs(runningTilt)).ToQuaternion();
        }
        else
        {
            RightArm.transform.localPosition = new Vector2(4.5f * Player.Direction, 2.5f) + circularMotion * 0.1f;
            RightArm.transform.localRotation = (circularMotion.x * 0.5f).ToQuaternion();
        }
        if (Player.Direction == -1)
        {
            LeftArm.GetComponent<SpriteRenderer>().sortingOrder = -1;
            RightArm.GetComponent<SpriteRenderer>().sortingOrder = 5;
            if (LeftItem.item.ChangeHoldAnimation)
            {
                if ((Utils.MouseWorld() - (Vector2)LeftArm.transform.position).x < 0)
                {
                    LeftArm.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    LeftItem.GetComponent<SpriteRenderer>().sortingOrder = -2;
                }
                else
                {
                    LeftArm.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 6;
                }
            }
            RightItem.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        else
        {
            RightArm.GetComponent<SpriteRenderer>().sortingOrder = -1;
            LeftArm.GetComponent<SpriteRenderer>().sortingOrder = 5;
            if (RightItem.item.ChangeHoldAnimation)
            {
                if ((Utils.MouseWorld() - (Vector2)RightArm.transform.position).x > 0)
                {
                    RightArm.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    RightItem.GetComponent<SpriteRenderer>().sortingOrder = -2;
                }
                else
                {
                    RightArm.GetComponent<SpriteRenderer>().sortingOrder = 7;
                    RightItem.GetComponent<SpriteRenderer>().sortingOrder = 6;
                }
            }
            LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 4;
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
    public void RotateLeftArmToCursor()
    {
        int direction = Player.Direction;
        Vector2 toMouse = (Utils.MouseWorld() - (Vector2)Body.transform.position + new Vector2(4.5f, -2.5f));
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
        LeftArmRotationToCursor = rotation.WrapAngle();
        LeftArm.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
    public void RotateRightArmToCursor()
    {
        int direction = Player.Direction;
        Vector2 toMouse = (Utils.MouseWorld() - (Vector2)Body.transform.position + new Vector2(-4.5f, -2.5f));
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
        RightArmRotationToCursor = rotation.WrapAngle();
        RightArm.transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }
}
