using UnityEngine;

public class BoxArea : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public Vector3 size = new Vector3(8f, 8f, 0f);
    //public Rect rect = new Rect(0, 0, 8, 8);

    public Vector3 Center { get { return gameObject.transform.position + offset; } }
    public Vector3 LeftTopOffset { get { return new Vector3(-size.x * 0.5f, size.y * 0.5f, size.z * 0.5f); } }
    public Vector3 LeftTop { get { return Center + LeftTopOffset; } }
    public Vector3 RightButtom { get { return Center - LeftTopOffset; } }

}
