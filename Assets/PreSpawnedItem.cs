using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PreSpawnedItem : MonoBehaviour
{
    public virtual ItemData SpawnItem => new FarmerGun();
    public virtual Vector2 SpawnVelocity()
    {
        Vector2 random = new Vector2(1, 0).RotatedBy(Random.Range(0, Mathf.PI * 2));
        return random;
    }
    private void Start()
    {
        ItemData.NewItem(SpawnItem, transform.position, SpawnVelocity());
        Destroy(this.gameObject);
    }
}
