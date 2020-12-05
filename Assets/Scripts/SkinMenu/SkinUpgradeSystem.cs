using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinUpgradeSystem : MonoBehaviour
{
    int currentSkin;
    public Image defaultSkinButton;
    public Image blueSkinButton;
    public Text defaultSkinText;
    public Text blueSkinText;
    public Sprite selectedButton;
    public Sprite selectButton;
    void Start()
    {
        currentSkin = PlayerPrefs.GetInt("Skin");
        if (currentSkin == 1 || currentSkin == 0)
        {
            OnDefaultSelect();
        }
        else if (currentSkin == 2)
        {
            OnBlueSelect();
        }
    }

    public void OnDefaultSelect()
    {
        defaultSkinButton.sprite = selectedButton;
        defaultSkinText.text = "SELECTED";
        blueSkinButton.sprite = selectButton;
        blueSkinText.text = "SELECT";
        PlayerPrefs.SetInt("Skin", 1);
        print("Skin Changed to 1");
    }

    public void OnBlueSelect()
    {
        defaultSkinButton.sprite = selectButton;
        defaultSkinText.text = "SELECT";
        blueSkinButton.sprite = selectedButton;
        blueSkinText.text = "SELECTED";
        PlayerPrefs.SetInt("Skin", 2);
        print("Skin Changed to 2");
    }
}
