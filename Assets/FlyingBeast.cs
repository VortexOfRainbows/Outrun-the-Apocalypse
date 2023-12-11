using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemyBehavior : Entity
{
    [SerializeField]
    private float EntitySpeed;
    [SerializeField]
    private int MinTimeLimit;
    [SerializeField]
    private int MaxTimeLimit;

    private int NextSpawnTime;
    private int timer;
    public override void SetStats()
    {
        NextSpawnTime = Random.Range(MinTimeLimit, MaxTimeLimit);
        MaxLife = 12;
        Life = MaxLife;
        ContactDamage = 10;
        Friendly = false;
    }
    public override void OnFixedUpdate()
    {
        GameObject player = Player.MainPlayer.gameObject;
        Vector2 velocity = (player.transform.position - this.transform.position).normalized * 8;
        Velocity *= 0.1f;
        if (transform.position.x < player.transform.position.x - 105) //move in from the left
        {
            Velocity.x += EntitySpeed;
        }
        if (transform.position.x > player.transform.position.x - 95 && transform.position.x < player.transform.position.x)
        {
            Velocity.x -= EntitySpeed;
        }
        if (transform.position.x > player.transform.position.x + 105) //move in from the right
        {
            Velocity.x -= EntitySpeed;
        }
        if (transform.position.x > player.transform.position.x && transform.position.x < player.transform.position.x + 95) //if player is too close
        {
            Velocity.x += EntitySpeed;
        }
        if (transform.position.y < player.transform.position.y - 3)
        {
            Velocity.y += EntitySpeed;
        }
        if (transform.position.y > player.transform.position.y + 3)
        {
            Velocity.y -= EntitySpeed;
        }
        Velocity *= EnemyScalingFactor;
        rb.velocity = Velocity;
        timer += 1;
        if (timer == NextSpawnTime)
        {
            ProjectileData.NewProjectile(this, new BeastBlast(), transform.position, velocity, 10);
            timer = 0;
        }
    }
}
