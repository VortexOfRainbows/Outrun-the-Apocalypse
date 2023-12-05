using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextBehavior : MonoBehaviour
{
    public float DefaultTextScale = 0.75f;
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }
    public static void SpawnDamageText(int damage, Vector2 position)
    {
        GameObject popupText = Instantiate(PrefabManager.GetPrefab("damageText"), position, new Quaternion());
        popupText.GetComponent<DamageTextBehavior>().Number = damage;
    }
    public const float MaxDuration = 60f;
    private float Duration;
    private int Number;
    private Vector2 DamageTextDirection;
    private float SpeedMult = 0.5f;
    private Vector2 BaseVelocity = Vector2.zero;
    void Start()
    {
        transform.localScale = Vector3.one * DefaultTextScale;
        BaseVelocity = new Vector2(0, 0.75f);
        Duration = MaxDuration;
        DamageTextDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * SpeedMult;
        DamageTextDirection += BaseVelocity;
        DisplayText();
    }
    private void FixedUpdate()
    {
        DisplayText();
        Vector2 TextDirection = DamageTextDirection * Mathf.Sqrt(Duration / MaxDuration);
        transform.position = new Vector3(transform.position.x + TextDirection.x, transform.position.y + TextDirection.y, transform.position.z);
        Duration--;
        if (Duration <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void DisplayText()
    {
        GetComponent<TextMeshPro>().color = Color.red * (Duration / MaxDuration);
        GetComponent<TextMeshPro>().text = Number.ToString();
    }
}
