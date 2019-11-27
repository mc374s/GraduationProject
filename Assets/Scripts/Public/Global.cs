using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    static public bool isBattling = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{

    //}

#if UNITY_EDITOR

    static public float debugUIStartX = 10f;
    static public float debugUIStartY = 10f;
    private float height = debugUIStartY;
    private void OnEnable()
    {
        height = debugUIStartY;
        debugUIStartY += 40;
    }

    static public bool isDebugMenuOpen = true;
    void OnGUI()
    {
        isDebugMenuOpen = GUI.Toggle(new Rect(debugUIStartX, height, 100, 20), isDebugMenuOpen, "DebugMeun");
        isBattling = GUI.Toggle(new Rect(debugUIStartX, height + 20, 100, 20), isBattling, "BattleStart");
    }

#endif


}
