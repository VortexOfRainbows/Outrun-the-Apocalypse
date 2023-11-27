using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Entity
{
    [SerializeField]
    private CharacterAnimator CharacterAnimator;
    [SerializeField]
    private GameObject Player;
    // Start is called before the first frame update
    public override int LayerDefaultPosition => -20;
    public override float ArmDegreesOffset => 90;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (LeftHeldItem == null)
            LeftHeldItem = new FarmerGun();
        if (RightHeldItem == null)
            RightHeldItem = new NoItem();
        Velocity *= 0.1f; //using velocity to update position because it helps instruct the animator what to do in order to animate the zombie
        if (transform.position.x < Player.transform.position.x)
        {
            Velocity.x += 1; //Increase velocity by 1 to the right, since the player is right of the zombie
        }
        if (transform.position.x > Player.transform.position.x)
        {
            Velocity.x -= 1;
        }
        if (transform.position.y < Player.transform.position.y)
        {
            Velocity.y += 1;
        }
        if (transform.position.y > Player.transform.position.y)
        {
            Velocity.y -= 1;
        }

        if (Velocity.x > 0)
            Direction = 1;
        else
            Direction = -1;
        rb.velocity = Velocity;

        LookTarget = Player.transform.position;
        CharacterAnimator.PerformUpdate();

        LastDirection = Direction;
    }
}
