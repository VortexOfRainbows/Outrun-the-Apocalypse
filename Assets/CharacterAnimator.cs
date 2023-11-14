using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Limbs;
    public void InitValues()
    {
        Body = Limbs[0];
        Head = Limbs[1];
        Eyes = Limbs[2];
        Shadow = Limbs[3];
        LeftLeg = Limbs[4];
        RightLeg = Limbs[5];
        LeftArm = Limbs[6];
        RightArm = Limbs[7];
    }
    GameObject Body;
    GameObject Head;
    GameObject LeftLeg;
    GameObject RightLeg;
    GameObject LeftArm;
    GameObject RightArm;
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
        for (int i = 2; i <= 5; i++)
        {
            //skin.boneTransforms[i].transform.localRotation = Mathf.PI.ToQuaternion();
        }
        LeftLeg.transform.localPosition = new Vector3(-0.35f, 0.175f, 0.0f);
        RightLeg.transform.localPosition = new Vector3(-0.35f, -0.175f, 0.0f);
        LeftArm.transform.localPosition = new Vector3(0.2f, 0.40f, 0.0f);
        RightArm.transform.localPosition = new Vector3(0.2f, -0.35f, 0.0f);
        Head.transform.localPosition = new Vector3(0.4f, 0.0f, 0.0f);
    }
    public void Start()
    {
        InitLimbs();
    }
    float rotationToCursor = 0f;
    public void FlipAll(bool flipX)
    {
        Body.transform.localScale = new Vector3(flipX ? -1.0f : 1.0f, 1.0f, 1.0f);
    }
    public void PerformUpdate()
    {
        if (Player.MainPlayer == null)
            return;
        FlipAll(Player.MainPlayer.Direction == -1);
        RotateHeadToCursor();
        WalkAnimation();
        //Player.MainPlayer.LeftGun.transform.parent = LeftArm.transform;
        //Player.MainPlayer.LeftGun.UpdatePosition(Player.MainPlayer.Direction == -1);
    }
    float walkSpeedMultiplier = 0.0f;
    float walkcounter = 0;
    public void WalkAnimation()
    {
        float walkDirection = 1f;
        //if (Player.MainPlayer.Velocity.y < -0.0 && MathF.Abs(Player.MainPlayer.Velocity.y) > 0.001f && MathF.Abs(Player.MainPlayer.Velocity.x) < 0.001f)
        //    walkDirection = -1;
        float velocity = Player.MainPlayer.Velocity.magnitude;
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
        float runningTilt = velocity * 0.0125f * walkSpeedMultiplier * Player.MainPlayer.Direction;
        LeftLeg.transform.localPosition = new Vector2(2f, -4f) + circularMotion;
        LeftLeg.transform.localRotation = (circularMotion.x * 0.3f + runningTilt * 0.25f).ToQuaternion();
        RightLeg.transform.localPosition = new Vector2(-2f, -4f) + inverseCM;
        RightLeg.transform.localRotation = (inverseCM.x * 0.3f + runningTilt * 0.25f).ToQuaternion();

        LeftArm.transform.localPosition = new Vector2(4.5f, 2.5f) + inverseCM * 0.1f;
        LeftArm.transform.localRotation = (inverseCM.x * 0.75f).ToQuaternion();
        RightArm.transform.localPosition = new Vector2(-4.5f, 2.5f) + circularMotion * 0.1f;
        RightArm.transform.localRotation = (circularMotion.x * 0.75f).ToQuaternion();

        float bobbingMotion = 0.5f + 0.5f * (float)MathF.Cos(walkcounter * 2);

        Head.transform.localPosition = new Vector3(0, 6 - bobbingMotion * walkSpeedMultiplier);
        Head.transform.rotation = (rotationToCursor * Player.MainPlayer.Direction).ToQuaternion();
        Body.transform.localRotation = (-runningTilt).ToQuaternion();
        Eyes.transform.localPosition = new Vector3(2, 5, 0);
        Shadow.transform.localPosition = new Vector3(0.5f, 5.5f, 0);
    }
    public void RotateHeadToCursor()
    {
        int direction = Player.MainPlayer.Direction;
        Vector2 toMouse = (Utils.MouseWorld() - (Vector2)transform.position);
        if (toMouse.x < 0)
        {
            direction *= -1;
        }
        else
        {
            direction *= 1;
        }
        toMouse = toMouse.normalized;
        toMouse.x = Mathf.Sign(toMouse.x) * 1; // * Player.MainPlayer.Direction;
        if (Player.MainPlayer.Direction == -1)
        {
            toMouse.x *= -1;
        }
        toMouse.y *= 0.4f;
        rotationToCursor = (toMouse.ToRotation()).WrapAngle();
        Head.transform.localScale = new Vector3(transform.localScale.x, direction, transform.localScale.z);
    }
}
