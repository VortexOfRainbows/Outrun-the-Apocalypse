using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class collectWeapon : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject prefab;

    public static collectWeapon instance;
    public GameObject itemSlot;
    public int weaponCount;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
