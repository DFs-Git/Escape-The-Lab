using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    // ��ʼ��ť
    public void StartEvent()
    {
        
    }

    // ���ð�ť
    public void SettingEvent()
    {
        SceneManager.LoadScene(1);
    }

    // ͼ����ť
    public void CollectionEvent()
    {

    }

    // �˳���ť
    public void ExitEvent()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
