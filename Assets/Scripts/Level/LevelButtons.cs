using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelButtons : MonoBehaviour
{
    // 返回主菜单
    public void BackEvent()
    {
        SceneManager.LoadScene(0);
    }

    // 进入某个关卡
    public void EnterLevel()
    {
        // 检查按钮
        string levelIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log(levelIndex);
        int chapter = (int)levelIndex[0] - '0';
        Debug.Log(chapter);
        int topic = (int)levelIndex[2] - '0';
        Debug.Log(topic);
    }
}
