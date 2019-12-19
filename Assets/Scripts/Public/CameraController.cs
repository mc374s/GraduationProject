using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField]
    public Transform follow;
    private Transform initFollow;
    public Vector3 targteOffset = Vector3.zero;
    [SerializeField]
    private float smoothTime = 0.2f;
    private float initDistance;

    public BoxArea border;
    private Vector3 borderLeftTop;
    private Vector3 borderRightButtom;

    public BoxArea FollowSquare { get; private set; }
    private Vector3 sceneFiledHalfSize;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;

    public Vector3 FollowPosition { get; set; }
    public bool IsZooming { get; private set; }

    //void OnValidate()
    //{
    //    Debug.Log("OnValidate: " + followOffset);
    //    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = followOffset;
    //}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initFollow = follow;
        FollowSquare = GetComponent<BoxArea>();
        sceneFiledHalfSize = FollowSquare.size * 0.5f;
        initDistance = transform.position.z;

        borderLeftTop = border.LeftTop;
        borderRightButtom = border.RightButtom;

        FollowPosition = follow.position;
        IsZooming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsZooming)
        {
            SmoothFollow();
            Utility.BorderCheck(transform, borderLeftTop, borderRightButtom);
        }

        FollowPosition = follow.position;

    }

    public void SmoothFollow()
    {
        sceneFiledHalfSize = FollowSquare.size * 0.5f;

        Vector3 targetPosition = transform.position;
        targetPosition.z = initDistance;
        Vector3 followPosition = FollowPosition + targteOffset;

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

    Vector3 startPosition;
    public void ZoomTo(float newDistance, Vector3 center, float useTime, float delayTime)
    {
        startPosition = transform.position;
        StartCoroutine(ZoomCoroutine(newDistance, center, useTime, delayTime));
    }
    public void ResetZoom(float useTime,float delayTime)
    {
        StartCoroutine(ZoomCoroutine(initDistance, startPosition, useTime, delayTime));
    }
    private IEnumerator ZoomCoroutine(float newDistance, Vector3 center, float useTime, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Vector3 targetPosition = center;
        targetPosition.z = newDistance;
        for (float slerpPos = 0; slerpPos <= useTime; slerpPos += Time.deltaTime)
        {
            transform.position = Vector3.Slerp(startPosition, targetPosition, slerpPos / useTime);
            yield return null;
        }
        transform.position = targetPosition;
        IsZooming = !IsZooming;
    }

}
