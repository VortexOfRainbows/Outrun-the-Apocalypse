using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    private const float PriceVariance = 0.2f;
    public static GameObject NewCapsule(ItemData data, Vector2 pos, int price)
    {
        GameObject capsule = Instantiate(PrefabManager.GetPrefab("capsule"), pos, Quaternion.identity);
        Capsule c = capsule.GetComponent<Capsule>();
        c.AssignItem(data);
        c.Cost = price; 
        //Debug.Log(c.item);
        return capsule;
    }
    public static GameObject NewCapsule(ItemData data, Vector2 pos)
    {
        return NewCapsule(data, pos, (int)(data.Cost * Random.Range(1 - PriceVariance, 1 + PriceVariance) + 0.5f));
    }
    public void AssignItem(ItemData data)
    {
        Slot.UpdateItem(data);
        item = data;
    }
    public void Awake()
    {
        AssignItem(defaultItem.SpawnItem);
    }
    [SerializeField] private GameObject Button;
    [SerializeField] private InventorySlot Slot;
    [SerializeField] private PreSpawnedItem defaultItem;
    [SerializeField] private TMPro.TextMeshPro priceDisplay;
    [SerializeField] private int Cost = 10;
    private ItemData item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            Button.SetActive(true);  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Button.SetActive(false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            AttemptPurchase();
        }
    }
    private void FixedUpdate()
    {
        priceDisplay.text = Cost.ToString();
        if(item != null && item is NoItem)
        {
            Destroy(gameObject);
        }
    }
    private bool dead = false;  
    private void AttemptPurchase()
    {
        if(CoinCounter.instance.CoinCount >= Cost && !dead)
        {
            AudioManager.instance.Play("ZombieDeath");
            CoinCounter.instance.ChangeCoins(-Cost);
            ItemData.NewItem(item, transform.position, new Vector2(Random.Range(-2, 2f), Random.Range(-2, 2f)));
            dead = true;
            Destroy(gameObject);
        }
    }
}

