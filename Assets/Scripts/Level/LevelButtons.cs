using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelButtons : MonoBehaviour
{
    // �������˵�
    public void BackEvent()
    {
        SceneManager.LoadScene(0);
    }

    // ����ĳ���ؿ�
    public void EnterLevel()
    {
        // ��鰴ť
        string levelIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log(levelIndex);
        int chapter = (int)levelIndex[0] - '0';
        Debug.Log(chapter);
        int topic = (int)levelIndex[2] - '0';
        Debug.Log(topic);
    }
}
