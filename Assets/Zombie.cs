using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : EntityWithCharDrawing
{
    [SerializeField]
    private CharacterAnimator CharacterAnimator;
    private GameObject player;
    public override int LayerDefaultPosition => -20;
    public override float ArmDegreesOffset => 90;
    public Vector2 DrunkVelocity; //A random velocity offset that is constantly added to the zombie in order to tilt it
    public bool FollowDirect; //Whether the zombies follows directly, or follows with a square pattern
    public override void SetStats()
    {
        FollowDirect = Random.Range(0.0f, 1.0f) < 0.5;
        DrunkVelocity = new Vector2(0.3f, 0.0f).RotatedBy(Random.Range(0, Mathf.PI * 2));
        MaxLife = 18;
        Life = 18;
        ContactDamage = 15;
        Friendly = false;
    }
    public override void OnFixedUpdate()
    {
        if (player == null) 
            player = Player.MainPlayer.gameObject;
        if (LeftHeldItem == null)
            LeftHeldItem = new NoItem();
        if (RightHeldItem == null)
            RightHeldItem = new NoItem();
        Velocity *= 0.1f; //using velocity to update position because it helps instruct the animator what to do in order to animate the zombie
        if(FollowDirect)
        {
            Vector2 toPlayer = player.transform.position - transform.position;
            Velocity += toPlayer.normalized;
        }
        else
        {
            if (transform.position.x < player.transform.position.x - 5) //These 5 are simply in place to prevent the zombie from jittering in its movements. They don't really need to be variables because this number is mostly inconsequential otherwise.
            {
                Velocity.x += 1; //Increase velocity by 1 to the right, since the player is right of the zombie
            }
            if (transform.position.x > player.transform.position.x + 5)
            {
                Velocity.x -= 1;
            }
            if (transform.position.y < player.transform.position.y - 5)
            {
                Velocity.y += 1;
            }
            if (transform.position.y > player.transform.position.y + 5)
            {
                Velocity.y -= 1;
            }
        }
        Velocity += DrunkVelocity;
        Velocity *= EnemyScalingFactor;
        if (Velocity.x > 0)
            Direction = 1;
        else
            Direction = -1;
        rb.velocity = Velocity;

        LookTarget = player.transform.position;
        CharacterAnimator.PerformUpdate();

        LastDirection = Direction;
    }
    public override void OnDeath()
    {
        AudioManager.instance.Play("ZombieDeath");
        GameObject coiny = Instantiate(PrefabManager.GetPrefab("coin"), transform.position, new Quaternion());
        coiny.GetComponent<Coin>().DespawnCounter = 0;
    }
}
