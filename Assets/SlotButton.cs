using System.Collections;
using UnityEngine;

public class SlotButton : MonoBehaviour
{
    private BoxCollider2D C2D;
    private SpriteRenderer Renderer;
    private Sprite DefaultSprite;
    private void Awake()
    {
        C2D = GetComponent<BoxCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();
        DefaultSprite = Renderer.sprite;
    }
    [SerializeField] private InventorySlot slot;
    private void MouseDown()
    {
        slot.OnButtonPress();
    }
    private void MouseOver()
    {
        Renderer.color = new Color(1, 0.95f, 0.1f);
        Renderer.sprite = SpriteLib.Library.GetSprite("UI", "SwapItemSlot");
    }
    public void PerformUpdate()
    {
        Vector3 mousePos = Utils.MouseWorld();
        bool mouseHovering = C2D.bounds.Contains(new Vector3(mousePos.x, mousePos.y, C2D.bounds.center.z));
        if(mouseHovering) //Manually checking for button collision because unity uses raytracing for 2D buttons for some reason (means buttons cant be pressed when on top of game walls)
        {
            MouseOver();
            if(Player.MainPlayer.Control.SwapItem && !Player.MainPlayer.LastControl.SwapItem)
            {
                MouseDown();
            }
        }
        else
        {
            Renderer.sprite = DefaultSprite;
            if (Renderer.color.r >= 0.99f && Renderer.color.g >= 0.99f && Renderer.color.b >= 0.99f)
            {
                Renderer.color = Color.white;
            }
            else
            {
                Renderer.color = Color.Lerp(Renderer.color, Color.white, 0.2f);
            }
        }
    }
}
