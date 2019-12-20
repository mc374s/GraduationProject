using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSlash : MonoBehaviour
{
    //private Transform slashTransform;
    public float speed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(speed, 0, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
