using System;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Player : EntityWithCharDrawing
{
    public bool Dead => UIManager.GameEnd;
    [SerializeField]
    private CharacterAnimator CharacterAnimator;
    [SerializeField]
    private Camera MainCamera;
    public static Player MainPlayer;
    public struct ControlDown
    {
        public bool LeftClick;
        public bool RightClick;
        public bool DashTap;
        public bool Left;
        public bool Right;
        public bool Up;
        public bool Down;
        public bool Jump;
        public bool Shift;
        public ControlDown(bool defaultState = false)
        {
            LeftClick = DashTap = Left = Right = Up = Down = Jump = Shift = defaultState;
            RightClick = defaultState;
        }
    }
    public ControlDown Control = new ControlDown();
    public ControlDown LastControl = new ControlDown();
    public readonly Vector2 ColliderSize = new Vector2(0.4f, 0.9f);
    [SerializeField]
    public BoxCollider2D Collider2D;
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
    public const float MovementAcceleration = 0.65f;
    public const float MovementDeacceleration = 0.5f; //Could seperate this into more variables later. Such as for Air based acceleration. Really just depends on what we want to change
    public const float BaseMaxMoveSpeed = 6f;
    public float MaxMoveSpeed = 6f;
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
    public override void SetStats()
    {
        MaxLife = 100;
        Life = 100;
        DefaultImmunityOnHit = 30;
        Friendly = true;
    }
    public override void OnUpdate()
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
        if (Life > 0)
            UIManager.instance.PlayGame();
    }
    public override void OnFixedUpdate()
    {
        //this.HealthUI.SetActive(true);
        if (LeftHeldItem == null)
            LeftHeldItem = new NoItem();
        if (RightHeldItem == null)
            RightHeldItem = new NoItem();
        //This should be moved somewhere where it would make more sense
        Physics2D.Simulate(0.25f); //Timestep is 0.25f because one unit is 4 pixels. Therefore this will move convert our movement to 1 velocity per update = 1 pixel per update
        
        Velocity = v;
        Physics();
        if(!Dead)
        {
            ControlUpdate();
            InventoryUpdate();
            ItemUpdate();
        }
        FinalUpdate();
        v = Velocity;
        PrevVelocity = v;
        LastDirection = Direction;
        LastPosition = Position;

        MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, new Vector3(Position.x, Position.y + 4, MainCamera.transform.position.z), 0.06f);
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
        if (Control.Right && !Control.Left)
            Direction = 1;
        if (Control.Left && !Control.Right)
            Direction = -1;
        else if (Direction == 0)
            Direction = 1;
    }
    public void ItemUpdate()
    {
        LookTarget = Utils.MouseWorld();
        CharacterAnimator.PerformUpdate();
        if (Control.LeftClick)
        {
            if (!LastControl.LeftClick || LeftHeldItem.HoldClick)
            {
                LeftHeldItem.UseItem(this, CharacterAnimator.LeftItem);
            }
        }
        if (Control.RightClick)
        {
            if (!LastControl.RightClick || RightHeldItem.HoldClick)
            {
                RightHeldItem.UseItem(this, CharacterAnimator.RightItem);
            }
        }
    }
    public void FinalUpdate()
    {
        LastControl = Control;
    }
    private void RegisterControls()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!Control.Right)
                Control.Left = true;
        }
        else if (LastControl.Left)
                Control.Left = false;

        if (Input.GetKey(KeyCode.D))
        {
            if (!Control.Left)
                Control.Right = true;
        }
        else if (LastControl.Right)
            Control.Right = false;

        if (Input.GetKey(KeyCode.W))
        {
            if (!Control.Down)
                Control.Up = true;
        }
        else if (LastControl.Up)
            Control.Up = false;

        if (Input.GetKey(KeyCode.S))
        {
            if (!Control.Up)
                Control.Down = true;
        }
        else if (LastControl.Down)
            Control.Down = false;

        UpdateKey(Input.GetKey(KeyCode.Space), LastControl.Jump, ref Control.Jump);
        UpdateKey(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift), LastControl.Shift, ref Control.Shift);
        UpdateKey(Input.GetMouseButton(0), LastControl.LeftClick, ref Control.LeftClick);
        UpdateKey(Input.GetMouseButton(1), LastControl.RightClick, ref Control.RightClick);
    }
    public void UpdateKey(bool AssociatedInput, bool LastControl, ref bool ControlToUpdate)
    {
        if (AssociatedInput)
        {
            ControlToUpdate = true;
        }
        else
        {
            if (LastControl)
                ControlToUpdate = false;
        }
    }
    [SerializeField] public Inventory Inventory;
    private const int LeftHandSlotNum = 4;
    public const int RightHandSlotNum = 5;
    public void InventoryUpdate()
    {
        LeftHeldItem = Inventory.Slot[LeftHandSlotNum].Item;
        RightHeldItem = Inventory.Slot[RightHandSlotNum].Item;
    }
    /// <summary>
    /// Adds an item to the inventory. Returns false if there is no room in the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItemToInventory(ItemData item)
    {
        if (Inventory.Slot[LeftHandSlotNum].TryAddingItem(item))
            return true;
        if (Inventory.Slot[RightHandSlotNum].TryAddingItem(item))
            return true;
        for (int i = 0; i < Inventory.Slot.Count; i++)
        {
            if (Inventory.Slot[i].TryAddingItem(item))
                return true;
        }
        return false;
    }
}
