using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    PlayerScript playerScript;
    GameMenu gameMenuScript;
    MiniMapCamera miniMapCamera;
    [SerializeField] public float maxHealth, currentHealth;
    private Transform playerTransform, mainCameraTransform;
    float currentFlopppyCount;
    GameObject fadeImage;
    GameObject reloadingCheckPointText;
    Text floppyCountText, lifeCountText;
    [HideInInspector] public Slider moveSlider, jumpSlider;
    [HideInInspector] public Transform lastCheckpoint;
    Image healthBarFillImage;
    GameObject mainCamera, mapCamera, mapBackbutton;
    //gameMenu is the Menu UI while playing the game.
    //mapMenu is the Menu UI while maximizing the map.
    GameObject gameMenu;
    //Script for pausing and playing the game while maximizing the Map.
    [HideInInspector] public bool isPaused; // isPaused is for NOT processing the inputs if the game is paused.
    [HideInInspector] public bool isMiniMap; // isMiniMap is true when map is Maximized. It is for NOT processing the inputs if Map is maximized. 
    [HideInInspector] public bool gameOver; // gameOver is true when all lives are used.
    private float currentLifeCount, totalLifeCount;
    public float testMagnitude, testRoughness;
    List<Vector2> frameLeftBound, frameRightBound, frameTopBound, frameBottomBound;
    Camera cam;
    [SerializeField] private List<GameObject> listOfFrames;
    int i, totalFrames;
    bool everyFrameIsActive; // everyFrameIsActive is true when every frame in the scene is active(When map is maximized).
    bool isDead; // isDead is true when the currentHealth is less than 0. isDead becomes false when player is respawned.
    
    void Start()
    {
        // Make the game run as fast as possible
        Application.targetFrameRate = 300;
        
        healthBarFillImage = GameObject.FindWithTag("Canvas").transform.Find("HealthBar").Find("GFX").Find("Fill").GetComponent<Image>();
    
        currentHealth = maxHealth = 300;
        currentLifeCount = totalLifeCount = 3;

        fadeImage = GameObject.FindWithTag("Canvas").transform.Find("FadeImage").gameObject;
        reloadingCheckPointText = GameObject.FindWithTag("Canvas").transform.Find("Reloading Checkpoint Text").gameObject;
        fadeImage.SetActive(false);
        reloadingCheckPointText.SetActive(false);


        floppyCountText = GameObject.FindWithTag("Canvas").transform.Find("FloppyCount").Find("FloppyCountText").GetComponent<Text>();
        lifeCountText = GameObject.FindWithTag("Canvas").transform.Find("LifeCount").Find("LifeCountText").GetComponent<Text>();

        lastCheckpoint = transform.Find("OriginPosition").GetComponent<Transform>();

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();

        mainCamera = GameObject.FindWithTag("MainCamera");
        mainCameraTransform = mainCamera.GetComponent<Transform>();
        gameMenu = GameObject.Find("Game Canvas");

        mapCamera = GameObject.Find("Map Camera");
        mapBackbutton = GameObject.Find("Map Canvas").transform.Find("Back Button").gameObject;
        mapBackbutton.SetActive(false);
        miniMapCamera = GameObject.Find("Map Canvas").GetComponent<MiniMapCamera>();

        mapCamera.SetActive(false);

        gameMenuScript = GetComponent<GameMenu>();

        floppyCountText.text = "x 0";
        lifeCountText.text = "x " + totalLifeCount;

        cam = Camera.main;

        //ReadTotalFrames();

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Function for enabling and disabling frames accordingly.
        //if (!isMiniMap)
        //{
        //    SetActiveFrame();
        //}
    }

    //Function to read the total frames present in the scene.
    //Getting positions of the boundaries of each frame.
    //Disabling every frame except the first frame.
    void ReadTotalFrames()
    {
        frameLeftBound = new List<Vector2>();
        frameRightBound = new List<Vector2>();
        frameTopBound = new List<Vector2>();
        frameBottomBound = new List<Vector2>();

        totalFrames = GameObject.Find("ScreenBoxes").transform.childCount;

        //Adding all the frames to the list.
        for (i = 1; i <= totalFrames ; i++)
        {
            string frameName = "Frame" + i ;
            listOfFrames.Add(GameObject.Find("ScreenBoxes").transform.Find(frameName).gameObject);
        }

        //Getting bounds of all the frames and storing inthe list of bounds.
        for (i = 0; i < totalFrames; i++)
        {
            frameLeftBound.Add(listOfFrames[i].transform.Find("LeftBound").transform.position);
            frameRightBound.Add(listOfFrames[i].transform.Find("RightBound").transform.position);
            frameTopBound.Add(listOfFrames[i].transform.Find("TopBound").transform.position);
            frameBottomBound.Add(listOfFrames[i].transform.Find("BottomBound").transform.position);
        }

        //Disabling Every frame except first frame.
        for (i = 1; i < totalFrames; i++)
        {
            listOfFrames[i].SetActive(false);
        }
    }

    //Set the frames that need to be rendered as Active.
    void SetActiveFrame()
    {
        for (i = 0; i < totalFrames; i++)
        {
            //Checking Horizontal position.
            if (cam.WorldToScreenPoint(frameLeftBound[i]).x < Screen.width + Screen.width/12 && (cam.WorldToScreenPoint(frameRightBound[i])).x > -Screen.width/12)
            {
                //Checking Vertical position.
                if (cam.WorldToScreenPoint(frameTopBound[i]).y > 0 && cam.WorldToScreenPoint(frameBottomBound[i]).y < Screen.height)
                {
                    listOfFrames[i].SetActive(true);
                }
                else
                {
                    listOfFrames[i].SetActive(false);
                }
            }
            else
            {
                listOfFrames[i].SetActive(false);
            }
        }
    }
    void SetEveryFrameActive()
    {
        for (i = 0; i < totalFrames; i++)
        {
            listOfFrames[i].SetActive(true);
        }
    }
    
    public void ChangeHealth(float damage)
    {
        currentHealth -= damage;
        
        SetHealth();
    }
    public void SetHealth()
    {
        healthBarFillImage.fillAmount = currentHealth/maxHealth;

        if (currentHealth <= 0 && !isDead)
        {
            Debug.Log("Current Health < 0");
            playerScript.spriteRenderer.enabled = false;

            currentLifeCount --;
            lifeCountText.text = "x " + currentLifeCount;

            if (currentLifeCount > 0)
            {
                isDead = true;

                //FadeIn and FadeOut animations are played if the player can be respawned.
                FadeIn();
            }
            else
            {
                isDead = true;
                gameOver = true;
            }
        }
    }

    #region Player Respawning
    //Black Screen gradually fades in.
    public void FadeIn()
    {
        fadeImage.SetActive(true);
        LeanTween.alpha(fadeImage.GetComponent<RectTransform>(), 1, 1f).setOnComplete(AfterFadeIn);
    }

    //Player Respawns at the last checkpoint.
    //Player properties are set.
    void AfterFadeIn()
    {
        //Respawn at the last checkpoint.
        currentHealth = maxHealth;
        playerTransform.position = lastCheckpoint.position;

        playerScript.spriteRenderer.enabled = true;
        healthBarFillImage.fillAmount = currentHealth / maxHealth;

        reloadingCheckPointText.SetActive(true);

        Invoke("FadeOut", 0.8f);
    }

    //Black Screen gradually fades out.
    void FadeOut()
    {
        reloadingCheckPointText.SetActive(false);
        LeanTween.alpha(fadeImage.GetComponent<RectTransform>(), 0, 1f).setOnComplete(AfterFadeOut);
    }

    //Function called after balck screen fades out.
    //Player can be moved.
    public void AfterFadeOut()
    {
        fadeImage.SetActive(false);

        playerScript.canMove = true;
        isDead = false;
    }
    #endregion

    public void ChangeFloppyCount()
    {
        currentFlopppyCount ++;
        floppyCountText.text = "x " + currentFlopppyCount;
    }

    public void MaximizeMap()
    {
        miniMapCamera.StartCoroutine("BlinkIcon");

        isMiniMap = true;
        //Pausing the game while maximizing the map.
        //gameMenuScript.PauseGame();
        mapBackbutton.SetActive(true);
        mapCamera.SetActive(true);
        mainCamera.SetActive(false);
        gameMenu.SetActive(false);

        Debug.Log("Maximizing Map");
    }

    public void MinimizeMap()
    {
        Debug.Log("Minimizing Map");

        miniMapCamera.StopCoroutine("BlinkIcon");
        miniMapCamera.playerIconImage.enabled = true;

        isMiniMap = false;
        //Resuming the game while minimizing the map.
        //gameMenuScript.ResumeGame();
        mapBackbutton.SetActive(false);
        mapCamera.SetActive(false);
        mainCamera.SetActive(true);
        gameMenu.SetActive(true);
    }
}
