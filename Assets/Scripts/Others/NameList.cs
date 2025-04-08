using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameList : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene(1);
    }

    public void GoNameList()
    {
        SceneManager.LoadScene(5);
    }
}
