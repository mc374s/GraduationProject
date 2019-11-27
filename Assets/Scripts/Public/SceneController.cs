using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static public SceneController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ReloadCurrentScene(0);
            
        }
    }

    public void ReloadCurrentScene(float delayTime)
    {
        StartCoroutine(DelayReload(delayTime));
    }

    IEnumerator DelayReload(float timeSecends)
    {
        yield return new WaitForSeconds(timeSecends);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
