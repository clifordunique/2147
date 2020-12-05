using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private PlayerAction playerAction;
    private PlayerAnimation playerAnimation;
    GameManagerScript gameManagerScript;
    [HideInInspector] public bool red, laser1, laser2;
    private float shieldDuration;
    GameObject playerShield;
    Vector3 spawnPosition;
    [SerializeField] private GameObject missile;
    [HideInInspector] public List<GameObject> enemiesOnScreen;
    List<Vector3> screenPos = new List<Vector3>();
    [HideInInspector] public bool shieldFlag = true, missileFlag = true;
    Camera cam;

    void Awake()
    {
        
        gameManagerScript = gameObject.GetComponent<GameManagerScript>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        playerShield = GameObject.FindWithTag("Player").transform.Find("Shield").gameObject;
    }
    void Start()
    {
        playerAnimation = GameObject.FindWithTag("Player").GetComponent<PlayerAnimation>();
        playerAction = GameObject.FindWithTag("Player").GetComponent<PlayerAction>();
        
        Red();
        shieldDuration = 10;
    }
    public void Red()
    {
        SetAllFalse();
        red = true;
    }

    public void Laser1()
    {
        SetAllFalse();
        laser1 = true;
    }

    public void Laser2()
    {
        SetAllFalse();
        laser2 = true;
    }

    void SetAllFalse()
    {
        red = false;
        laser1 = false;
        laser2 = false;
    }

    public void ShieldOn()
    {
        //ShieldFlag is true after reloading.
        if (shieldFlag == true)
        {
            playerAction.currentShieldCount --;
            shieldFlag = false;
            playerShield.SetActive(true);
            Invoke("ShieldOff", shieldDuration);
        }
    }

    void ShieldOff()
    {
        playerShield.SetActive(false);
    }

    //Function is called when Missile attack is initiated.
    public void AirAttack()
    {
        //MissileFlag is true after Reloading.
        if (missileFlag == true && playerAction.currentMissileCount > 0)
        {
            //Changing the selected gun to Red.
            Red();
            
            int i = 0;
            enemiesOnScreen.Clear();
            screenPos.Clear();

            //Adding every enemy in the scene to the list.
            enemiesOnScreen.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            foreach(var enemy in enemiesOnScreen)
            {
                screenPos.Add(cam.WorldToScreenPoint(enemy.transform.position));
            }

            //Removing enemies that are not in camera space from the list.
            foreach(var position in screenPos)
            {
                if (position.x > Screen.width || position.x < 0 || position.y < 0 || position.y > Screen.height)
                {
                    enemiesOnScreen.RemoveAt(screenPos.IndexOf(position) - i);
                    i ++;
                }
            }

            if (enemiesOnScreen.Count != 0)
            {
                missileFlag = false;
                playerAction.currentMissileCount --;
                
                //Spawning miisiles according to enemy count.
                for (i = 0; i < enemiesOnScreen.Count; i++)
                {
                    float randomPos = Random.Range(-Screen.width/2, Screen.width/2);
                    spawnPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2 + randomPos, Screen.height + 10));
                    spawnPosition.z = 0;

                    //Instantiate missile.
                    Instantiate(missile, spawnPosition, Quaternion.identity);
                }
            }


        }
    }
}
