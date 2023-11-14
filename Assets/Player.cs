using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player MainPlayer;
    public struct ControlDown
    {
        public bool DashClick;
        public bool DashTap;
        public bool Left;
        public bool Right;
        public bool Up;
        public bool Down;
        public bool Jump;
        public bool Shift;
        public ControlDown(bool defaultState = false)
        {
            DashClick = DashTap = Left = Right = Up = Down = Jump = Shift = defaultState;
        }
    }
    [SerializeField]
    public GameObject Sprite;
    public ControlDown Control = new ControlDown();
    public ControlDown LastControl = new ControlDown();
    public readonly Vector2 ColliderSize = new Vector2(0.4f, 0.9f);
    public int Direction = 0;
    public int LastDirection = 0;
    [SerializeField]
    public BoxCollider2D Collider2D;
    [SerializeField]
    public Rigidbody2D rb;
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    private Vector2 v
    {
        get
        {
            return rb.velocity;
        }
        set
        {
            rb.velocity = value;
        }
    }
    public Vector2 LastPosition = Vector2.zero;
    public Vector2 PrevVelocity = Vector2.zero;
    public Vector2 Velocity;
    public const float MovementAcceleration = 0.65f;
    public const float MovementDeacceleration = 0.5f; //Could seperate this into more variables later. Such as for Air based acceleration. Really just depends on what we want to change
    public const float BaseMaxMoveSpeed = 8f;
    public float MaxMoveSpeed = 8f;
    public float RotationOffset;
    // Start is called before the first frame update
    void Awake()
    {
        if (MainPlayer == null)
        {
            MainPlayer = this;
            DontDestroyOnLoad(MainPlayer);
        }
        else
        {
            if (MainPlayer != this)
            {
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        if (MainPlayer == null)
        {
            MainPlayer = this;
            DontDestroyOnLoad(MainPlayer);
        }
        else
        {
            if (MainPlayer != this)
            {
                Destroy(gameObject);
            }
        }
        RegisterControls();
        // mainCamera.transform.position = new Vector3(Position.x, Position.y, mainCamera.transform.position.z); // Now using Cinemachine for camera following
    }
    void FixedUpdate()
    {
        //TouchingCollider = false;
        Velocity = v;
        ControlUpdate();
        Physics();
        v = Velocity;
        PrevVelocity = v;
        LastDirection = Direction;
        LastPosition = Position;
    }
    /// <summary>
    /// Performs calculations for acceleration, deacceleration, rotation, etc. Velocity is applied to the local property, which is then applied to the rigid body to perform the movement.
    /// </summary>
    public void Physics()
    {
        float BonusMaxSpeedResolveFactor = 0.04f;
        float SpinResolveFactor = 0.08f;

        MaxMoveSpeed = Mathf.Lerp(MaxMoveSpeed, BaseMaxMoveSpeed, BonusMaxSpeedResolveFactor);
        float currentSpeed = Velocity.magnitude;
        float topSpeed = Math.Clamp(currentSpeed, 0, MaxMoveSpeed);
        Vector2 velo = Velocity.normalized * topSpeed;
        Velocity = velo;

        RotationOffset = Mathf.Lerp(RotationOffset, 0, SpinResolveFactor);
        if (RotationOffset != 0)
        {
            if (Math.Abs(RotationOffset) <= Mathf.Deg2Rad * 0.1f)
            {
                RotationOffset = 0;
            }
            else
            {
                Collider2D.size = new Vector2(ColliderSize.x, Mathf.Lerp(ColliderSize.x, ColliderSize.y, Math.Abs(Mathf.Cos(RotationOffset))));
            }
        }
    }
    /// <summary>
    /// Checks which controls are pressed down by the player, then acts on them
    /// </summary>
    public void ControlUpdate()
    {
        if (Control.Left && !Control.Right)
        {
            if (Velocity.x > 0)
                Velocity.x *= MovementDeacceleration;
            Velocity -= new Vector2(MovementAcceleration, 0);
        }
        else if (Control.Right && !Control.Left)
        {
            if (Velocity.x < 0)
                Velocity.x *= MovementDeacceleration;
            Velocity += new Vector2(MovementAcceleration, 0);
        }
        else
        {
            Velocity.x *= MovementDeacceleration;
        }
        if (Control.Up && !Control.Down)
        {
            if (Velocity.y < 0)
                Velocity.y *= MovementDeacceleration;
            Velocity += new Vector2(0, MovementAcceleration);
        }
        else if (Control.Down && !Control.Up)
        {
            if (Velocity.y > 0)
                Velocity.y *= MovementDeacceleration;
            Velocity -= new Vector2(0, MovementAcceleration);
        }
        else
        {
            Velocity.y *= MovementDeacceleration;
        }
        if(Control.Shift)
        {
            MaxMoveSpeed = BaseMaxMoveSpeed / 4.0f;
        }
        PostControlUpdate();
    }
    public void PostControlUpdate()
    {
        LastControl = Control;
        if(Control.Right && !Control.Left)
            Direction = 1;
        if(Control.Left && !Control.Right)
            Direction = -1;
        PlayerDrawing Drawing = GetComponentInChildren<PlayerDrawing>();
        Drawing.PerformUpdate();
    }
    private void RegisterControls()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (!Control.Right)
                Control.Left = true;
        }
        else
        {
            if (LastControl.Left)
            {
                Control.Left = false;
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!Control.Left)
                Control.Right = true;
        }
        else
        {
            if (LastControl.Right)
            {
                Control.Right = false;
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (!Control.Down)
                Control.Up = true;
        }
        else
        {
            if (LastControl.Up)
            {
                Control.Up = false;
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (!Control.Up)
                Control.Down = true;
        }
        else
        {
            if (LastControl.Down)
            {
                Control.Down = false;
            }
        }
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Control.Jump = true;
        }
        else
        {
            if (LastControl.Jump)
                Control.Jump = false;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Control.Shift = true;
        }
        else
        {
            if (LastControl.Shift)
                Control.Shift = false;
        }
    }
}
