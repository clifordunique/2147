using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeMenuManager : MonoBehaviour
{
    Image fadeImage;
    Color fadeImageColor;
    GameObject fadeImageObject;
    bool imageFaded;

    // Start is called before the first frame update
    void Awake()
    {
        fadeImage = GameObject.Find("Canvas").transform.Find("FadeImage").GetComponent<Image>();
        fadeImageObject = GameObject.Find("Canvas").transform.Find("FadeImage").gameObject;
        
        AudioManager.instance.StartCoroutine("IncreaseVolume");
        StartCoroutine(FadeImageInAnimation());
    }

    public void OnClickBack()
    {
        //Playing Click Sound.
        AudioManager.instance.Play("UI Select");

        AudioManager.instance.StartCoroutine("ReduceVolume");
        StartCoroutine(FadeImageOutAnimation());
    }


    //Black Screen Slowly appears.
    IEnumerator FadeImageOutAnimation()
    {
        fadeImageObject.SetActive(true);
        imageFaded = false;
        while (!imageFaded)
        {
            fadeImageColor = fadeImage.color;
            fadeImageColor.a += 0.15f;
            fadeImage.color = fadeImageColor;
            yield return new WaitForSeconds(0.03f);

            if (fadeImageColor.a >= 1)
            {
                imageFaded = true;
                SceneManager.LoadScene("CampaignMenu");
            }
        }
    }

    //Black Screen Slowly disappears.
    IEnumerator FadeImageInAnimation()
    {
        fadeImageColor.a = 1;
        fadeImage.color = fadeImageColor;
        imageFaded = false;
        while (!imageFaded)
        {
            fadeImageColor = fadeImage.color;
            fadeImageColor.a -= 0.15f;
            fadeImage.color = fadeImageColor;
            yield return new WaitForSeconds(0.03f);

            if (fadeImageColor.a <= 0)
            {
                fadeImageObject.SetActive(false);
                imageFaded = true;
            }
        }
    }
}
