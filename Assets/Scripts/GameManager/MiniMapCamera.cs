using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Script attached to Minimap Canvas panel.
public class MiniMapCamera : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    Transform playerTransform;
    RectTransform playerIconTransform;
    RectTransform miniMapOutline;
    [HideInInspector] public Image playerIconImage;
    Transform leftBound;
    Transform rightBound;
    Transform topBound;
    Transform bottomBound;
    Vector2 playerIconTempPosition;
    float xRatio;
    float yRatio;
    float xOffset;
    float yOffset;
    void Awake()
    {
        gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManagerScript>();

        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        playerIconTransform = gameObject.transform.Find("MiniMap Level Outline").Find("Player Icon").GetComponent<RectTransform>();
        miniMapOutline = gameObject.transform.Find("MiniMap Level Outline").GetComponent<RectTransform>();
        playerIconImage = gameObject.transform.Find("MiniMap Level Outline").Find("Player Icon").GetComponent<Image>();

        leftBound = GameObject.FindWithTag("GameController").transform.Find("Left Boundary").transform;
        rightBound = GameObject.FindWithTag("GameController").transform.Find("Right Boundary").transform;
        topBound = GameObject.FindWithTag("GameController").transform.Find("Top Boundary").transform;
        bottomBound = GameObject.FindWithTag("GameController").transform.Find("Bottom Boundary").transform;
    }

    void Update()
    {
        CalculateRatioPosition();
    }

    void CalculateRatioPosition()
    {
        xRatio = playerTransform.position.x - leftBound.position.x;
        xRatio = xRatio / (rightBound.position.x - leftBound.position.x);
        xOffset = xRatio * 5f;

        yRatio = playerTransform.position.y - bottomBound.position.y;
        yRatio = yRatio / (topBound.position.y - bottomBound.position.y);
        yOffset = yRatio * 7f;

        playerIconTempPosition.x = (xRatio * miniMapOutline.sizeDelta.x);
        playerIconTempPosition.y = (yRatio * miniMapOutline.sizeDelta.y) + yOffset;

        playerIconTransform.anchoredPosition = playerIconTempPosition;
    }

    public IEnumerator BlinkIcon()
    {
        while (true)
        {
            if (playerIconImage.enabled)
            {
                yield return new WaitForSeconds(0.7f);
                playerIconImage.enabled = false;
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
                playerIconImage.enabled = true;
            }
        }
    }
}
