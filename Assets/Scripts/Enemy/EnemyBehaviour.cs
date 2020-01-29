using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
    private Vector3 position = Vector3.zero;

    public Transform ground = null;
    public Transform[] movePointCollection;
    //public Vec

    public enum Pattern
    {
        Player = 0,
        BoneEnemy,
        SlimeEnemy,
        BirdBoss,
    }

    public Pattern movePattern = Pattern.BoneEnemy;

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
        position = transform.position + offset;
        switch (movePattern)
        {
            case Pattern.Player:
                enemyController.input.Gain();
                break;
            case Pattern.BoneEnemy:
                BoneEnemyBehaviour();
                break;
            case Pattern.SlimeEnemy:
                SlimeEnemyBehaviour();
                break;
            case Pattern.BirdBoss:
                BirdBossBehaviour();
                break;
            default:
                break;
        }
    }

    void BoneEnemyBehaviour()
    {
        if (target == null)
        {
            return;
        }
        Vector3 dir = target.position - position;
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
    void SlimeEnemyBehaviour()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - position;
        if (dir.sqrMagnitude < attackDistance * attackDistance)
        {
            //dir = new Vector3(dir.x, 0, dir.z);
            if (Vector3.Angle(Vector3.left, dir) < 80)
            {
                enemyController.input.Horizontal = -1;
            }
            else if (Vector3.Angle(Vector3.left, dir) > 100)
            {
                enemyController.input.Horizontal = 1;
            }
            if (character2D.IsGrounded)
            {
                enemyController.input.Attack.Down = true;
            }
        }
    }

    [System.Serializable]
    public class BirdParameter
    {
       public float nearDistance = 15;
       public float[] nearAttackRate = new float[3];
       public float mediumDistance = 40;
       public float[] mediunAttackRate = new float[3];
       public float farDistance = 90;
       public float[] farAttackRate = new float[3];
    }
    [Header("ボス攻撃パラメータ")]
    [SerializeField]
    private BirdParameter birdAttackParameter;

    [SerializeField]
    private float nearDistance = 15;
    [SerializeField]
    private float[] nearAttackRate = new float[3];
    [SerializeField]
    private float mediumDistance = 40;
    [SerializeField]
    private float[] mediunAttackRate = new float[3];
    [SerializeField]
    private float farDistance = 90;
    [SerializeField]
    private float[] farAttackRate = new float[3];

    [SerializeField]
    enum MoveStep
    {
        BattleSatrt = 0,
        LoopStart,
        Loop,
        AttackStart,
        Attack,
        Damaged,
    }
    private MoveStep moveStep = MoveStep.BattleSatrt;

    [Header("ボス移動パラメータ")]
    public float moveToleranceMin = 1;
    public float moveToleranceMax = 7;
    private float moveTolerance = 1;
    private int targetPointIndex = 0;
    [SerializeField]
    private float skillCoolDownTime = 5;
    [SerializeField]
    private float skillCoolDownTimeOnGround = 3;
    private float skillCoolDownTimer = 0;
    private bool isTargetLeft = true;
    [SerializeField]

    void BirdBossBehaviour()
    {
        Vector3 dir;
        if (target == null)
        {
            return;
        }
        if (movePointCollection.Length > 0)
        {
            skillCoolDownTimer += Time.deltaTime;
            switch (moveStep)
            {
                case MoveStep.BattleSatrt:


                    moveStep = MoveStep.LoopStart;
                    break;
                case MoveStep.LoopStart:
                    float sqrMagnitudeMax = float.MaxValue;
                    for (int i = 0; i < movePointCollection.Length; i++)
                    {
                        dir = movePointCollection[i].position - position;
                        if (dir.sqrMagnitude < sqrMagnitudeMax)
                        {
                            sqrMagnitudeMax = dir.sqrMagnitude;
                            targetPointIndex = i;
                        }
                    }
                    dir = target.position - position;
                    isTargetLeft = Vector3.Dot(dir, Vector3.left) > 0 ? true : false;
                    moveTolerance = moveToleranceMin;
                    skillCoolDownTimer = 0;
                    moveStep = MoveStep.Loop;
                    break;
                case MoveStep.Loop:
                    // DropAttack close to target 
                    Vector3 movePointPosition = movePointCollection[targetPointIndex].position;
                    if (enemyController.animator.GetInteger(((BirdEnemyController)enemyController).hashSkillType) == 3)
                    {
                        movePointPosition = target.position;
                        if (character2D.IsGrounded)
                        {
                            moveStep = MoveStep.AttackStart;
                            skillCoolDownTimer = 0;
                        }
                    }
                    Debug.DrawLine(position, movePointPosition);
                    if (position.x < movePointPosition.x - moveTolerance)
                    {
                        enemyController.input.Horizontal = 1;
                    }
                    else if (position.x > movePointPosition.x + moveTolerance)
                    {
                        enemyController.input.Horizontal = -1;
                    }
                    if (position.y < movePointPosition.y - moveTolerance)
                    {
                        enemyController.input.Vertical = 1;
                    }
                    else if (position.y > movePointPosition.y + moveTolerance)
                    {
                        enemyController.input.Vertical = -1;
                    }
                    if (skillCoolDownTimer > skillCoolDownTime)
                    {
                        moveStep = MoveStep.AttackStart;
                        skillCoolDownTimer = 0;
                    }
                    break;
                case MoveStep.AttackStart:
                    if (position.x < target.position.x)
                    {
                        enemyController.input.Horizontal = 1;
                    }
                    else
                    {
                        enemyController.input.Horizontal = -1;
                    }
                    // Towards to target before attack
                    dir = target.position - position;
                    if (Vector3.Dot(transform.right, dir) <= 0)
                    {
                        moveStep = MoveStep.Attack;
                    }
                    break;
                case MoveStep.Attack:
                    Debug.DrawLine(position, target.position, Color.red);
                    if (!character2D.IsGrounded)
                    {
                        enemyController.input.Skill.Down = true;
                        if (Random.Range(0, 100) < 70)
                        {
                            enemyController.input.Vertical = 1;
                        }
                        moveStep = MoveStep.LoopStart;
                    }
                    else
                    {
                        dir = target.position - position;
                        if (dir.sqrMagnitude < nearDistance * nearDistance)
                        {
                            targetPointIndex = movePointCollection.Length / 2 + 1;
                            moveStep = MoveStep.LoopStart;
                        }
                        else if (dir.sqrMagnitude < mediumDistance * mediumDistance)
                        {
                            enemyController.input.Skill.Down = true;
                            if (Random.Range(0, 100) < 30)
                            {
                                enemyController.input.Vertical = -1;
                            }
                            moveStep = MoveStep.AttackStart;
                        }
                        else/* if(dir.sqrMagnitude < farDistance * farDistance)*/
                        {
                            enemyController.input.Skill.Down = true;
                            if (Random.Range(0, 100) < 60)
                            {
                                enemyController.input.Vertical = -1;
                            }
                            moveStep = MoveStep.AttackStart;
                        }
                        if (skillCoolDownTimer > skillCoolDownTimeOnGround)
                        {
                            moveStep = MoveStep.LoopStart;
                        }
                    }
                    break;
                case MoveStep.Damaged:
                    if (true)
                    {

                    }

                    break;

                default:
                    break;
            }
        }
    }
    public void NextMovePoint()
    {
        targetPointIndex = isTargetLeft ? targetPointIndex + 1 : targetPointIndex - 1 + movePointCollection.Length;
        targetPointIndex %= movePointCollection.Length;
        moveTolerance = moveToleranceMin;

    }
    public void MoveAroundObstacle(GameObject other)
    {
        moveTolerance = moveToleranceMax;
        Vector3 otherPosition = other.transform.position + new Vector3(0, -6, 0);
        Debug.DrawLine(position, otherPosition);

        //float angelToNextPoint = Vector3.Angle(Vector3.left, movePointCollection[targetPointIndex].position - position);
        //if (angelToNextPoint > 45 && angelToNextPoint < 135)
        //{
        //    if (position.x < otherPosition.x)
        //    {
        //        enemyController.input.Horizontal = -1;
        //    }
        //    else if (position.x > otherPosition.x)
        //    {
        //        enemyController.input.Horizontal = 1;
        //    }
        //}
        //else
        //{
        //    if (position.y < otherPosition.y)
        //    {
        //        enemyController.input.Vertical = -1;
        //    }
        //    else if (position.y > otherPosition.y)
        //    {
        //        enemyController.input.Vertical = 1;
        //    }
        //}
        //if (position.x < otherPosition.x)
        //{
        //    enemyController.input.Horizontal = -1;
        //}
        //else if (position.x >= otherPosition.x)
        //{
        //    enemyController.input.Horizontal = 1;
        //}
        if (position.y < otherPosition.y)
        {
            enemyController.input.Vertical = -1;
        }
        else if (position.y >= otherPosition.y)
        {
            enemyController.input.Vertical = 1;
        }
    }

    public void EscapeFromMovePoint()
    {
        //moveTolerance = moveToleranceMin;
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
        //Vector3 position = transform.position + offset;
        //draw the cone of view
        Vector3 forward = (character2D == null ? spriteFaceLeft : character2D.spriteFaceLeft) ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, (character2D == null ? spriteFaceLeft : character2D.spriteFaceLeft) ? -attackDirection : attackDirection) * forward;

        //if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = position + (Quaternion.Euler(0, 0, attackFov * 0.5f) * forward);

        Handles.color = new Color(1.0f, 0, 0, 0.2f);
        Handles.DrawSolidArc(position, -Vector3.forward, (endpoint - position).normalized, attackFov, attackDistance);

        if (movePattern == Pattern.BirdBoss)
        {
            Handles.color = new Color(1.0f, 1.0f, 0, 0.1f);
            Handles.DrawSolidArc(position, -Vector3.forward, (endpoint - position).normalized, attackFov, farDistance);
            Handles.color = new Color(1.0f, 0, 0, 0.1f);
            Handles.DrawSolidArc(position, -Vector3.forward, (endpoint - position).normalized, attackFov, mediumDistance);
            Handles.color = new Color(0.0f, 0, 1.0f, 0.1f);
            Handles.DrawSolidArc(position, -Vector3.forward, (endpoint - position).normalized, attackFov, nearDistance);

        }

        //Draw attack range
        //Handles.color = new Color(1.0f, 0, 0, 0.1f);
        //Handles.DrawSolidDisc(transform.position, Vector3.back, meleeRange);
    }

#endif
}
