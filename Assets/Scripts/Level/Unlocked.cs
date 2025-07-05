using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocked : MonoBehaviour
{
    public GameObject FatherLayer;

    public void UnlockedShow()
    {
        FatherLayer.SetActive(true);
    }

    public void UnlockedCommit()
    {
        FatherLayer.SetActive(false);
    }
}
