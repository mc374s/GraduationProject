//This script lets you load a Scene asynchronously. It uses an asyncOperation to calculate the progress and outputs the current progress to Text (could also be used to make progress bars).

//Attach this script to a GameObject
//Create a Button (Create>UI>Button) and a Text GameObject (Create>UI>Text) and attach them both to the Inspector of your GameObject
//In Play Mode, press your Button to load the Scene, and the Text changes depending on progress. Press the space key to activate the Scene.
//Note: The progress may look like it goes straight to 100% if your Scene doesn’t have a lot to load.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncOperationProgressExample : MonoBehaviour
{
    public Object nextScene;
    private WrappedInput input = new WrappedInput();

    [System.Obsolete]
    void Start()
    {
        //Call the LoadButton() function
        StartCoroutine(LoadScene());

    }
    IEnumerator LoadScene()
    {
        yield return null;
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene.name);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            input.Gain();
            //Debug.Log("Pro :" + asyncOperation.progress);
            //Output the current progress
            //m_Text.text = "Loading... " + (asyncOperation.progress * 100).ToString("F0") + "%";
            //ringImage.fillAmount = asyncOperation.progress / 0.9f;
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //m_Text.text = "Tap screen to continue";
                //Wait to you press the space key to activate the Scene
                if (input.Jump.Down)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}