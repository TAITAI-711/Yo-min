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

        if (!animator.GetBool("isIdle"))
            animator.SetBool("isIdle", true);
    }

    public void SetThrowAnimetion()
    {
        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        if (!animator.GetBool("isWin"))
        {
            animator.SetBool("isWin", true);
            EffectManager.Instance.SetEffect("Light", gameObject.transform.position - new Vector3(0, -8.0f, -5.0f), Quaternion.identity, 8.0f);
        }
    }
}
