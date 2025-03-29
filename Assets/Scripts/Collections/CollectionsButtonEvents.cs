using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionsButtonEvents : MonoBehaviour
{
   public void BackEvent()
    {
        SceneManager.LoadScene(0);
    }
}
