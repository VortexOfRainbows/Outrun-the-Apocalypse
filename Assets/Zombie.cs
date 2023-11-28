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
    public override void SetStats()
    {
        MaxLife = 25;
        Life = 25;
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
        if (transform.position.x < player.transform.position.x)
        {
            Velocity.x += 1; //Increase velocity by 1 to the right, since the player is right of the zombie
        }
        if (transform.position.x > player.transform.position.x)
        {
            Velocity.x -= 1;
        }
        if (transform.position.y < player.transform.position.y)
        {
            Velocity.y += 1;
        }
        if (transform.position.y > player.transform.position.y)
        {
            Velocity.y -= 1;
        }

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
        Instantiate(PrefabManager.GetPrefab("coin"), transform.position, new Quaternion());
    }
}
