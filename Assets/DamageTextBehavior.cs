using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextBehavior : MonoBehaviour
{
    int duration = 0;
    int damage;
    float damageTextDirectionX;
    float damageTextDirectionY;
    // Start is called before the first frame update
    void Start()
    {
        damageTextDirectionX = Random.Range(-1f, 1f);
        damageTextDirectionY = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (duration == 70)
        {
            Destroy(gameObject);
        }
        duration++;
        DisplayText();
    }

    private void DisplayText()
    {
        GetComponent<TextMeshPro>().color = Color.red;
        GetComponent<TextMeshPro>().text = damage.ToString();
        transform.position = new Vector3(transform.position.x + damageTextDirectionX, transform.position.y + damageTextDirectionY, transform.position.z);
    }
}
