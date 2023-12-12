using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] SpriteRenderer Renderer;
    public const float DespawnTime = 600;
    public float DespawnCounter; //-1 means never will despawn. 0 means it will take 600 more ticks to despawn. 200 = 400 to despawn. etc
    private void Awake()
    {
        DespawnCounter = -1;
    }
    void FixedUpdate()
    {
        if (DespawnCounter >= 0)
        {
            Renderer.color = Color.white * Mathf.Sqrt(1 - DespawnCounter / DespawnTime);
            DespawnCounter++;
            if (DespawnCounter >= DespawnTime)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            CoinCounter.instance.ChangeCoins(1);
            AudioManager.instance.Play("CoinPickup");
        }
    }
}
