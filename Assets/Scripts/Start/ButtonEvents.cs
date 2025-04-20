using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    public Mask mask;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("DevelopmentMode"))
        {
            PlayerPrefs.SetInt("DevelopmentMode", 0);
            PlayerPrefs.Save();
        }
    }

    // 开始按钮
    public void StartEvent()
    {
        // SceneManager.LoadScene(3);
        StartCoroutine(mask.MaskFadeIn(3));
    }

    // 设置按钮
    public void SettingEvent()
    {
        StartCoroutine(mask.MaskFadeIn(1));
    }

    // 图鉴按钮
    public void CollectionEvent()
    {
        StartCoroutine(mask.MaskFadeIn(2));
    }

    // 退出按钮
    public void ExitEvent()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
