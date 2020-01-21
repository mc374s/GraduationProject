using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyBehaviour : MonoBehaviour
{
    EnemyController enemyController;
    Character2D character2D;

    public Transform target = null;

    [Range(0, 360)]
    public float attackFov = 30;
    [Range(0, 360)]
    public float attackDirection = 180;
    public float attackDistance = 10;
    public Vector3 offset = Vector3.zero;

    public enum Pattern
    {
        Simple = 0,
        Boss = 1,
        Player = 2
    }

    public Pattern movePattern = Pattern.Simple;


    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        character2D = GetComponent<Character2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.isBattling)
        {
            return;
        }
        switch (movePattern)
        {
            case Pattern.Simple:
                SimpleBehaviour();
                break;
            case Pattern.Boss:
                BossBehaviour();
                break;
            case Pattern.Player:
                enemyController.input.Gain();
                break;
            default:
                break;
        }
    }

    void SimpleBehaviour()
    {
        if (target == null)
        {
            return;
        }
        Vector3 dir = target.position - transform.position - offset;
        //dir = new Vector3(dir.x, 0, dir.z);
        if (Vector3.Angle(Vector3.left, dir) < 80 )
        {
            enemyController.input.Horizontal = -1;
        }
        else if (Vector3.Angle(Vector3.left, dir) > 100)
        {
            enemyController.input.Horizontal = 1;
        }

        if (dir.sqrMagnitude < attackDistance * attackDistance)
        {
            enemyController.input.Horizontal = 0;
            enemyController.input.Attack.Down = true;
        }


    }

    void BossBehaviour()
    {
        enemyController.input.Gain();
    }





#if UNITY_EDITOR
    bool spriteFaceLeft;
    //float height;
    //void OnEnable()
    //{
    //    height = Global.debugUIStartY;
    //    Global.debugUIStartY += 20;
    //}
    void Reset()
    {
        enemyController = GetComponent<EnemyController>();
        character2D = GetComponent<Character2D>();
        spriteFaceLeft = character2D.spriteFaceLeft;
        Debug.Log(character2D.name + " Reset");
    }
    //void OnGUI()
    //{
    //    if (Global.isDebugMenuOpen)
    //    {
    //        GUILayout.BeginArea(new Rect(Global.debugUIStartX, height, 200, 100));
    //        GUILayout.Label("Enemy MoveVector: " + enemyController.MoveVector.ToString());

    //        GUILayout.EndArea();
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + offset;
        //draw the cone of view
        Vector3 forward = (character2D == null ? spriteFaceLeft : character2D.spriteFaceLeft) ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, (character2D == null ? spriteFaceLeft : character2D.spriteFaceLeft) ? -attackDirection : attackDirection) * forward;

        //if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = position + (Quaternion.Euler(0, 0, attackFov * 0.5f) * forward);

        Handles.color = new Color(1.0f, 0, 0, 0.2f);
        Handles.DrawSolidArc(position, -Vector3.forward, (endpoint - position).normalized, attackFov, attackDistance);

        //Draw attack range
        //Handles.color = new Color(1.0f, 0, 0, 0.1f);
        //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
    }

#endif
}
