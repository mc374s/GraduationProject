using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener2D : MonoBehaviour
{
    public Transform follow = null;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        if (follow == null)
        {
            follow = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position + offset;
    }
}
