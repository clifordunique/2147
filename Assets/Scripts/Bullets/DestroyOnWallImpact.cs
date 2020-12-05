using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnWallImpact : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
