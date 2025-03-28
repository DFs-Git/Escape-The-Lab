using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public SettingsLoader loader;

    public TMP_Dropdown screen;
    public Toggle fullsc;

    // 建立从Dropdown索引到屏幕分辨率的映射
    private Vector2[] indexToScreenSize;

    private void Start()
    {
        // 打表
        indexToScreenSize = new Vector2[5];

        indexToScreenSize[0] = new Vector2(1920, 1080);
        indexToScreenSize[1] = new Vector2(1600, 900);
        indexToScreenSize[2] = new Vector2(1360, 768);
        indexToScreenSize[3] = new Vector2(1024, 768);
        indexToScreenSize[4] = new Vector2(800, 600);

        // 修改各选项为设定的选项
        fullsc.isOn = loader.settings.fullscreen;
        if (loader.settings.width == 1920) screen.value = 0;
        if (loader.settings.width == 1600) screen.value = 1;
        if (loader.settings.width == 1360) screen.value = 2;
        if (loader.settings.width == 1024) screen.value = 3;
        if (loader.settings.width == 800) screen.value = 4;
    }

    // 返回按钮
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    // 应用设置按钮
    public void Apply()
    {
        // 读取Dropdown/Toggle输入
        int screenSizeIndex = screen.value;
        bool fullScreen = fullsc.isOn;

        // 将输入写入SettingsLoader类中的settings
        Vector2 screenSize = indexToScreenSize[screenSizeIndex];
        loader.settings.fullscreen = fullScreen;
        loader.settings.width = (int)screenSize.x;
        loader.settings.height = (int)screenSize.y;

        // 写入文件
        loader.WriteSettings();

        // 应用窗口更改
        Screen.SetResolution(loader.settings.width, loader.settings.height, loader.settings.fullscreen);
    }
}
