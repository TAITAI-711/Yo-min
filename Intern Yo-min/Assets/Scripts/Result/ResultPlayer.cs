using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material material)
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = material;
    }

    public void SetIdleAnimetion()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        animator.SetBool("isIdle", true);
    }

    public void SetThrowAnimetion()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        animator.SetBool("isWin", true);
    }
}
