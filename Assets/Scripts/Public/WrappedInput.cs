using UnityEngine;

public class WrappedInput
{
    public class Button
    {
        public bool Down;
        public bool Held;
        public bool Up;
        public void Release()
        {
            Down = false;
            Held = false;
            Up = false;
        }
    }
    public float Horizontal;
    public float Vertical;
    public float HorizontalRaw;
    public float VerticalRaw;

    public Button Jump = new Button();
    public Button Attack = new Button();
    public Button Skill = new Button();
    public Button Roll = new Button();
    public Button Action = new Button();

    public WrappedInput()
    {
        Release();
    }

    public void Gain()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        HorizontalRaw = Input.GetAxisRaw("Horizontal");
        VerticalRaw = Input.GetAxisRaw("Vertical");
        Attack.Down = Input.GetButtonDown("Attack");
        Jump.Down = Input.GetButtonDown("Jump");
        Jump.Held = Input.GetButton("Jump");
        Jump.Up = Input.GetButtonUp("Jump");
        if (Input.GetAxis("Roll") > 0 || Input.GetButtonDown("Roll"))
        {
            Roll.Down = true;
        }
        else
        {
            Roll.Down = false;
        }
        Skill.Down = Input.GetButtonDown("Skill");
        Action.Down = Input.GetButtonDown("Action");
    }

    public void Release()
    {
        Horizontal = 0;
        Vertical = 0;
        Jump.Release();
        Attack.Release();
        Skill.Release();
        Roll.Release();
        Action.Release();
    }
}
