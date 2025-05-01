using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject prompt;
    public string sceneToLoad = "Start"; // Ҫ���صĳ�������

    void Start()
    {

        prompt.SetActive(false);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            prompt.SetActive(true);
        }
        SceneManager.LoadScene(sceneToLoad);

    }
}
