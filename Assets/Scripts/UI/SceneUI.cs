using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    public GameObject gameOverImage;
    public GameObject gameClearImage;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameOverImage)
        {
            gameOverImage.SetActive(false);
        }
        if (gameClearImage)
        {
            gameClearImage.SetActive(false);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void GameOver()
    {
        if (gameOverImage)
        {
            gameOverImage.SetActive(true);
        }
    }

    public void GameClear()
    {
        if (gameClearImage)
        {
            gameClearImage.SetActive(true);
        }
    }
}
