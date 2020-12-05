using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignMenuManager : MonoBehaviour
{
    Image fadeImage;
    Color fadeImageColor;
    GameObject fadeImageObject;
    string sceneName;
    void Awake()
    {
        Application.targetFrameRate = 300;

        fadeImage = GameObject.Find("Canvas").transform.Find("FadeImage").GetComponent<Image>();
        fadeImageObject = GameObject.Find("Canvas").transform.Find("FadeImage").gameObject;
        
        AudioManager.instance.StartCoroutine("IncreaseVolume");
        StartCoroutine(FadeImageInAnimation());
    }
    public void OnClickBack()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        sceneName = "MainMenu";

        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation(sceneName));
    }

    public void OnClickUpgrade()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        sceneName = "UpgradeMenu";

        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation(sceneName));
    }

    public void OnClickShop()
    {
        Debug.Log("Shop Clicked");
    }

    public void OnClickPlay()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        sceneName = "LevelOne";

        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation(sceneName));
    }

    public void OnClickSkin()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        sceneName = "SkinMenu";

        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation(sceneName));
    }

    //Black Screen Slowly appears.
    IEnumerator FadeImageOutAnimation(string sceneName)
    {
        fadeImageObject.SetActive(true);
        while (true)
        {
            fadeImageColor = fadeImage.color;
            fadeImageColor.a += 0.15f;
            fadeImage.color = fadeImageColor;
            yield return new WaitForSeconds(0.03f);

            if (fadeImageColor.a >= 1)
            {
                SceneManager.LoadScene(sceneName);
                break;
            }
        }
    }

    //Black Screen Slowly disappears.
    IEnumerator FadeImageInAnimation()
    {
        fadeImageColor.a = 1;
        fadeImage.color = fadeImageColor;
        while (true)
        {
            fadeImageColor = fadeImage.color;
            fadeImageColor.a -= 0.15f;
            fadeImage.color = fadeImageColor;
            yield return new WaitForSeconds(0.03f);

            if (fadeImageColor.a <= 0)
            {
                fadeImageObject.SetActive(false);
                break;
            }
        }
    }
}
