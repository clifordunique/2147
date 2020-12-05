using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public AnimatorOverrideController blueAnimator;
    PlayerScript playerScript;
    PlayerInput playerInput;
    ActionManager actionManager;
    Animator animator;
    int currentSkinNumber;

    void Awake()
    {
        animator = GetComponent<Animator>();

        currentSkinNumber = PlayerPrefs.GetInt("Skin");
        print(currentSkinNumber);
        
        if (currentSkinNumber == 2)
        {
            animator.runtimeAnimatorController = blueAnimator as RuntimeAnimatorController;
        }
    }

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerInput = GetComponent<PlayerInput>();
        actionManager = GameObject.FindWithTag("GameController").GetComponent<ActionManager>();

    }
    void Update()
    {
        //Changing Run speed accordingly.
        if (Mathf.Abs(playerScript.horizontal) > 0 && Mathf.Abs(playerScript.horizontal) < 0.8f)
        {
            animator.SetFloat("moveSpeed", 0.6f);
        }
        else if (Mathf.Abs(playerScript.horizontal) > 0.8f)
        {
            animator.SetFloat("moveSpeed", 1f);
        }

        if (playerScript.isOnGround)
            animator.SetBool("IsOnGround", true);
        else
            animator.SetBool("IsOnGround", false);
        if (playerScript.horizontal != 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);

        //Making the player stand up while crouching.
        if ((playerScript.isOnGround && playerScript.isCrouching) && (playerScript.jumpPressed || Mathf.Abs(playerScript.horizontal) > 0 ))
        {
            playerScript.isCrouching = false;
            playerScript.canMove = true;
            animator.Play("Stand_Up_Animation");
        }
    }
    public void ChangeCanMove()
    {
        playerScript.canMove = true;
    }
}
