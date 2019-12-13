using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Transform follow;
    [SerializeField]
    public Transform aim;
    public Vector3 targteOffset = Vector3.zero;
    public float smoothTime = 0.2f;
    public float distance;

    public BoxArea SceneFiled { get; private set; }
    [HideInInspector]
    public Vector3 sceneFiledHalfSize;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;

    //void OnValidate()
    //{
    //    Debug.Log("OnValidate: " + followOffset);
    //    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = followOffset;
    //}

    // Start is called before the first frame update
    void Start()
    {
        SceneFiled = GetComponent<BoxArea>();
        sceneFiledHalfSize = SceneFiled.size * 0.5f;
        distance = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        SmoothFollow();
    }

    public void SmoothFollow()
    {
        sceneFiledHalfSize = SceneFiled.size * 0.5f;
        Vector3 targetPosition = transform.position;
        targetPosition.z = distance;
        Vector3 followPosition = follow.position + targteOffset;
        if (transform.position.x < followPosition.x - sceneFiledHalfSize.x)
        {
            targetPosition.x = followPosition.x - sceneFiledHalfSize.x;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        if (transform.position.x > followPosition.x + sceneFiledHalfSize.x)
        {
            targetPosition.x = followPosition.x + sceneFiledHalfSize.x;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        if (transform.position.y < followPosition.y - sceneFiledHalfSize.y)
        {
            targetPosition.y = followPosition.y - sceneFiledHalfSize.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        if (transform.position.y > followPosition.y + sceneFiledHalfSize.y)
        {
            targetPosition.y = followPosition.y + sceneFiledHalfSize.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }


    }

    public void Zoom(float value, float speed, float duration)
    {
        // Focus on aim
    }

}
