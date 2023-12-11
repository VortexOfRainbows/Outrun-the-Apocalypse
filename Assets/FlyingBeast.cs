using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyBehavior : Entity
{
    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject LeftWing;
    [SerializeField] private GameObject RightWing;
    [SerializeField] private float InertiaPercent = 0.8f;
    [SerializeField] private float movespeedMultiplier = 2f;
    [SerializeField]
    private float EntitySpeed;
    [SerializeField]
    private int MinTimeLimit;
    [SerializeField]
    private int MaxTimeLimit;

    private int NextSpawnTime;
    private int timer;
    [SerializeField] private int ProjectileSpeed = 8;
    [SerializeField] private float HoverDistance = 120;
    [SerializeField] private float moveSpeedHoverMult = 0.2f;
    [SerializeField] private float SpeedMultiplierOutOfRange = 3f;
    [SerializeField] private float ActivateOutOfRangeDistance = 240f;
    public override void SetStats()
    {
        NextSpawnTime = Random.Range(MinTimeLimit, MaxTimeLimit);
        MaxLife = 11;
        Life = MaxLife;
        ContactDamage = 10;
        Friendly = false;
    }
    public override void OnFixedUpdate()
    {
        GameObject player = Player.MainPlayer.gameObject;
        Vector2 toPlayer = (player.transform.position - this.transform.position);
        Velocity *= InertiaPercent;
        float speedMult = movespeedMultiplier;
        float length = toPlayer.magnitude;
        if (length < HoverDistance)
        {
            speedMult *= moveSpeedHoverMult;
        }
        else if(length > ActivateOutOfRangeDistance)
        {
            speedMult *= SpeedMultiplierOutOfRange;
        }
        Velocity += toPlayer.normalized * speedMult * (1 - InertiaPercent);
        Velocity *= EnemyScalingFactor;
        rb.velocity = Velocity;

        timer++;
        if (timer >= NextSpawnTime)
        {
            toPlayer = toPlayer.normalized * ProjectileSpeed;
            AudioManager.instance.Play("BeastBlast");
            ProjectileData.NewProjectile(this, new BeastBlast(), transform.position, toPlayer, (int)ContactDamage);
            timer = 0;
        }
        HandleAnimation();
    }
    [SerializeField] private Vector2 LeftWingRelativePosition = new Vector2(-14.5f, 2.5f);
    [SerializeField] private Vector2 RightWingRelativePosition = new Vector2(14.5f, 2.5f);
    [SerializeField] private float FlapSpeed = 10;
    [SerializeField] private float FlapDegrees = 15;
    [SerializeField] private float FlapFactor = 0.05f;
    private float FlapCounter = 0;
    public void HandleAnimation()
    {
        FlapCounter += FlapSpeed;
        float sinusoid = Mathf.Sin(FlapCounter * Mathf.Deg2Rad);
        transform.position = new Vector3(transform.position.x, transform.position.y - sinusoid * FlapFactor);
        GameObject player = Player.MainPlayer.gameObject;
        Vector2 toPlayer = (player.transform.position - this.transform.position);
        int direction = 1;
        if (toPlayer.x < 0)
        {
            direction *= -1;
        }
        else
        {
            direction *= 1;
        }
        float radians = sinusoid * FlapDegrees * Mathf.Deg2Rad;
        Head.transform.rotation = (toPlayer.ToRotation() + (direction == -1 ? Mathf.PI : 0)).ToQuaternion();
        Head.transform.localScale = new Vector3(direction, 1, 1);
        LeftWing.transform.localPosition = LeftWingRelativePosition.RotatedBy(radians);
        RightWing.transform.localPosition = RightWingRelativePosition.RotatedBy(radians * -1);
        LeftWing.transform.localRotation = (-(Vector2)LeftWing.transform.localPosition).ToRotation().ToQuaternion();
        RightWing.transform.localRotation = ((Vector2)RightWing.transform.localPosition).ToRotation().ToQuaternion();
    }
    [SerializeField] private float PotatoGunDropChance = 0.001f;
    public override void OnDeath()
    {
        AudioManager.instance.Play("ZombieDeath");
        GameObject coin = Instantiate(PrefabManager.GetPrefab("coin"), transform.position, new Quaternion());
        coin.GetComponent<Coin>().DespawnCounter = 0;
        if(PotatoGunDropChance > Random.Range(0, 1f))
        {
            ItemData.NewItem(new PotatoGun(), transform.position, new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f)));
        }
    }
    [SerializeField] private int GoreDropVelo = -6;
    public override void GenerateGore()
    {
        Gore.NewGore(Head, new Vector2(0, GoreDropVelo));
        Gore.NewGore(LeftWing, new Vector2(0, GoreDropVelo));
        Gore.NewGore(RightWing, new Vector2(0, GoreDropVelo));
    }
}
