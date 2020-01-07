using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static public SceneController Instance { get; private set; }

    public Object nextScene;

    public BoxArea SceneFiled { get; private set; }
    [HideInInspector]
    public Vector3 leftTop;
    [HideInInspector]
    public Vector3 rightButtom;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SceneFiled = GetComponent<BoxArea>();
        //Vector2 center = new Vector2(transform.position.x + SceneFiled.offset.x, transform.position.y + SceneFiled.offset.y);
        //Vector2 offset = new Vector2(-SceneFiled.size.x * 0.5f, SceneFiled.size.y * 0.5f);
        leftTop = SceneFiled.LeftTop;
        rightButtom = SceneFiled.RightButtom;
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    // Return false: target was low than SceneFiled
    // Return true: target was out of ScenFiled, setting target back to the filed
    //
    public bool SceneFiledCheck(GameObject target)
    {
        if (Utility.BorderCheck(target.transform, leftTop, rightButtom) == Vector3.down)
        {
            return false;
        }

        return true;
    }

    public void ReloadCurrentScene(float delayTime)
    {
        StartCoroutine(Delayload(delayTime, SceneManager.GetActiveScene().name));
    }

    IEnumerator Delayload(float timeSecends,string sceneName)
    {
        yield return new WaitForSeconds(timeSecends);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextScene(float delayTime)
    {
        Debug.Log("nextScene type: " + nextScene.GetType() + ", name: " + nextScene.name);
        StartCoroutine(Delayload(delayTime, nextScene.name));
    }
}
