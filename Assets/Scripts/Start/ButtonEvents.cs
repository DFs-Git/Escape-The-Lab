using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    // 开始按钮
    public void StartEvent()
    {
        SceneManager.LoadScene(3);
    }

    // 设置按钮
    public void SettingEvent()
    {
        SceneManager.LoadScene(1);
    }

    // 图鉴按钮
    public void CollectionEvent()
    {
        SceneManager.LoadScene(2);
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
