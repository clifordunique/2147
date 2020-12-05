using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignProgress : MonoBehaviour
{
    RectTransform campaignProgressRectTransfrom;
    Vector3 rotationChange;
    [SerializeField] private float speed;

    void Start()
    {
        campaignProgressRectTransfrom = gameObject.GetComponent<RectTransform>();
    }
    void FixedUpdate()
    {
        rotationChange.z += speed;
        campaignProgressRectTransfrom.eulerAngles = rotationChange;
    }

}
