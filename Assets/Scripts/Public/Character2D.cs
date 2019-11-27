using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Character2D : MonoBehaviour
{

    public LayerMask groundedLayerMask;
    public float groundedRaycastDistance = 0.1f;

    SpriteRenderer spriteRenderer;

    new Rigidbody2D rigidbody2D;
    CapsuleCollider2D capsule;
    Vector2 previousPosition;
    Vector2 currentPosition;
    Vector2 nextMovement;
    RaycastHit2D[] hitBuffer = new RaycastHit2D[5];
    RaycastHit2D[] foundHits = new RaycastHit2D[3];

    private ContactFilter2D contactFilter2D;

    public bool spriteFaceLeft = false;
    [HideInInspector]
    public bool doSimpleMove = false;
    

    public bool IsGrounded { get; protected set; }
    public bool IsCeilinged { get; protected set; }
    public Vector2 Velocity { get; protected set; }
    public Rigidbody2D Rigidbody2D { get { return rigidbody2D; } }
    public ContactFilter2D ContactFilter { get { return contactFilter2D; } }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();

        currentPosition = rigidbody2D.position;
        previousPosition = rigidbody2D.position;

        contactFilter2D.layerMask = groundedLayerMask;
        contactFilter2D.useLayerMask = true;
        contactFilter2D.useTriggers = false;

        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!doSimpleMove)
        {
            previousPosition = rigidbody2D.position;
            currentPosition = previousPosition + nextMovement;
            Velocity = (currentPosition - previousPosition) / Time.fixedDeltaTime;

            rigidbody2D.MovePosition(currentPosition);
            nextMovement = Vector2.zero;
        }
        else
        {
            Velocity = rigidbody2D.velocity;
        }


        Collider2DCheck();
    }

    public void Move(Vector2 movement)
    {
        nextMovement += movement;
    }
    


    int count = 0;
    private void Collider2DCheck()
    {
        Vector2 raycastStart;
        Vector2 raycastDirection;
        float raycastDistance;
        if (capsule)
        {
            raycastStart = rigidbody2D.position + capsule.offset + Vector2.down * (capsule.size.y * 0.5f - capsule.size.x * 0.5f);
            raycastDirection = Vector2.down;
            raycastDistance = capsule.size.x * 0.5f + groundedRaycastDistance * 2f;
        }
        else
        {
            raycastStart = rigidbody2D.position + Vector2.up;
            raycastDirection = Vector2.down;
            raycastDistance = 1 + groundedRaycastDistance;
        }
        int count = Physics2D.Raycast(raycastStart, raycastDirection, contactFilter2D, hitBuffer, raycastDistance);
        Debug.DrawLine(raycastStart, raycastStart + raycastDirection * raycastDistance, Color.blue);

        if (count > 0)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }

    }

    public void Flip()
    {
        spriteFaceLeft = !spriteFaceLeft;
        if (spriteRenderer)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        else
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.y += 180;
            transform.eulerAngles = rotation;
            //Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;
        }
    }

#if UNITY_EDITOR

    float height = 0;
    void OnEnable()
    {
        height = Global.debugUIStartY;
        Global.debugUIStartY += 100;
    }

    void OnGUI()
    {
        if (Global.isDebugMenuOpen)
        {
            GUILayout.BeginArea(new Rect(Global.debugUIStartX, height, 200, 200));
            //GUILayout.BeginVertical("box");
            GUILayout.Label("currentPosition: " + currentPosition);
            GUILayout.Label("Raycast hit Count: " + count);
            GUILayout.Label("IsGrounded: " + IsGrounded);
            GUILayout.Label("Rgbd Velocity: " + rigidbody2D.velocity);

            //GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
    private void OnDrawGizmosSelected()
    {
        //Handles.color = new Color(0, 1.0f, 0, 0.2f);
        //Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov, viewDistance);

    }

#endif

}

