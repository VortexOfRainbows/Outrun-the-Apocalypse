using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text Text;
    private void FixedUpdate()
    {
        Text.text = Mathf.CeilToInt(Player.MainPlayer.Life).ToString();
    }
}
