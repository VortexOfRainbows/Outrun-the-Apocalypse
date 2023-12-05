using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Consumables : ScriptableObject
{
    public new string name;

    public bool changeCharacterSize;
    public bool changeDamage;
    public bool changeRange;
    public bool changeBulletSize;
    public bool changeSpeed;
    public bool changeHealth;

    public int buffduration;

    public int hpincrease;

    public float playerSizeMultiplyer;
    public float damageMultiplier;
    public float bulletRangeMultiplier;
    public float bulletSizeMultiplier;
    public float playerSpeedMultiplier;

    public bool invincibility;

}
