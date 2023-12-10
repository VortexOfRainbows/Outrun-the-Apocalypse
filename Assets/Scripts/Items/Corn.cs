using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Corn : ItemData
{
    public override string StatObjName => "Corn";
    public override string SpriteName => "Corn";
    public override Vector2 HandOffset => new Vector2(2.5f, -5.5f);
    public override bool Shoot(Player player, ref Vector2 position, ref Vector2 velocity, ref int damage)
    {
        return false;
    }
    public override bool ConsumeAfterUsing(Player player)
    {
        return true;
    }
    public override void OnUseItem(Player player)
    {
        player.Heal(Damage);
    }
}
