﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBorder : MonoBehaviour
{
    new Collider2D collider = null;

    public Collider2D[] wallCollection;
    public EnemySpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawner.CheckAlive())
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Global.isBattling = true;
            collider.enabled = false;
            foreach (var item in wallCollection)
            {
                if (item != null)
                {
                    item.isTrigger = !Global.isBattling;
                }
            }
        }
        Debug.Log(gameObject.name + " Trigger Exit");
    }
}
