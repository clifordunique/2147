using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuAnimation : MonoBehaviour
{
    GameMenu gameMenu;

    void Awake()
    {
        gameMenu = GameObject.FindWithTag("GameController").GetComponent<GameMenu>();
    }
    void OnEnable()
    {
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>().localScale*2.5f, 0.15f).setOnComplete(gameMenu.PauseGame);
    }
}
