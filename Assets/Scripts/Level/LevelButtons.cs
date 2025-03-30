using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LevelButtons : MonoBehaviour
{
    public List<GameObject> AllChapters;
    public List<GameObject> AllChaptersButtons;

    public LevelLoader Loader;
    public GameObject Attention;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void Start()
    {
        Attention.SetActive(false);

        // ȷ���ǹؿ���ť�������ؿ���ť��ɫ
        if (gameObject.tag == "levelbtn")
        {
            // ��ȡ��ǰ��ť��Ӧ�Ĺؿ�
            string levelIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
            int chapter = (int)levelIndex[0] - '0';
            int topic = (int)levelIndex[2] - '0';

            UnityEngine.UI.Image img = gameObject.GetComponent<UnityEngine.UI.Image>();

            // ��ȡ��ҽ�����Ϣ
            int completedChapter = PlayerPrefs.GetInt("chapter");
            int completedTopic = PlayerPrefs.GetInt("topic");

            // ����ɵĹؿ�
            if (completedChapter > chapter ||
                (completedChapter == chapter && completedTopic > topic))
            {
                // ����Ϊ��ɫ
                img.color = new Color(0F, 255F, 0F);
            }

            // ��ǰ�ؿ�
            else if (completedChapter == chapter && completedTopic == topic)
            {
                // ����Ϊ��ɫ
                img.color = new Color(255F, 180F, 0F);
            }

            // δ�����ؿ�
            else if (completedChapter < chapter ||
                (completedChapter == chapter && completedTopic < topic))
            {
                // ����Ϊ��ɫ
                img.color = new Color(255F, 0F, 0F);
            }
        }
    }

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
        if (levelIndex[2] == 'X') topic = 10;
        Debug.Log(topic);

        // ȷ��������ɹؿ����߽����йؿ�
        if (chapter <= PlayerPrefs.GetInt("chapter") && topic <= PlayerPrefs.GetInt("topic"))
        {
            Loader.LoadLevel(chapter, topic);
        }
        else
        {
            // ��ʾ������Ϣ
            Attention.SetActive(true);
        }
    }

    public void EnterChapter()
    {
        // ��ȡ��ǰ��ť�ı��ĵڶ��������ģ��ַ�
        string chapterIndex = gameObject.GetComponentInChildren<TMP_Text>().text;
        Debug.Log(chapterIndex[1]);

        for (int i = 0; i < AllChapters.Count; i++)
        {
            string index = AllChaptersButtons[i].GetComponentInChildren<TMP_Text>().text;
            if (index[1] != chapterIndex[1])
            {
                // ���ز��ǵ�ǰ�½ڵ��½�
                AllChapters[i].SetActive(false);
            }
            else
            {
                AllChapters[i].SetActive(true);
            }
        }
    }

    public void Alright()
    {
        Attention.SetActive(false);
    }
}
