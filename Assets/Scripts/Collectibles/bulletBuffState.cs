using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBuffState : MonoBehaviour
{
    public GameObject projectile;
    public MortarCannon mortarCannon;

    public float rangeMultiplier;
    public float bulletSizeMultiplier;

    public bool collectedBuff;

    public int duration;

    private void Update()
    {
        if (duration < 0)
        {
            collectedBuff = false;
            bulletSizeMultiplier = 1f;
        }
        else if (duration > 0)
        {
            //projectile.transform.localScale *= bulletSizeMultiplier;
            duration--;
        }
    }



}
