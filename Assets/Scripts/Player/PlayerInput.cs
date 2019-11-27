using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton
{
    public bool Down { get; private set; }
    public bool Held { get; private set; }
    public bool Up { get; private set; }

    private string name;

    public bool Enable { get; set; }
    public InputButton(string buttonName)
    {
        Down = false;
        Held = false;
        Up = false;
        Enable = true;
        name = buttonName;
    }
    public void GainInput()
    {
        if (!Enable)
        {
            Down = false;
            Held = false;
            Up = false;
            return;
        }
        Up = Input.GetButtonUp(name);
        Held = Input.GetButton(name);
        Down = Input.GetButtonDown(name);
    }

}

public class InputAxis
{
    public float Value { get; private set; }
    public bool Enable { get; set; }
    private string name;
    public InputAxis(string axisName)
    {
        Value = 0;
        Enable = true;
        name = axisName;
    }

    public void GainInput()
    {
        if (!Enable) 
        {
            Value = 0;
            return;
        }
        Value = Input.GetAxis(name);
    }
    
}




public class PlayerInput : MonoBehaviour
{

    public string horizontal;
    public string vertical;
    public string jump;
    public string attack;
    public string skill;
    public string action;
    //public InputButton Attack=new InputButton()

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
