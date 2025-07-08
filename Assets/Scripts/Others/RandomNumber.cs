using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ������� FPS
public class RandomNumber : MonoBehaviour
{
    public TMP_Text fpsText; // ������ʾ FPS ���ı����

    void Start()
    {
        StartCoroutine(RandomFPS()); // ����Э����������� FPS
    }

    IEnumerator RandomFPS()
    {
        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks); // ��ʼ�������������
                                                              // ����һ�� 23.50 �� 24.50 ֮��������
            float randomFps = Random.Range(23.50f, 24.50f);
            // ���������ʽ��Ϊ�ַ�������ʾ���ı������
            fpsText.text = "FPS " + randomFps.ToString("F2");

            yield return new WaitForSeconds(1f);
        }
    }
}
