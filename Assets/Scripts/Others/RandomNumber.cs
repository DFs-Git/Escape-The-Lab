using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 随机生成 FPS
public class RandomNumber : MonoBehaviour
{
    public TMP_Text fpsText; // 用于显示 FPS 的文本组件

    void Start()
    {
        StartCoroutine(RandomFPS()); // 启动协程以随机生成 FPS
    }

    IEnumerator RandomFPS()
    {
        while (true)
        {
            Random.InitState((int)System.DateTime.Now.Ticks); // 初始化随机数生成器
                                                              // 生成一个 23.50 到 24.50 之间的随机数
            float randomFps = Random.Range(23.50f, 24.50f);
            // 将随机数格式化为字符串并显示在文本组件上
            fpsText.text = "FPS " + randomFps.ToString("F2");

            yield return new WaitForSeconds(1f);
        }
    }
}
