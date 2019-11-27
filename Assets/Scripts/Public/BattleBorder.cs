using System.Collections;
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
        if (!spawner.CheckAlive() && Global.isBattling)
        {
            gameObject.SetActive(false);
            spawner.enabled = false;
            Global.isBattling = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Global.isBattling = true;
            Global.borderLeft = wallCollection[0].transform.position.x/* + wallCollection[0].GetComponentInParent<Transform>().position.x + transform.position.x*/;
            Global.borderRight = wallCollection[1].transform.position.x/* + wallCollection[1].GetComponentInParent<Transform>().position.x + transform.position.x*/;

            collider.enabled = false;
            spawner.enabled = true;
            //foreach (var item in wallCollection)
            //{
            //    if (item != null)
            //    {
            //        item.isTrigger = !Global.isBattling;
            //    }
            //}
            wallCollection[0].isTrigger = !Global.isBattling;
            wallCollection[1].isTrigger = !Global.isBattling;

        }
        Debug.Log(gameObject.name + " Trigger Exit");
    }
}
