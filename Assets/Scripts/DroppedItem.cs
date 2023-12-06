using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Type DefaultItemSpawn;
    public Vector2 Velocity; //Public. so it can be modified in ItemData
    public ItemData Item; //Public. So that ItemData can access this for its general purpose item-spawning method
    private Rigidbody2D RB;
    private SpriteRenderer Renderer;
    private BoxCollider2D Hitbox;
    private int TimeInWorld = 0;
    public void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Hitbox = GetComponent<BoxCollider2D>();
        Renderer.enabled = false;
    }
    public void Start()
    {
        if (Item == null)
        {
            Destroy(this.gameObject);
        }
        Renderer.sprite = Item.sprite;
        FixedUpdate();
        if (!Renderer.enabled)
        {
            Renderer.enabled = true;
            Item.ModifyRenderer(ref Renderer);
        }
    }
    private void FixedUpdate()
    {
        TimeInWorld++;
        RB.velocity = Velocity;
        if (Item == null)
        {
            Destroy(gameObject);
            return;
        }
        Item.DoUpdate(this);
        Item.UpdateRenderer(ref Renderer);
        if (Velocity.x != 0)
            Renderer.flipY = Mathf.Sign(Velocity.x) < 0;
        AdjustHitbox();
    }
    private void AdjustHitbox()
    {
        Hitbox.size = Item.Size;
        Vector2 size = Item.sprite.rect.size;
        Vector2 pivot = Item.sprite.pivot;
        Vector2 offset = -(pivot - size * 0.5f) / Utils.PixelsPerUnit;
        offset.y *= Renderer.flipY ? -1 : 1;
        Hitbox.offset = offset;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && TimeInWorld >= 30)
        {
            if (Player.MainPlayer.AddItemToInventory(Item))
            {
                AudioManager.instance.Play("CoinPickup");
                Destroy(gameObject);
            }
        }
    }
}
