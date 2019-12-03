using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DisallowMultipleComponent]
public class SingleAnimation : MonoBehaviour
{
    
    public AnimationClip animationClip = null;

    private Animator animator;

    //public float delayTime = 0;
    //private float delayTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Play();
        Destroy(gameObject, animationClip.length);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (delayTimer <= delayTime)
    //    {
    //        delayTimer += Time.deltaTime;
    //        if (delayTimer > delayTime)
    //        {
    //            Play();
    //            Destroy(gameObject, animationClip.length);
    //        }

    //    }
    //}

    public void Play()
    {
        if (animator != null && animationClip != null)
        {
            animator.Play(animationClip.name);
        }
    }

#if UNITY_EDITOR

    //private float height = 0;
    //private void OnEnable()
    //{
    //    height = Global.debugUIStartY;
    //    Global.debugUIStartY += 20;
    //}

    //void OnGUI()
    //{
    //    if (Global.isDebugMenuOpen)
    //    {
    //        if (GUI.Button(new Rect(Global.debugUIStartX, height, 100, 20), animationClip.name))
    //        {
    //            Play();
    //        }
    //    }
    //}

    //void OnDestroy()
    //{
    //    Global.debugUIStartY -= 20;
    //}

#endif
}
