using UnityEngine;

public class Stab : ProjectileData
{
    private const int MaxLife = 13;
    public override string SpriteName => "Stab";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = MaxLife;
        Pierce = 100;
        Friendly = true;
    }
    public override void AfterSpawning(GameObject obj)
    {
        AudioManager.instance.Play("Slash");
        obj.transform.localScale = new Vector3(1f, 0.4f, 1.0f);
    }
    public override void UpdateRenderer(ref SpriteRenderer Renderer)
    {
        Color c = Renderer.color;
        c.a = Mathf.Sqrt(Lifetime / (float)MaxLife);
        Renderer.color = c;
    }
}
