using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpCount : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Awake()
    {
        text = transform.Find("Text").gameObject.GetComponent<Text>();
    }

    public void ChangeCount(int count)
    {
        text.text = count.ToString();
    }
}
