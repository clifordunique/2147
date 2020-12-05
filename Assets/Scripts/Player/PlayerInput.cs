using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput : MonoBehaviour
{
    PlayerScript playerScript;
    GameManagerScript gameManagerScript;
    bool readyToClear;
    [SerializeReference] private float jumpThreshold; 
    float[] touchStartY = new float[8];
    float[] fireSetter = new float[8];
    private bool swipedRight; //Boolean to keep track of player movement input.
    private bool canJump, canCrouch; //Boolean to keep track of jumps. One touch input can only jump once.
    private int jumpIndex, crouchIndex; //Index of the touch input which caused to jump and crouch.
    [HideInInspector] public float dynamicMoveSpeed;
    
    [Header("Movement Sensitivity")]
    private float changeDirectionThreshold;
    private float increaseSpeedThreshold;
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();

        canJump = true;

        playerScript.horizontal = 0;

        changeDirectionThreshold = Screen.width / 200;
        increaseSpeedThreshold = Screen.width / 600;
    }

    void Update()
    {
        ClearInput();

        // Inputs for movement are taken if the game is not Paused.
        if (!gameManagerScript.isPaused && !gameManagerScript.isMiniMap)
        {
            ProcessInputs();
        }

		playerScript.horizontal = Mathf.Clamp(playerScript.horizontal, -1f, 1f);
    }

    void FixedUpdate()
    {
        readyToClear = true;
    }

    void ClearInput()
    {
        if (!readyToClear)
            return;
        playerScript.horizontal = 0f;
        playerScript.jumpPressed = false;
        playerScript.swipedRight = false;
        playerScript.swipedLeft = false;
        playerScript.firePressed = false;
        playerScript.crouchPressed = false;
        //playerScript.jumpHeld = false;
        readyToClear = false;
    }
    void ProcessInputs()
    {
        
        //Code for Joystick-On-Screen
        /*if (Mathf.Abs(joystick.Horizontal) >= 0.3f)
        {
            playerScript.horizontal += joystick.Horizontal;
        }*/
        if (Input.touchCount > 0)
        {
            Touch[] touches = Input.touches;
            //foreach (Touch t in touches)
            for (int i = 0; i < Input.touchCount; i++)
            {

                //Jumping and Firing.
                //And also crouching :P

                //Setting the limit for getting touches.
                if (touches[i].position.x > (Screen.width/2) && touches[i].position.y < (Screen.height - Screen.height/5))
                {
                    //Starting of the touch.
                    if (touches[i].phase == TouchPhase.Began)
                    {
                        fireSetter[i] = Time.time;
                        touchStartY[i] = touches[i].position.y;
                    }

                    #region Jump Input
                    //Jumping the player if swipe crosses threshold and the touch has previously not made the player jump.
                    //A touch can only make the player jump once.
                    //else if (touches[i].phase == TouchPhase.Moved && touches[i].position.y - touchStartY[i] > jumpThreshold/gameManagerScript.jumpSlider.value && canJump)
                    else if (touches[i].phase == TouchPhase.Moved && touches[i].deltaPosition.y > jumpThreshold)
                    {
                        playerScript.jumpPressed = true;
                        canJump = false;
                        jumpIndex = i;
                    }

                    //Setting the canJump boolean to true when the touch that caused the "previous" jump is ended.
                    else if (touches[i].phase == TouchPhase.Ended && i == jumpIndex)
                    {
                        canJump = true;
                        jumpIndex = -1;
                    }
                    #endregion

                    //Firing the weapon.
                    //else if ((Time.time - fireSetter[i] >= 0.075f  || touches[i].phase == TouchPhase.Ended) && Mathf.Abs(touches[i].position.y - touchStartY[i]) < 0.5)
                    else if ((Time.time - fireSetter[i] >= 0.075f  || touches[i].phase == TouchPhase.Ended) && i != jumpIndex)
                    {
                        playerScript.firePressed = true;
                        fireSetter[i] = Time.time;
                    }
                }

                //Joystick.

                //Setting the limit for getting touches.
                else if (touches[i].position.x < (Screen.width/2) && touches[i].position.y < (Screen.height - Screen.height/5))
                {
                    if (touches[i].phase == TouchPhase.Moved || touches[i].phase == TouchPhase.Stationary)
                    {
                        if (touches[i].deltaPosition.x > changeDirectionThreshold && dynamicMoveSpeed < 1)
                        {
                            if (playerScript.facingRight)
                            {
                                dynamicMoveSpeed += 0.2f;
                            }
                            else
                            {
                                dynamicMoveSpeed = 0.2f;
                            }
                        }
                        else if (touches[i].deltaPosition.x > increaseSpeedThreshold  && dynamicMoveSpeed < 1)
                        {
                            if (playerScript.facingRight)
                            {
                                dynamicMoveSpeed += 0.2f;
                            }
                            else if (dynamicMoveSpeed < -0.6f)
                            {
                                dynamicMoveSpeed += 0.2f;
                            }
                        }
                        else if (touches[i].deltaPosition.x < -changeDirectionThreshold && dynamicMoveSpeed > -1)
                        {
                            if (!playerScript.facingRight)
                            {
                                dynamicMoveSpeed -= 0.2f;
                            }
                            else
                            {
                                dynamicMoveSpeed = -0.2f;
                            }
                        }
                        else if (touches[i].deltaPosition.x < -increaseSpeedThreshold && dynamicMoveSpeed > -1)
                        {
                            if (!playerScript.facingRight)
                            {
                                dynamicMoveSpeed -= 0.2f;
                            }
                            else if (dynamicMoveSpeed > 0.6f)
                            {
                                dynamicMoveSpeed -= 0.2f;
                            }
                        }
                        
                        playerScript.horizontal = dynamicMoveSpeed;

    
                        /*if (touches[i].deltaPosition.x > moveThreshold/gameManagerScript.moveSlider.value)
                        {
                            //swipedRight = true;
                            playerScript.horizontal = 1;
                        }
                        else if (touches[i].deltaPosition.x < -moveThreshold/gameManagerScript.moveSlider.value)
                        {
                            //swipedRight = false;
                            playerScript.horizontal = -1;
                        }*/
                        /*if (swipedRight)
                        {
                            playerScript.horizontal = 1;
                        }
                        else
                        {
                            playerScript.horizontal = -1;
                        }*/
                    /*if (touches[i].phase == TouchPhase.Began)
                    {
                        touchStartX[i] = touches[i].position.x;
                    }*/
                        /*if (touches[i].position.x - touchStartX[i] > moveThreshold/moveSlider.value)
                        {
                            playerScript.horizontal = 1;
                        }
                        else if (touches[i].position.x - touchStartX[i] < -moveThreshold/moveSlider.value)
                        {
                            playerScript.horizontal = -1;
                        }
                    }*/
                    }
                    if (touches[i].phase == TouchPhase.Ended || touches[i].phase == TouchPhase.Canceled)
                    {
                        dynamicMoveSpeed = 0;
                    }
                }
            }
        }
        //playerScript.horizontal = joystick.Horizontal;
        #if UNITY_EDITOR
        playerScript.horizontal = Input.GetAxis("Horizontal");
        playerScript.jumpPressed = playerScript.jumpPressed || Input.GetButtonDown("Jump");
        playerScript.firePressed = playerScript.firePressed || Input.GetButton("Fire");
        playerScript.crouchPressed = playerScript.crouchPressed || Input.GetButton("Crouch");
        //playerScript.jumpHeld		= playerScript.jumpHeld || Input.GetButton("Jump");
        #endif
    }
}
