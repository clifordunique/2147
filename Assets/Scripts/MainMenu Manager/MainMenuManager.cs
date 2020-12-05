using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    Image fadeImage;
    Color fadeImageColor;
    GameObject fadeImageObject;
    void Start()
    {
        fadeImage = GameObject.Find("Canvas").transform.Find("FadeImage").GetComponent<Image>();
        fadeImageObject = GameObject.Find("Canvas").transform.Find("FadeImage").gameObject;
        
        AudioManager.instance.StartCoroutine("IncreaseVolume");
        StartCoroutine(FadeImageInAnimation());
    }

    public void OnClickCampaign()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        //Reducing Background SFX.
        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation());
    }

    //Black Screen Slowly appears.
    IEnumerator FadeImageOutAnimation()
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
                SceneManager.LoadScene("CampaignMenu");
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
