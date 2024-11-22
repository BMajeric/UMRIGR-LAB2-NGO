using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildManager : MonoBehaviour
{
    private Animator animator;
    MeshRenderer playerChildMeshRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerChildMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void AnimationStart()
    {
        animator.ResetTrigger("Game end");
        animator.SetTrigger("Player win");
    }

    public void AnimationEnd()
    {
        animator.ResetTrigger("Player win");
        animator.SetTrigger("Game end");
    }

    public void SetColor(Color color)
    {
        playerChildMeshRenderer.material.SetColor("_Color", color);
    }

}
