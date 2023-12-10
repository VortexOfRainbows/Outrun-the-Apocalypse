using System.Collections;
using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.UIElements;

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
    public void PerformUpdate(int SlotNumber)
    {
        Player p = Player.MainPlayer;
        Vector3 mousePos = Utils.MouseWorld();
        bool mouseHovering = C2D.bounds.Contains(new Vector3(mousePos.x, mousePos.y, C2D.bounds.center.z));
        if(mouseHovering) //Manually checking for button collision because unity uses raytracing for 2D buttons for some reason (means buttons cant be pressed when on top of game walls)
        {
            MouseOver();
            if(p.Control.SwapItem && !p.LastControl.SwapItem)
            {
                MouseDown();
            }
            else if (SlotNumber == Player.LeftHandSlotNum && p.Control.LeftClick && !p.Control.LeftClick)
                MouseDown();
            else if (SlotNumber == Player.RightHandSlotNum && p.Control.RightClick && !p.Control.RightClick)
                MouseDown();
        }
        else if(CorrectKeyDown(SlotNumber))
            MouseDown();
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
    public bool CorrectKeyDown(int Number)
    {
        Player p = Player.MainPlayer; 
        //I just want to let you  know. I am in as much pain looking at this as you are
        //I was not able to make an array inside my control storing structs, because they require a external reference to be established.
        //I figured that would be bad to do, as I would need to generate a fresh array every frame.
        //This is the outcome of my decision.
        if (Number == 0 && p.Control.Hotkey1 && !p.LastControl.Hotkey1)
            return true;
        if (Number == 1 && p.Control.Hotkey2 && !p.LastControl.Hotkey2)
            return true;
        if (Number == 2 && p.Control.Hotkey3 && !p.LastControl.Hotkey3)
            return true;
        if (Number == 3 && p.Control.Hotkey4 && !p.LastControl.Hotkey4)
            return true;
        if (Number == 4 && p.Control.Hotkey5 && !p.LastControl.Hotkey5)
            return true;
        if (Number == 5 && p.Control.Hotkey6 && !p.LastControl.Hotkey6)
            return true;
        if (Number == 6 && p.Control.Hotkey7 && !p.LastControl.Hotkey7)
            return true;
        if (Number == 7 && p.Control.Hotkey8 && !p.LastControl.Hotkey8)
            return true;
        if (Number == 8 && p.Control.Hotkey9 && !p.LastControl.Hotkey9)
            return true;
        if (Number == 9 && p.Control.Hotkey0 && !p.LastControl.Hotkey0)
            return true;
        return false;
    }
}
