using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerGun : MonoBehaviour
{
    public void UpdatePosition(bool Flip = false)
    {
        GetComponent<SpriteRenderer>().flipY = Flip;
        Vector2 GunOffsetFromArm = new Vector2(0.4f, 0f);
        transform.localPosition = GunOffsetFromArm;
    }
}
