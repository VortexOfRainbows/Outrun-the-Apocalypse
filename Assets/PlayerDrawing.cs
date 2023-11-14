using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class PlayerDrawing : MonoBehaviour
{
    SpriteSkin skin => GetComponent<SpriteSkin>();
    Transform Body => skin.boneTransforms[0];
    Transform Head => skin.boneTransforms[1];
    Transform LegLeft => skin.boneTransforms[2];
    Transform LegRight => skin.boneTransforms[3];
    Transform ArmLeft => skin.boneTransforms[4];
    Transform ArmRight => skin.boneTransforms[5];
    Transform Shadow => skin.boneTransforms[6];
    Transform Eyes => skin.boneTransforms[7];
    public void InitLimbs()
    {
        for (int i = 0; i < skin.boneTransforms.Length; i++)
        {
            skin.boneTransforms[i].rotation = 0.0f.ToQuaternion();
            skin.boneTransforms[i].localPosition = Vector3.zero;
        }
        Body.rotation = (Mathf.PI * 0.5f).ToQuaternion();
        for (int i = 2; i <= 5; i++)
        {
            skin.boneTransforms[i].localRotation = Mathf.PI.ToQuaternion();
        }
        LegLeft.localPosition = new Vector3(-0.35f, 0.175f, 0.0f);
        LegRight.localPosition = new Vector3(-0.35f, -0.175f, 0.0f);
        ArmLeft.localPosition = new Vector3(0.2f, 0.40f, 0.0f);
        ArmRight.localPosition = new Vector3(0.2f, -0.35f, 0.0f);
        Head.localPosition = new Vector3(0.4f, 0.0f, 0.0f);
        Eyes.localPosition = Head.localPosition - new Vector3(1 / 48f, 8 / 48f, 0);
        Shadow.localPosition = Head.localPosition + new Vector3(12 / 48f, 0, 0);
    }
    public void Start()
    {
        InitLimbs();
    }
    float rotationToCursor = 0f;
    public void PerformUpdate()
    {
        if (Player.MainPlayer == null)
            return;
        GetComponent<SpriteRenderer>().flipX = Player.MainPlayer.Direction == -1;
        RotateHeadToCursor();
        WalkAnimation();
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
        Vector2 circularMotion = new Vector2(0.5f, 0).RotatedBy(-walkcounter) * walkSpeedMultiplier;
        circularMotion.x *= 0.275f;
        circularMotion.y *= 0.225f * walkSpeedMultiplier;
        Vector2 inverseCM = -circularMotion;
        if(circularMotion.x < 0)
        {
            circularMotion.x *= 0.1f;
        }
        if (inverseCM.x < 0)
        {
            inverseCM.x *= 0.1f;
        }
        float runningTilt = velocity * 0.015f * walkSpeedMultiplier;
        LegLeft.localPosition = new Vector2(-0.35f, 0.175f) + circularMotion;
        LegLeft.localRotation = (Mathf.PI - circularMotion.y * 3.5f + runningTilt).ToQuaternion();
        LegRight.localPosition = new Vector2(-0.35f, -0.175f) + inverseCM;
        LegRight.localRotation = (Mathf.PI - inverseCM.y * 3.5f + runningTilt).ToQuaternion();

        ArmLeft.localPosition = new Vector2(0.2f, 0.4f) + inverseCM * 0.1f;
        ArmLeft.localRotation = (Mathf.PI - inverseCM.y * 10f).ToQuaternion();
        ArmRight.localPosition = new Vector2(0.2f, -0.35f) + circularMotion * 0.1f;
        ArmRight.localRotation = (Mathf.PI - circularMotion.y * 10f).ToQuaternion();

        float bobbingMotion = 0.5f + 0.5f * (float)MathF.Cos(walkcounter * 2);

        Head.localPosition = new Vector3(0.4f + bobbingMotion * 4 / 48f * walkSpeedMultiplier, 0);
        Head.localRotation = (rotationToCursor + runningTilt).ToQuaternion();
        Body.localRotation = (Mathf.PI * 0.5f - runningTilt).ToQuaternion();
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
        if(Player.MainPlayer.Direction == -1)
        {
            toMouse.x *= -1;
            direction *= -1;
        }
        toMouse.y *= 0.4f;
        rotationToCursor = (toMouse.ToRotation()).WrapAngle();
        if (direction == -1)
            rotationToCursor -= Mathf.PI;
        Head.transform.localScale = new Vector3(Player.MainPlayer.Direction, direction, transform.transform.localScale.z);
    }
}
