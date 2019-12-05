using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    public LayerMask includeLayers;

    [Serializable]
    public class Event : UnityEvent
    { }
    public Event onEnter;
    public Event onStay;
    public Event onExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("EnterLayer, " + LayerMask.LayerToName(collision.gameObject.layer));
        // other gameobj.layer bit shift
        if ((includeLayers & (1 << collision.gameObject.layer)) != 0)
        {
            onEnter.Invoke();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((includeLayers & (1 << collision.gameObject.layer)) != 0)
        {
            onStay.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((includeLayers & (1 << collision.gameObject.layer)) != 0)
        {
            onExit.Invoke();
        }
    }
}
