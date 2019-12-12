using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    [Serializable]
    public class Event : UnityEvent<GameObject>
    { }

    public LayerMask onEnterLayers;
    public Event onEnter;

    public LayerMask onStayLayers;
    public Event onStay;

    public LayerMask onExitLayers;
    public Event onExit;

    [HideInInspector]
    public GameObject recordedObject;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("EnterLayer, " + LayerMask.LayerToName(collision.gameObject.layer));
        // other gameobj.layer bit shift
        if ((onEnterLayers & (1 << collision.gameObject.layer)) != 0)
        {
            recordedObject = collision.gameObject;
            onEnter.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((onStayLayers & (1 << collision.gameObject.layer)) != 0)
        {
            onStay.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((onExitLayers & (1 << collision.gameObject.layer)) != 0)
        {
            onExit.Invoke(collision.gameObject);
        }
    }
}
