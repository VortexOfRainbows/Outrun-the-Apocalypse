using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gore : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private Rigidbody2D rb;
    private const float RandomLaunchSpeed = 1f;
    private const float RandomSpeedLaunchMin = 0.5f;
    private const float RandomSpeedLaunchMax = 2.0f;
    private const float AngularSpeedMax = 18.5f;
    /// <summary>
    /// Generates a gore object based on an existing gameobject.
    /// </summary>
    /// <param name="goreParent"></param>
    /// <returns></returns>
    public static GameObject NewGore(GameObject goreParent, Sprite sprite, Vector2 velocity)
    {
        Vector2 randomOffset = new Vector2(RandomLaunchSpeed, 0).RotatedBy(Random.Range(0f, Mathf.PI * 2));
        GameObject gore = Instantiate(PrefabManager.GetPrefab("gore"), goreParent.transform.position, Quaternion.identity);
        gore.transform.localScale = goreParent.transform.lossyScale;
        gore.transform.localRotation = goreParent.transform.rotation;
        Gore g = gore.GetComponent<Gore>();
        g.Renderer.sprite = sprite;
        g.rb.velocity = velocity + randomOffset;
        g.rb.angularVelocity = Random.Range(0, AngularSpeedMax);
        return gore;
    }
    /// <summary>
    /// Generates a gore object based on an existing gameobject.
    /// Requires the gameObject with a SpriteRenderer component
    /// </summary>
    /// <param name="goreParent"></param>
    /// <returns></returns>
    public static GameObject NewGore(GameObject goreParent, Vector2 bonusVelocity = default)
    {
        Vector3 ParentParent = goreParent.transform.parent.position;
        Vector2 awayFromParent = (Vector2)(goreParent.transform.position - ParentParent);
        awayFromParent = awayFromParent.normalized * Mathf.Sqrt(awayFromParent.magnitude);
        return NewGore(goreParent, goreParent.GetComponent<SpriteRenderer>().sprite, awayFromParent * Random.Range(RandomSpeedLaunchMin, RandomSpeedLaunchMax) + bonusVelocity);
    }
    private void Start()
    {
        timeLeft = BaseTimeUntilFadeOut;
    }
    [SerializeField] private float BaseTimeUntilFadeOut = 1200;
    [SerializeField] private float SlowDownMultiplier = 0.945f;
    private float timeLeft;
    private void FixedUpdate()
    {
        rb.velocity *= SlowDownMultiplier;
        rb.angularVelocity *= SlowDownMultiplier;
        timeLeft--;
        if(timeLeft <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Color c = Renderer.color;
            c.a = timeLeft / BaseTimeUntilFadeOut;
            Renderer.color = c;
        }
    }
}
