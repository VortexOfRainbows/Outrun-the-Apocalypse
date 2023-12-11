using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Corn : ItemData
{
    public override string StatObjName => "Corn";
    public override string SpriteName => "Corn";
    public override Vector2 HandOffset => new Vector2(4f, -4.5f);
    public override bool Shoot(Entity player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        return false;
    }
    public override bool ConsumeAfterUsing(Entity player)
    {
        return true;
    }
    public override void OnUseItem(Entity player)
    {
        AudioManager.instance.Play("OmNom");
        player.Heal(Damage);
    }
}
