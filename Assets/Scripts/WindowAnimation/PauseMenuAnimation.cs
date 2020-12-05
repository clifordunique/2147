using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimation : MonoBehaviour
{
    GameMenu gameMenu;
    RectTransform pauseMenuRectTransform;
    Vector3 scaleChange;
    float scaleChangeFraction;
    bool resumingGame = false;

    void Awake()
    {
        gameMenu = GameObject.FindWithTag("GameController").GetComponent<GameMenu>();

        pauseMenuRectTransform = gameObject.GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        StartCoroutine(PopupAnimation());
        gameMenu.PauseGame();
    }

    IEnumerator  PopupAnimation()
    {
        while(true)
        {
            scaleChangeFraction = 0.5f - pauseMenuRectTransform.localScale.x;
            scaleChange.x = scaleChangeFraction / 2;
            scaleChange.y = scaleChangeFraction / 2;
            pauseMenuRectTransform.localScale += scaleChange;
            if (pauseMenuRectTransform.localScale.x >= 0.49f && !resumingGame)
            {
                break;
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    // Play Button calls this function.
    public void ExitTween()
    {
        resumingGame = true;
        gameMenu.ResumeGame();
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale/2.5f, 0.1f).setOnComplete(Disable);
    }

    void Disable()
    {
        gameObject.SetActive(false);
        resumingGame = false;
    }
}
