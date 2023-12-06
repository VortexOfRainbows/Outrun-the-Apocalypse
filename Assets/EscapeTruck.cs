using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;

public class EscapeTruck : MonoBehaviour
{
    private int WheelCount = 0;
    private SpriteResolver resolver;
    private bool BeginEscape = false;
    private void Awake()
    {
        resolver = GetComponent<SpriteResolver>();
    }
    private int EndScreenPopupCounter = 0;
    private int EscapeCounter = 0;
    [SerializeField] private float EscapeEscalationFactor = 20f;
    [SerializeField] private float BeginDrivingAt = 8f;
    [SerializeField] private float MaxEscalation = 10f;
    [SerializeField] private float WobbleMult = 0.036f;
    [SerializeField] private float SpeedMult = 2.5f;
    [SerializeField] private float TimeUntilPopup = 300;
    [SerializeField] private float WobbleDisplacementMax = 0.5f;
    private void FixedUpdate()
    {
        int numWheels = WheelCount;
        if (WheelCount < 0)
            WheelCount = 0;
        if (WheelCount >= 4)
            numWheels = 4;
        resolver.SetCategoryAndLabel("Car", numWheels + "Wheel");
        if (BeginEscape)
        {
            Player.MainPlayer.ImmunityFrames = 60;
            EscapeCounter++;
            float gradualVisual = Mathf.Clamp(EscapeCounter / EscapeEscalationFactor, 0, MaxEscalation);
            float sinusoid = Mathf.Clamp(Mathf.Sin(Mathf.Deg2Rad * EscapeCounter * gradualVisual * 2), -WobbleDisplacementMax, WobbleDisplacementMax) * gradualVisual * WobbleMult;
            transform.position = new Vector2(transform.position.x, transform.position.y + sinusoid);
            if (gradualVisual >= BeginDrivingAt)
            {
                float speed = (gradualVisual - BeginDrivingAt) * SpeedMult;
                transform.position = transform.position - new Vector3(speed, 0, 0);
                if(gradualVisual >= MaxEscalation)
                {
                    EndScreenPopupCounter++;
                    if(EndScreenPopupCounter > TimeUntilPopup)
                        UIManager.instance.GameWon();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            GameObject g = collision.gameObject;
            DroppedItem item = g.GetComponent<DroppedItem>();
            if(item != null)
            {
                if(item.Item is Wheel)
                {
                    Destroy(g);
                    WheelCount++;
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                if (WheelCount >= 4)
                {
                    player.ActivateAnimator(false);
                    collision.collider.transform.position = transform.position;
                    BeginEscape = true;
                }
            }
        }
    }
}
