using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBorder : MonoBehaviour
{
    public bool isTrigger = false;
    new Collider2D collider = null;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collider != null)
        {
            collider.isTrigger = !Global.isBattling;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Global.isBattling = true;
        }
        Debug.Log(gameObject.name + " Trigger Exit");
    }
}
